using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin.Overlays
{
    class LogParseOverlayAddon : IOverlayAddon
    {
        public string Name
        {
            get { return "Log Parse"; }
        }

        public string Description
        {
            get { return "Miniparse + Full Log Access"; }
        }

        public Type OverlayType
        {
            get { return typeof(LogParseOverlay); }
        }

        public Type OverlayConfigType
        {
            get { return typeof(LogParseOverlayConfig); }
        }

        public Type OverlayConfigControlType
        {
            get { return typeof(LogParseConfigPanel); }
        }

        public IOverlay CreateOverlayInstance(IOverlayConfig config)
        {
            return new LogParseOverlay((LogParseOverlayConfig)config);
        }

        public IOverlayConfig CreateOverlayConfigInstance(string name)
        {
            return new LogParseOverlayConfig(name);
        }

        public System.Windows.Forms.Control CreateOverlayConfigControlInstance(IOverlay overlay)
        {
            return new LogParseConfigPanel((LogParseOverlay)overlay);
        }

        public void Dispose()
        {
            
        }
    }
}
