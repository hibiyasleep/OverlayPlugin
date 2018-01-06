using System;

namespace RainbowMage.OverlayPlugin
{
    public class LogEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public LogLevel Level { get; private set; }
        public LogEventArgs(LogLevel level, string message)
        {
            this.Message = message;
            this.Level = level;
        }
    }
}
