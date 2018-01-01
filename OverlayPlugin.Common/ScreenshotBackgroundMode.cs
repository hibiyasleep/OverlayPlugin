namespace RainbowMage.OverlayPlugin
{
    public enum ScreenshotBackgroundMode
    {
        Hide = 0,
        Normal = 1,
        Center = 2,
        Fill = 3,
        Uniform = 4,
        UniformToFill = 5,
    }

    public class ScreenshotConfig
    {
        public string SavePath { get; set; }
        public bool AutoClipping { get; set; }
        public string BackgroundImagePath { get; set; }
        public ScreenshotBackgroundMode BackgroundMode { get; set; }
        public int Margin { get; set; }
    }
}
