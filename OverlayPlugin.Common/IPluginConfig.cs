using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RainbowMage.OverlayPlugin
{
    public interface IPluginConfig
    {
        OverlayConfigList<IOverlayConfig> Overlays { get; set; }
        bool FollowLatestLog { get; set; }
        bool HideOverlaysWhenNotActive { get; set; }
        bool HideOverlayDuringCutscene { get; set; }
        bool ErrorReports { get; set; }
        bool UpdateCheck { get; set; }
        string WSServerIP { get; set; }
        int WSServerPort { get; set; }
        bool WSServerSSL { get; set; }
        bool WSServerRunning { get; set; }
        Version Version { get; set; }
        DateTime LastUpdateCheck { get; set; }
        bool IsFirstLaunch { get; set; }
        Dictionary<string, JObject> EventSourceConfigs { get; set; }
    }
}
