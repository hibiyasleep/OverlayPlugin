using System;

namespace RainbowMage.HtmlRenderer
{
    public class TakeScreenShotEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public TakeScreenShotEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
