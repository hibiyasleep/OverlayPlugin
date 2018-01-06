namespace RainbowMage.OverlayPlugin
{
    public enum ScreenShotBackgroundMode
    {
        Hide = 0,
        Normal = 1,
        Center = 2,
        Fill = 3,
        Uniform = 4,
        UniformToFill = 5,
    }

    public class ScreenShotConfig
    {
        public string SavePath { get; set; }
        public bool AutoClipping { get; set; }
        public string BackgroundImagePath { get; set; }
        public ScreenShotBackgroundMode BackgroundMode { get; set; }
        public int Margin { get; set; }
    }
}
