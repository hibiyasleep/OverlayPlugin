using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RainbowMage.OverlayPlugin.Overlays
{
    [Serializable]
    public class LabelOverlay : OverlayBase<LabelOverlayConfig>
    {
        public LabelOverlay(LabelOverlayConfig config, string name)
            : base(config, name)
        {
            timer.Stop();

            Config.TextChanged += (o, e) =>
            {
                UpdateOverlayText();
            };
            Config.HTMLModeChanged += (o, e) =>
            {
                UpdateOverlayText();
            };
            this.Overlay.Renderer.BrowserLoad += (o, e) =>
            {
                UpdateOverlayText();
            };
        }

        public override System.Windows.Forms.Control CreateConfigControl()
        {
            return new LabelOverlayConfigPanel(this);
        }

        private void UpdateOverlayText()
        {
            try
            {
                ExecuteScript(CreateEventDispatcherScript());
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, "Update: {1}", this.Name, ex);
            }
        }

        private string CreateEventDispatcherScript()
        {
            return string.Format(
                "document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', {{ detail: {0} }}));",
                CreateJson());
        }

        internal string CreateJson()
        {
            return string.Format(
                "{{ text: {0}, isHTML: {1} }}",
                JsonConvert.SerializeObject(this.Config.Text),
                this.Config.HtmlModeEnabled ? "true" : "false");
        }

        protected override void Update()
        {
            
        }
    }
}
