using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Advanced_Combat_Tracker;
using RainbowMage.OverlayPlugin.Overlays;

namespace RainbowMage.OverlayPlugin
{
    class WSServer
    {
        HttpServer _server;
        static bool _failed = false;
        static WSServer _inst = null;

        public static EventHandler<StateChangedArgs> OnStateChanged;

        public static void Initialize()
        {
            _inst = new WSServer();
        }

        public static void Stop()
        {
            if (_inst != null)
            {
                try
                {
                    _inst._server.Stop();
                }
                catch (Exception e)
                {
                    Log(LogLevel.Error, Resources.WSShutdownError, e);
                }
                _inst = null;
                _failed = false;

                OnStateChanged(null, new StateChangedArgs(false, false));
            }
        }

        public static bool IsRunning()
        {
            return _inst != null && _inst._server != null && _inst._server.IsListening;
        }

        public static bool IsFailed()
        {
            return _failed;
        }

        public static bool IsSSLPossible()
        {
            return File.Exists(GetCertPath());
        }

        private WSServer()
        {
            var plugin = Registry.Resolve<PluginMain>();
            var cfg = plugin.Config;
            _failed = false;

            try
            {
                var sslPath = GetCertPath();
                var secure = cfg.WSServerSSL && File.Exists(sslPath);

                _server = new HttpServer(IPAddress.Parse(cfg.WSServerIP), cfg.WSServerPort, secure);
                _server.ReuseAddress = true;
                _server.Log.Output += (LogData d, string msg) =>
                {
                    Log(LogLevel.Info, "WS: {0}: {1} {2}", d.Level.ToString(), d.Message, msg);
                };
                _server.Log.Level = WebSocketSharp.LogLevel.Info;

                if (secure)
                {
                    Log(LogLevel.Debug, Resources.WSLoadingCert, sslPath);

                    // IMPORTANT: Do *not* change the password here. This is the default password that mkcert uses.
                    // If you use a different password here, you'd have to pass it to mkcert and update the text on the WSServer tab to match.
                    _server.SslConfiguration.ServerCertificate = new X509Certificate2(sslPath, "changeit");
                    _server.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls12;
                }

                _server.AddWebSocketService<SocketHandler>("/ws");
                _server.AddWebSocketService<LegacyHandler>("/MiniParse");
                _server.AddWebSocketService<LegacyHandler>("/BeforeLogLineRead");

                _server.OnGet += (object sender, HttpRequestEventArgs e) =>
                {
                    if (e.Request.RawUrl == "/")
                    {
                        var builder = new StringBuilder();
                        builder.Append(@"<!DOCTYPE html>
<html>
    <head>
        <title>OverlayPlugin WSServer</title>
    </head>
    <body>
        " + Resources.WSIndexPage + @"
        <ul>");

                        foreach (var overlay in plugin.Overlays)
                        {
                            if (overlay.GetType() != typeof(MiniParseOverlay)) continue;

                            var (confident, url) = GetUrl((MiniParseOverlay) overlay);

                            url = url.Replace("&", "&amp;").Replace("\"", "&quot;");
                            var overlayName = overlay.Name.Replace("&", "&amp;").Replace("<", "&lt;");
                            
                            if (url.StartsWith("file://"))
                            {
                                builder.Append($"<li>Local: {overlayName}: {url}</li>");
                            } else
                            {
                                builder.Append($"<li><a href=\"{url}\">{overlayName}</a>");
                            }
                            
                            if (!confident)
                            {
                                builder.Append(" " + Resources.WSNotConfidentLink);
                            }

                            builder.Append("</li>");
                        }

                        builder.Append("</ul></body></html>");

                        var res = e.Response;
                        res.StatusCode = 200;
                        res.ContentType = "text/html";
                        Ext.WriteContent(res, Encoding.UTF8.GetBytes(builder.ToString()));
                    }
                };

                _server.Start();
                OnStateChanged?.Invoke(this, new StateChangedArgs(true, false));
            }
            catch(Exception e)
            {
                _failed = true;
                Log(LogLevel.Error, Resources.WSStartFailed, e);
                OnStateChanged?.Invoke(this, new StateChangedArgs(false, true));
            }
        }

        public static (bool, string) GetUrl(MiniParseOverlay overlay)
        {
            var cfg = Registry.Resolve<IPluginConfig>();
            string argName = "HOST_PORT";

            if (overlay.ModernApi)
            {
                argName = "OVERLAY_WS";
            }

            var url = Regex.Replace(overlay.Config.Url, @"[?&](?:HOST_PORT|OVERLAY_WS)=[^&]*", "");
            if (url.Contains("?"))
            {
                url += "&";
            } else
            {
                url += "?";
            }

            url += argName + "=ws";
            if (cfg.WSServerSSL) url += "s";
            url += "://" + cfg.WSServerIP + ":" + cfg.WSServerPort + "/";

            if (argName == "OVERLAY_WS") url += "ws";

            return (argName != "HOST_PORT" || overlay.Config.ActwsCompatibility, url);
        }

        public static string GetCertPath()
        {
            var path = Path.Combine(
                ActGlobals.oFormActMain.AppDataFolder.FullName,
                "Config",
                "OverlayPluginSSL.p12");

            return path;
        }

        private static void Log(LogLevel level, string msg, params object[] args)
        {
            PluginMain.Logger.Log(level, msg, args);
        }

        public class SocketHandler : WebSocketBehavior, IEventReceiver
        {
            public void HandleEvent(JObject e)
            {
                SendAsync(e.ToString(Formatting.None), (success) =>
                {
                    if (!success)
                    {
                        Log(LogLevel.Error, Resources.WSMessageSendFailed, e);
                    }
                });
            }

            protected override void OnOpen()
            {

            }

            protected override void OnMessage(MessageEventArgs e)
            {
                JObject data = null;

                try
                {
                    data = JObject.Parse(e.Data);
                }
                catch(JsonException ex)
                {
                    Log(LogLevel.Error, Resources.WSInvalidDataRecv, ex, e.Data);
                    return;
                }

                if (!data.ContainsKey("call")) return;

                var msgType = data["call"].ToString();
                if (msgType == "subscribe")
                {
                    try
                    {
                        foreach (var item in data["events"].ToList())
                        {
                            EventDispatcher.Subscribe(item.ToString(), this);
                        }
                    } catch(Exception ex)
                    {
                        Log(LogLevel.Error, Resources.WSNewSubFail, ex);
                    }

                    return;
                } else if (msgType == "unsubscribe")
                {
                    try
                    {
                        foreach (var item in data["events"].ToList())
                        {
                            EventDispatcher.Unsubscribe(item.ToString(), this);
                        }
                    } catch (Exception ex)
                    {
                        Log(LogLevel.Error, Resources.WSUnsubFail, ex);
                    }
                    return;
                }

                Task.Run(() => {
                    try
                    {
                        var response = EventDispatcher.CallHandler(data);

                        if (response != null && response.Type != JTokenType.Object) {
                            throw new Exception("Handler response must be an object or null");
                        }

                        if (response == null) {
                            response = new JObject();
                            response["$isNull"] = true;
                        }

                        if (data.ContainsKey("rseq")) {
                            response["rseq"] = data["rseq"];
                        }

                        Send(response.ToString(Formatting.None));
                    } catch(Exception ex)
                    {
                        Log(LogLevel.Error, Resources.WSHandlerException, ex);
                    }
                });
            }


            protected override void OnClose(CloseEventArgs e)
            {
                EventDispatcher.UnsubscribeAll(this);
            }
        }

        private class LegacyHandler : WebSocketBehavior, IEventReceiver
        {
            protected override void OnOpen()
            {
                base.OnOpen();

                EventDispatcher.Subscribe("CombatData", this);
                EventDispatcher.Subscribe("LogLine", this);
                EventDispatcher.Subscribe("ChangeZone", this);
                EventDispatcher.Subscribe("ChangePrimaryPlayer", this);

                Send(JsonConvert.SerializeObject(new
                {
                    type = "broadcast",
                    msgtype = "SendCharName",
                    msg = new
                    {
                        charName = FFXIVRepository.GetPlayerName() ?? "YOU",
                        charID = FFXIVRepository.GetPlayerID()
                    }
                }));
            }

            protected override void OnClose(CloseEventArgs e)
            {
                base.OnClose(e);

                EventDispatcher.UnsubscribeAll(this);
            }

            public void HandleEvent(JObject e)
            {
                switch (e["type"].ToString())
                {
                    case "CombatData":
                        Send("{\"type\":\"broadcast\",\"msgtype\":\"CombatData\",\"msg\":" + e.ToString(Formatting.None) + "}");
                        break;
                    case "LogLine":
                        Send("{\"type\":\"broadcast\",\"msgtype\":\"Chat\",\"msg\":" + JsonConvert.SerializeObject(e["rawLine"].ToString()) + "}");
                        break;
                    case "ChangeZone":
                        Send("{\"type\":\"broadcast\",\"msgtype\":\"ChangeZone\",\"msg\":" + e.ToString(Formatting.None) + "}");
                        break;
                    case "ChangePrimaryPlayer":
                        Send("{\"type\":\"broadcast\",\"msgtype\":\"SendCharName\",\"msg\":" + e.ToString(Formatting.None) + "}");
                        break;
                }
            }

            protected override void OnMessage(MessageEventArgs e)
            {
                JObject data = null;

                try
                {
                    data = JObject.Parse(e.Data);
                }
                catch (JsonException ex)
                {
                    Log(LogLevel.Error, Resources.WSInvalidDataRecv, ex, e.Data);
                    return;
                }

                if (!data.ContainsKey("type") || !data.ContainsKey("msgtype")) return;

                switch (data["msgtype"].ToString())
                {
                    case "Capture":
                        Log(LogLevel.Warning, "ACTWS Capture is not supported outside of overlays.");
                        break;
                    case "RequestEnd":
                        ActGlobals.oFormActMain.EndCombat(true);
                        break;
                }
            }
        }

        public class StateChangedArgs : EventArgs
        {
            public bool Running { get; private set; }
            public bool Failed { get; private set; }

            public StateChangedArgs(bool Running, bool Failed)
            {
                this.Running = Running;
                this.Failed = Failed;
            }
        }
    }
}
