using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
