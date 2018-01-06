using System;

namespace RainbowMage.OverlayPlugin
{
    public class ExceptionOccuredEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public ExceptionOccuredEventArgs(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
