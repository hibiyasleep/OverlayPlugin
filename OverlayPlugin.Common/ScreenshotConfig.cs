namespace RainbowMage.OverlayPlugin
{
    public class ScreenshotConfig
    {
        public string SavePath { get; set; }
        public bool AutoClipping { get; set; }
        public string BackgroundImagePath { get; set; }
        public ScreenshotBackgroundMode BackgroundMode { get; set; }
        public int Margin { get; set; }
    }
}
