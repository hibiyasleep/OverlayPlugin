using Advanced_Combat_Tracker;
using RainbowMage.HtmlRenderer;
using RainbowMage.OverlayPlugin.Overlays;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
<<<<<<< HEAD
<<<<<<< HEAD
using System.Windows.Forms;
=======
using System.Threading;
>>>>>>> Remove Error Cause
=======
>>>>>>> Revert "Remove Error Cause"

namespace RainbowMage.OverlayPlugin
{
    public class PluginMain
    {
        TabPage tabPage;
        Label label;
        ControlPanel controlPanel;

        internal static readonly string DefaultScreenShotPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ScreenShot");
        internal PluginConfig Config { get; private set; }
        internal List<IOverlay> Overlays { get; private set; }
        internal List<IOverlayAddon> Addons { get; set; }
        internal Logger Logger { get; set; }
        internal string PluginDirectory { get; private set; }

        public PluginMain(string pluginDirectory, Logger logger)
        {
            PluginDirectory = pluginDirectory;
            Logger = logger;
        }

        /// <summary>
        /// プラグインが有効化されたときに呼び出されます。
        /// </summary>
        /// <param name="pluginScreenSpace"></param>
        /// <param name="pluginStatusText"></param>
        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            try
            {
                tabPage = pluginScreenSpace;
                label = pluginStatusText;
#if DEBUG
                Logger.Log(LogLevel.Warning, "##################################");
                Logger.Log(LogLevel.Warning, "    THIS IS THE DEBUG BUILD");
                Logger.Log(LogLevel.Warning, "##################################");
#endif

                Logger.Log(LogLevel.Info, "InitPlugin: PluginDirectory = {0}", this.PluginDirectory);


                // プラグイン読み込み
                LoadAddons();
                // コンフィグ系読み込み
                LoadConfig();

                // プラグイン間のメッセージ関連
                Renderer.BroadcastMessage += BroadcaseMessageEvent;
                Renderer.SendMessage += SendMessageEvent;
                Renderer.OverlayMessage += OverlayMessageEvent;
                Renderer.TakeScreenShotHandler += RendererTakeScreenShotEvent;
                Renderer.RendererFeatureRequest += RendererFeatureRequestEvent;
                
                // ACT 終了時に CEF をシャットダウン（ゾンビ化防止）
                Application.ApplicationExit += (o, e) =>
                {
                    try { Renderer.Shutdown(); }
                    catch { }
                };

                InitializeOverlays();

                // コンフィグUI系初期化
                controlPanel = new ControlPanel(this, Config);
                controlPanel.Dock = DockStyle.Fill;
                tabPage.Controls.Add(controlPanel);

                Logger.Log(LogLevel.Info, "InitPlugin: Initialized.");
                label.Text = "Initialized.";
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "InitPlugin: {0}", e.ToString());
                MessageBox.Show(e.ToString());

