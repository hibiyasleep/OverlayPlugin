using System;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public class MiniParseExtOverlayAddon : IOverlayAddon
    {
        public string Name
        {
            get { return "Mini Parse Extension"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public Type OverlayType
        {
            get { return typeof(MiniParseExtOverlay); }
        }

        public Type OverlayConfigType
        {
            get { return typeof(MiniParseExtOverlayConfig); }
        }

        public Type OverlayConfigControlType
        {
            get { return typeof(MiniParseExtConfigPanel); }
        }

        public IOverlay CreateOverlayInstance(IOverlayConfig config)
        {
            return new MiniParseExtOverlay((MiniParseExtOverlayConfig)config);
        }

        public IOverlayConfig CreateOverlayConfigInstance(string name)
        {
            return new MiniParseExtOverlayConfig(name);
        }

        public System.Windows.Forms.Control CreateOverlayConfigControlInstance(IOverlay overlay)
        {
            return new MiniParseExtConfigPanel((MiniParseExtOverlay)overlay);
        }

        public void Dispose()
        {
            
        }
    }
}
