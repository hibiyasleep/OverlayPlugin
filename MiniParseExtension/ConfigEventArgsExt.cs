using System;
using System.Collections.Generic;
namespace RainbowMage.OverlayPlugin.Overlays
{
    public class ScrPathEventArgs : EventArgs
    {
        public string NewScrPath { get; private set; }
        public ScrPathEventArgs(string path)
        {
            NewScrPath = path;
        }
    }

    public class ShowDebugLogEventArgs : EventArgs
    {
        public bool show { get; private set; }
        public ShowDebugLogEventArgs(bool s)
        {
            show = s;
        }
    }

    public class EnableOnLogLineReadEventArgs : EventArgs
    {
        public bool Enable { get; private set; }
        public EnableOnLogLineReadEventArgs(bool s)
        {
            Enable = s;
        }
    }

    public class EnableBeforeLogLineReadEventArgs : EventArgs
    {
        public bool Enable { get; private set; }
        public EnableBeforeLogLineReadEventArgs(bool s)
        {
            Enable = s;
        }
    }
}
