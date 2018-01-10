using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using System;

namespace RainbowMage.OverlayPlugin.Overlays
{
    partial class MiniParseOverlay : OverlayBase<MiniParseOverlayConfig>
    {        
        private void LogLineReader(bool isImported, LogLineEventArgs e)
        {
            string[] d = e.logLine.Split('|');

            if (d == null || d.Length < 2) // DataErr0r: null or 1-section
            {
                return;
            }

            MessageType type = (MessageType)Convert.ToInt32(d[0]);
            
            switch(type)
            {
                case MessageType.LogLine:
                    if (d.Length < 5) // Invalid
                    {
                        break;
                    }
                    int logType = Convert.ToInt32(d[2], 16);

                    if (logType == 56) // type:echo
                    {
                        sendEchoEvent(isImported, "echo", d[4]);
                    }
                    break;
            }
        }

        private void sendEchoEvent(bool isImported, string type, string text)
        {
            if (this.Overlay != null &&
                this.Overlay.Renderer != null &&
                this.Overlay.Renderer.Browser != null)
            {
                JObject message = new JObject();
                message["isImported"] = isImported;
                message["type"] = type;
                message["message"] = text;
                this.Overlay.Renderer.ExecuteScript(
                    "document.dispatchEvent(new CustomEvent('onLogLine', { detail: " + message.ToString() + " } ));"
                );
            }
        }
    }
}
