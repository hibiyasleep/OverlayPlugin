using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace RainbowMage.OverlayPlugin.Overlays
{
    partial class LogParseOverlay : OverlayBase<LogParseOverlayConfig>
    {
        private void LogLineReader(bool isImported, LogLineEventArgs e)
        {
            if (isImported)
            {
                return;
            }
            try
            {
                string[] chunk = e.logLine.Split(new[] { '|' });

                if (chunk.Length < 3) // DataErr0r
                {
                    return;
                }

                if (this.Overlay != null &&
                this.Overlay.Renderer != null &&
                this.Overlay.Renderer.Browser != null)
                {
                    JObject message = new JObject();
                    message["opcode"] = Convert.ToInt32(chunk[0]);
                    message["timestamp"] = chunk[1];
                    message["payload"] = JArray.FromObject(chunk.Skip(2));
                    this.Overlay.Renderer.ExecuteScript(
                        "document.dispatchEvent(new CustomEvent('onLogLine', { detail: " + message.ToString() + " } ));"
                    );
                }
            }
            catch { }
        }
    }
}
