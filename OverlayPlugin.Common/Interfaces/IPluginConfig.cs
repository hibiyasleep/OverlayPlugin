using System;

namespace RainbowMage.OverlayPlugin
{
    public interface IPluginConfig
    {
        OverlayConfigList Overlays { get; set; }
        bool FollowLatestLog { get; set; }
        bool HideOverlaysWhenNotActive { get; set; }
        Version Version { get; set; }
        bool IsFirstLaunch { get; set; }
        string ScreenshotSavePath { get; }
        string ScreenshotBackgroundPath { get; }
        int ScreenshotBackgroundMode { get; }
        bool ScreenshotAutoClipping { get; }
        int ScreenshotMargin { get; }
    }
}
