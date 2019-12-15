using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Advanced_Combat_Tracker;
using RainbowMage.HtmlRenderer;
using RainbowMage.OverlayPlugin.Overlays;

namespace RainbowMage.OverlayPlugin
{
    class OverlayApi
    {
        public static event EventHandler<BroadcastMessageEventArgs> BroadcastMessage;
        public static event EventHandler<SendMessageEventArgs> SendMessage;
        public static event EventHandler<SendMessageEventArgs> OverlayMessage;

        IOverlay receiver;

        public OverlayApi(IOverlay receiver)
        {
            this.receiver = receiver;
        }

        public void broadcastMessage(string msg)
        {
            BroadcastMessage(this, new BroadcastMessageEventArgs(msg));
        }

        public void sendMessage(string target, string msg)
        {
            SendMessage(this, new SendMessageEventArgs(target, msg));
        }

        public void overlayMessage(string target, string msg)
        {
            if (target == receiver.Name)
            {
                receiver.OverlayMessage(msg);
            }
            else
            {
                OverlayMessage(this, new SendMessageEventArgs(target, msg));
            }
        }

        public void endEncounter()
        {
            ActGlobals.oFormActMain.EndCombat(true);
        }

        public void setAcceptFocus(bool accept)
        {
            receiver.SetAcceptFocus(accept);
        }

        // Also handles (un)subscription to make switching between this and WS easier.
        public void callHandler(string data, object callback)
        {
            Task.Run(() => {
                try
                {
                    // Tell the overlay that the page is using the modern API.
                    receiver.InitModernAPI();

                    var message = JObject.Parse(data);
                    if (!message.ContainsKey("call"))
                    {
                        PluginMain.Logger.Log(LogLevel.Error, Resources.OverlayApiInvalidHandlerCall, data);
                        return;
                    }
                
                    var handler = message["call"].ToString();
                    if (handler == "subscribe")
                    {
                        if (!message.ContainsKey("events"))
                        {
                            PluginMain.Logger.Log(LogLevel.Error, Resources.OverlayApiMissingEventsField, data);
                            return;
                        }

                        foreach (var name in message["events"].ToList())
                        {
                            EventDispatcher.Subscribe(name.ToString(), (IEventReceiver) receiver);
                            PluginMain.Logger.Log(LogLevel.Debug, Resources.OverlayApiSubscribed, receiver.Name, name.ToString());
                        }
                        return;
                    } else if (handler == "unsubscribe")
                    {
                        if (!message.ContainsKey("events"))
                        {
                            PluginMain.Logger.Log(LogLevel.Error, Resources.OverlayApiMissingEventsFieldUnsub, data);
                            return;
                        }

                        foreach (var name in message["events"].ToList())
                        {
                            EventDispatcher.Unsubscribe(name.ToString(), (IEventReceiver) receiver);
                        }
                        return;
                    }

                    var result = EventDispatcher.CallHandler(message);
                    if (result != null && result.Type != JTokenType.Object) {
                        throw new Exception("Handler response must be an object or null");
                    }
                    Renderer.ExecuteCallback(callback, result == null ? null : result.ToString(Newtonsoft.Json.Formatting.None));
                }
                catch (Exception e)
                {
                    PluginMain.Logger.Log(LogLevel.Error, Resources.JsHandlerCallException, e);
                }
            });
        }
    }

    public class BroadcastMessageEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public BroadcastMessageEventArgs(string message)
        {
            this.Message = message;
        }
    }

    public class SendMessageEventArgs : EventArgs
    {
        public string Target { get; private set; }
        public string Message { get; private set; }

        public SendMessageEventArgs(string target, string message)
        {
            this.Target = target;
            this.Message = message;
        }
    }

    public class EndEncounterEventArgs : EventArgs
    {
        public EndEncounterEventArgs()
        {

        }
    }
}
