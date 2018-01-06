using System;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public class TextChangedEventArgs : EventArgs
    {
        public string Text { get; private set; }
        public TextChangedEventArgs(string text)
        {
            this.Text = text;
        }
    }
}
