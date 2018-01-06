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
        string ScreenShotSavePath { get; }
        string ScreenShotBackgroundPath { get; }
        int ScreenShotBackgroundMode { get; }
        bool ScreenShotAutoClipping { get; }
        int ScreenShotMargin { get; }
    }
}
