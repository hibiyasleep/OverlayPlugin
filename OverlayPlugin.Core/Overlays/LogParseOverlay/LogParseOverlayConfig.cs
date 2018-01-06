using System;

namespace RainbowMage.OverlayPlugin.Overlays
{
    [Serializable]
    public class LogParseOverlayConfig : OverlayConfigBase
    {
        public LogParseOverlayConfig(string name) : base(name)
        {

        }

        // XmlSerializer用
        private LogParseOverlayConfig() : base(null)
        {

        }

        public override Type OverlayType
        {
            get { return typeof(LogParseOverlay); }
        }
    }
}
