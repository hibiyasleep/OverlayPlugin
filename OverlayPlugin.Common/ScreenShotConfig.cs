namespace RainbowMage.OverlayPlugin
{
    public class ScreenShotConfig
    {
        public string SavePath { get; set; }
        public bool AutoClipping { get; set; }
        public string BackgroundImagePath { get; set; }
        public ScreenShotBackgroundMode BackgroundMode { get; set; }
        public int Margin { get; set; }
    }
}
