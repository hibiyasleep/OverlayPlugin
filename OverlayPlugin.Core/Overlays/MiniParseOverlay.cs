using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RainbowMage.HtmlRenderer;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public partial class MiniParseOverlay : OverlayBase<MiniParseOverlayConfig>
    {
        protected DateTime lastUrlChange;
        protected string lastLoadedUrl;
        public bool ModernApi { get; protected set; }

        public MiniParseOverlay(MiniParseOverlayConfig config, string name)
            : base(config, name)
        {
            Config.ActwsCompatibilityChanged += (o, e) =>
            {
                if (lastLoadedUrl != null && lastLoadedUrl != "about:blank") Navigate(lastLoadedUrl);
            };
            Config.NoFocusChanged += (o, e) =>
            {
                Overlay.SetAcceptFocus(!Config.NoFocus);
            };
            Config.ZoomChanged += (o, e) =>
            {
                Overlay.Renderer.SetZoomLevel(Config.Zoom / 100.0);
            };

            Overlay.Renderer.BrowserStartLoading += PrepareWebsite;
            Overlay.Renderer.BrowserLoad += FinishLoading;
            Overlay.Renderer.BrowserConsoleLog += Renderer_BrowserConsoleLog;
        }

        public override Control CreateConfigControl()
        {
            return new MiniParseConfigPanel(this);
        }

        private void Renderer_BrowserConsoleLog(object sender, BrowserConsoleLogEventArgs e)
        {
            if (Config.ActwsCompatibility && e.Message.Contains("ws://127.0.0.1/") && (e.Message.Contains("SecurityError:") || e.Message.Contains("ERR_CONNECTION_")))
            {
                Overlay.Reload();
            }
        }

        private void FinishLoading(object sender, BrowserLoadEventArgs e)
        {
            if (Config.ActwsCompatibility)
            {
                var charName = JsonConvert.SerializeObject(FFXIVRepository.GetPlayerName() ?? "YOU");
                var charID = JsonConvert.SerializeObject(FFXIVRepository.GetPlayerID());

                ExecuteScript(@"var msg = { charName: " + charName + ", charID: " + charID + @" };
                if (window.__OverlayPlugin_ws_faker) {
                    __OverlayPlugin_ws_faker({ type: 'broadcast', msgtype: 'SendCharName', msg });
                } else {
                    window.__OverlayPlugin_char_msg = msg;
                }");
            }
        }

        private void PrepareWebsite(object sender, BrowserLoadEventArgs e)
        {
            lastLoadedUrl = e.Url;
            Config.Url = e.Url;

            // Reset page-specific state
            Overlay.Renderer.SetZoomLevel(Config.Zoom / 100.0);
            ModernApi = false;

            if (Config.ActwsCompatibility)
            {
                Overlay.SetAcceptFocus(!Config.NoFocus);

                var shimMsg = Resources.ActwsShimEnabled;

                // Install a fake WebSocket so we can directly call the event handler.
                ExecuteScript(@"(function() {
                    let realWS = window.WebSocket;
                    let queue = [];
                    window.__OverlayPlugin_ws_faker = (msg) => queue.push(msg);
                    window.overlayWindowId = 'ACTWS_shim';

                    window.WebSocket = function(url) {
                        if (url.indexOf('ws://127.0.0.1/fake/') > -1)
                        {
                            window.__OverlayPlugin_ws_faker = (msg) => {
                                if (this.onmessage) this.onmessage({ data: JSON.stringify(msg) });
                            };
                            this.close = () => null;
                            this.send = (msg) => {
                                if (msg === '.') return;

                                msg = JSON.parse(msg);
                                switch (msg.msgtype) {
                                    case 'Capture':
                                        OverlayPluginApi.captureOverlay();
                                        break;
                                    case 'RequestEnd':
                                        OverlayPluginApi.endEncounter();
                                        break;
                                }
                            };

                            console.log(" + JsonConvert.SerializeObject(shimMsg) + @");

                            if (queue !== null) {
                                setTimeout(() => {
                                    queue.forEach(__OverlayPlugin_ws_faker);
                                    queue = null;

                                    if (window.__OverlayPlugin_char_msg) this.__OverlayPlugin_ws_faker(window.__OverlayPlugin_char_msg);
                                }, 100);
                            }
                        }
                        else
                        {
                            return new realWS(url);
                        }
                    };
                })();");

                Subscribe("CombatData");
                Subscribe("LogLine");
                Subscribe("ChangeZone");
                Subscribe("ChangePrimaryPlayer");
            } else {
                // Reset page-specific state
                Overlay.SetAcceptFocus(false);

                // Subscriptions are cleared on page navigation so we have to restore this after every load.
                Subscribe("CombatData");
                Subscribe("LogLine");
            }
        }

        public override void Navigate(string url)
        {
            if (Config.ActwsCompatibility)
            {
                if (!url.Contains("HOST_PORT=") && url != "about:blank")
                {
                    if (!url.EndsWith("?"))
                    {
                        if (url.Contains("?"))
                        {
                            url += "&";
                        } else
                        {
                            url += "?";
                        }
                    }
                    url += "HOST_PORT=ws://127.0.0.1/fake/";
                }
            } else
            {
                int pos = url.IndexOf("HOST_PORT=");
                if (pos > -1 && url.Contains("/fake/"))
                {
                    url = url.Substring(0, pos).Trim(new char[] { '?', '&' });
                }
            }

            // If this URL was just loaded (see PrepareWebsite), ignore this request since we're loading that URL already.
            if (url == lastLoadedUrl) return;

            lastUrlChange = DateTime.Now;
            if (url != Overlay.Url)
            {
                base.Navigate(url);
            }
        }

        public override void Reload()
        {
            // If the user changed the URL less than a second ago, ignore the reload since it would interrupt
            // the currently loading overlay and end up with an empty page.
            // The user probably just wanted to load the page so doing nothing here (in that case) is fine.

            if (DateTime.Now - lastUrlChange > new TimeSpan(0, 0, 1))
            {
                base.Reload();
            }
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public override void HandleEvent(JObject e)
        {
            if (Config.ActwsCompatibility)
            {
                // NOTE: Keep this in sync with WSServer's LegacyHandler.
                switch (e["type"].ToString())
                {
                    case "CombatData":
                        ExecuteScript("__OverlayPlugin_ws_faker({'type': 'broadcast', 'msgtype': 'CombatData', 'msg': " + e.ToString(Formatting.None) + " });");
                        break;
                    case "LogLine":
                        ExecuteScript("__OverlayPlugin_ws_faker({'type': 'broadcast', 'msgtype': 'Chat', 'msg': " + JsonConvert.SerializeObject(e["rawLine"].ToString()) + " });");
                        break;
                    case "ChangeZone":
                        ExecuteScript("__OverlayPlugin_ws_faker({'type': 'broadcast', 'msgtype': 'ChangeZone', 'msg': " + e.ToString(Formatting.None) + " });");
                        break;
                    case "ChangePrimaryPlayer":
                        ExecuteScript("__OverlayPlugin_ws_faker({'type': 'broadcast', 'msgtype': 'SendCharName', 'msg': " + e.ToString(Formatting.None) + " });");
                        break;
                }
            }
            else if(ModernApi)
            {
                base.HandleEvent(e);
            }
            else
            {
                // Old OverlayPlugin API
                switch (e["type"].ToString())
                {
                    case "CombatData":
                        ExecuteScript("document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', { detail: " + e.ToString(Formatting.None) + " }));");
                        break;
                    case "LogLine":
                        ExecuteScript("document.dispatchEvent(new CustomEvent('onLogLine', { detail: " + JsonConvert.SerializeObject(e["line"].ToString(Formatting.None)) + " }));");
                        break;
                }
            }
        }

        public override void InitModernAPI()
        {
            if (!ModernApi)
            {
                // Clear the subscription set in PrepareWebsite().
                Unsubscribe("CombatData");
                Unsubscribe("LogLine");
                ModernApi = true;
            }
        }

        protected override void Update()
        {
            
        }
    }
}