                throw;
            }
        }

        private void BroadcaseMessageEvent(object sender, BroadcastMessageEventArgs e)
        {
            Task.Run(() =>
            {
                foreach (var overlay in Overlays)
                {
                    overlay.SendMessage(e.Message);
                }
            });
        }

        private void SendMessageEvent(object sender, SendMessageEventArgs e)
        {
            Task.Run(() =>
            {
                var targetOverlay = Overlays.FirstOrDefault(x => x.Name == e.Target);
                if (targetOverlay != null)
                {
                    targetOverlay.SendMessage(e.Message);
                }
            });
        }

        private void OverlayMessageEvent(object sender, SendMessageEventArgs e)
        {
            Task.Run(() =>
            {
                var targetOverlay = Overlays.FirstOrDefault(x => x.Name == e.Target);
                if (targetOverlay != null)
                {
                    targetOverlay.OverlayMessage(e.Message);
                }
            });
        }

        /// <summary>
        /// Take Screenshot target overlay (API: window.overlayApi.takeScreenShot( window.overlayApi.takeScreenShot))
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RendererTakeScreenShotEvent(object sender, TakeScreenShotEventArgs e)
        {
            Task.Run(() =>
            {
                foreach(var i in Overlays)
                {
                    if (i.Name == e.Message)
                    {
                        i.TakeScreenShot(
                            new ScreenshotConfig
                            {
                                SavePath            = Config.ScreenShotSavePath,
                                AutoClipping        = Config.ScreenShotAutoClipping,
                                BackgroundImagePath = Config.ScreenShotBackgroundPath,
                                BackgroundMode      = (ScreenshotBackgroundMode)Config.ScreenShotBackgroundMode,
                                Margin              = Config.ScreenShotMargin,
                            });
                        break;
                    }
                }
            });
        }

        private void RendererFeatureRequestEvent(object sender, RendererFeatureRequestEventArgs e)
        {
            Task.Run(() =>
            {
                if (e.Request == "EndEncounter")
                {
                    ActGlobals.oFormActMain.EndCombat(true);
                }
            });
        }

        /// <summary>
        /// コンフィグのオーバーレイ設定を基に、オーバーレイを初期化・登録します。
        /// </summary>
        private void InitializeOverlays()
        {
            // オーバーレイ初期化
            this.Overlays = new List<IOverlay>();
            foreach (var overlayConfig in this.Config.Overlays)
            {
                var addon = this.Addons.FirstOrDefault(x => x.OverlayType == overlayConfig.OverlayType);
                if (addon != null)
                {
                    var overlay = addon.CreateOverlayInstance(overlayConfig);
                    RegisterOverlay(overlay);
                }
                else
                {
                    Logger.Log(LogLevel.Error, "InitPlugin: Could not find addon for {0}.", overlayConfig.Name);
                }
            }
        }

        /// <summary>
        /// オーバーレイを登録します。
        /// </summary>
        /// <param name="overlay"></param>
        internal void RegisterOverlay(IOverlay overlay)
        {
            overlay.OnLog += (o, e) => Logger.Log(e.Level, e.Message);
            overlay.PluginConfig = (IPluginConfig)Config;
            overlay.Start();
            this.Overlays.Add(overlay);
        }

        /// <summary>
        /// 登録されているオーバーレイを削除します。
        /// </summary>
        /// <param name="overlay">削除するオーバーレイ。</param>
        internal void RemoveOverlay(IOverlay overlay)
        {
            overlay.Dispose();
            this.Overlays.Remove(overlay);
        }

        /// <summary>
        /// プラグインが無効化されたときに呼び出されます。
        /// </summary>
        public void DeInitPlugin()
        {
            SaveConfig();

            foreach (var overlay in this.Overlays)
            {
                overlay.Dispose();
            }

            foreach (var addon in this.Addons)
            {
                addon.Dispose();
            }

            Logger.Log(LogLevel.Info, "DeInitPlugin: Finalized.");
            this.label.Text = "Finalized.";
        }

        /// <summary>
        /// アドオンを読み込みます。
        /// </summary>
        private void LoadAddons()
        {
            try
            {
                // <プラグイン本体があるディレクトリ>\plugins\*.dll を検索する
                var directory = Path.Combine(PluginDirectory, "addons");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                this.Addons = new List<IOverlayAddon>();

                // 内蔵アドオンを追加
                this.Addons.Add(new MiniParseOverlayAddon());
                this.Addons.Add(new SpellTimerOverlayAddon());
                this.Addons.Add(new LabelOverlayAddon());
                this.Addons.Add(new LogParseOverlayAddon());

                var version = typeof(PluginMain).Assembly.GetName().Version;

                foreach (var pluginFile in Directory.GetFiles(directory, "*.dll"))
                {
                    try
                    {
                        Logger.Log(LogLevel.Info, "LoadAddons: {0}", pluginFile);

                        // アセンブリが見つかったら読み込む
                        var asm = Assembly.LoadFrom(pluginFile);

                        // アセンブリから IOverlayAddon を実装した public クラスを列挙し...
                        var types = asm.GetExportedTypes().Where(t => 
                                t.GetInterface(typeof(IOverlayAddon).FullName) != null && t.IsClass);
                        foreach (var type in types)
                        {
                            try
                            {
                                // プラグインのインスタンスを生成し、アドオンリストに追加する
                                var addon = (IOverlayAddon)asm.CreateInstance(type.FullName);
                                this.Addons.Add(addon);

                                Logger.Log(LogLevel.Info, "LoadAddons: {0}: Initialized", type.FullName);
                            }
                            catch (Exception e)
                            {
                                Logger.Log(LogLevel.Error, "LoadAddons: {0}: {1}", type.FullName, e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log(LogLevel.Error, "LoadAddons: {0}: {1}", pluginFile, e);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "LoadAddons: {0}", e);
            }
        }

        /// <summary>
        /// 設定を読み込みます。
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                Config = PluginConfig.LoadXml(this.PluginDirectory, GetConfigPath());
                if(Config.ScreenShotSavePath == "" || Config.ScreenShotSavePath == null)
                {
                    Config.ScreenShotSavePath = DefaultScreenShotPath;
                }
            }
            catch (Exception e)
            {
                // 設定ファイルが存在しない、もしくは破損している場合は作り直す
                Logger.Log(LogLevel.Warning, "LoadConfig: {0}", e);
                Logger.Log(LogLevel.Info, "LoadConfig: Creating new configuration.");
                Config = new PluginConfig();
                Config.SetDefaultOverlayConfigs(this.PluginDirectory);
                if (Config.ScreenShotSavePath == "" || Config.ScreenShotSavePath == null)
                {
                    Config.ScreenShotSavePath = DefaultScreenShotPath;
                }
            }

            Directory.CreateDirectory(Config.ScreenShotSavePath);
        }

        /// <summary>
        /// 設定を保存します。
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                foreach (var overlay in this.Overlays)
                {
                    overlay.SavePositionAndSize();
                }

                Config.SaveXml(GetConfigPath());
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, "SaveConfig: {0}", e);
            }
        }

        /// <summary>
        /// 設定ファイルのパスを取得します。
        /// </summary>
        /// <returns></returns>
        private static string GetConfigPath()
        {
            var path = Path.Combine(
                ActGlobals.oFormActMain.AppDataFolder.FullName,
                "Config",
                "RainbowMage.OverlayPlugin.config.xml");

            return path;
        }
    }
}
