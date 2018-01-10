using System;
using Xilium.CefGlue;

namespace RainbowMage.HtmlRenderer
{
    class BuiltinFunctionHandler : CefV8Handler
    {
        public event EventHandler<TakeScreenshotEventArgs> TakeScreenshot;
        public event EventHandler<BroadcastMessageEventArgs> BroadcastMessage;
        public event EventHandler<SendMessageEventArgs> SendMessage;
        public event EventHandler<SendMessageEventArgs> OverlayMessage;
        public event EventHandler<EndEncounterEventArgs> EndEncounter;

        public const string TakeScreenshotFunctionName = "takeScreenshot";
        public const string BroadcastMessageFunctionName = "broadcastMessage";
        public const string SendMessageFunctionName = "sendMessage";
        public const string OverlayMessageFunctionName = "overlayMessage";
        public const string EndEncounterFunctionName = "endEncounter";

        protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
        {
            exception = "";
            returnValue = CefV8Value.CreateUndefined();

            if (name == EndEncounterFunctionName)
            {
                EndEncounter?.Invoke(obj, new EndEncounterEventArgs());
            }
            else if (name == TakeScreenshotFunctionName)
            {
                if (arguments.Length > 0)
                {
                    TakeScreenshot?.Invoke(obj, new TakeScreenshotEventArgs(arguments[0].GetStringValue()));
                }
                else
                {
                    exception = "Invalid argument count.";
                }

                return true;
            }
            else if (name == BroadcastMessageFunctionName)
            {
                if (arguments.Length > 0)
                {
                    BroadcastMessage?.Invoke(obj, new BroadcastMessageEventArgs(arguments[0].GetStringValue()));
                }
                else
                {
                    exception = "Invalid argument count.";
                }

                return true;
            }
            else if (name == SendMessageFunctionName)
            {
                if (arguments.Length > 1)
                {
                    SendMessage?.Invoke(obj, new SendMessageEventArgs(arguments[0].GetStringValue(), arguments[1].GetStringValue()));
                }
                else
                {
                    exception = "Invalid argument count.";
                }

                return true;
            }
            else if (name == OverlayMessageFunctionName)
            {
                if (arguments.Length > 1)
                {
                    OverlayMessage?.Invoke(obj, new SendMessageEventArgs(arguments[0].GetStringValue(), arguments[1].GetStringValue()));
                }
                else
                {
                    exception = "Invalid argument count.";
                }

                return true;
            }

            return false;
        }
    }
}
