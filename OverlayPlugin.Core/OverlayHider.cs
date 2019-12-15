using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RainbowMage.OverlayPlugin
{
    class OverlayHider
    {
        static bool gameActive = true;
        static bool inCutscene = false;
        static IPluginConfig config;
        static ILogger logger;
        static PluginMain main;

        public static void Initialize()
        {
            config = Registry.Resolve<IPluginConfig>();
            logger = Registry.Resolve<ILogger>();
            main = Registry.Resolve<PluginMain>();

            NativeMethods.ActiveWindowChanged += ActiveWindowChangedHandler;
            NetworkParser.OnOnlineStatusChanged += OnlineStatusChanged;
        }

        public static void UpdateOverlays()
        {
            if (!config.HideOverlaysWhenNotActive)
                gameActive = true;

            if (!config.HideOverlayDuringCutscene)
                inCutscene = false;

            try
            {
                foreach (var overlay in main.Overlays)
                {
                    if (overlay.Config.IsVisible) overlay.Visible = gameActive && !inCutscene;
                }
            } catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"OverlayHider: Failed to update overlays: {ex}");
            }
        }

        static void ActiveWindowChangedHandler(object sender, IntPtr changedWindow)
        {
            if (!config.HideOverlaysWhenNotActive || changedWindow == IntPtr.Zero) return;
            try
            {
                try
                {
                    NativeMethods.GetWindowThreadProcessId(NativeMethods.GetForegroundWindow(), out uint pid);

                    if (pid == 0)
                        return;

                    var exePath = Process.GetProcessById((int)pid).MainModule.FileName;
                    var fileName = Path.GetFileName(exePath.ToString());
                    gameActive = (fileName == "ffxiv.exe" || fileName == "ffxiv_dx11.exe" ||
                                    exePath.ToString() == Process.GetCurrentProcess().MainModule.FileName);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    // Ignore access denied errors. Those usually happen if the foreground window is running with
                    // admin permissions but we are not.
                    if (ex.ErrorCode == -2147467259)  // 0x80004005
                    {
                        gameActive = false;
                    }
                    else
                    {
                        logger.Log(LogLevel.Error, "XivWindowWatcher: {0}", ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "XivWindowWatcher: {0}", ex.ToString());
            }

            UpdateOverlays();
        }

        static void OnlineStatusChanged(object sender, OnlineStatusChangedArgs e)
        {
            if (!config.HideOverlayDuringCutscene || e.Target != FFXIVRepository.GetPlayerID()) return;

            inCutscene = e.Status == 15;
            UpdateOverlays();
        }
    }
}
