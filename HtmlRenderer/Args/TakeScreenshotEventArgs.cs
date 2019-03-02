using System;

namespace RainbowMage.HtmlRenderer
{
    public class TakeScreenshotEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public TakeScreenshotEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
