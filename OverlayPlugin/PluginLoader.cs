using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using RainbowMage.HtmlRenderer;
using RainbowMage.OverlayPlugin.Updater;

namespace RainbowMage.OverlayPlugin
{
    public class PluginLoader : IActPluginV1
    {
        PluginMain pluginMain;
        Logger logger;
        static AssemblyResolver asmResolver;
        string pluginDirectory;
        TabPage pluginScreenSpace;
        Label pluginStatusText;

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginDirectory = GetPluginDirectory();

            if (asmResolver == null)
            {
                asmResolver = new AssemblyResolver(new List<string>
                {
                    Path.Combine(pluginDirectory, "libs"),
                    Path.Combine(pluginDirectory, "addons"),
                    GetCefPath()
                });
            }

            this.pluginScreenSpace = pluginScreenSpace;
            this.pluginStatusText = pluginStatusText;

            /*
             * We explicitly load OverlayPlugin.Common here for two reasons:
             *  * To prevent a stack overflow in the assembly loaded handler when we use the logger interface.
             *  * To check that the loaded version matches.
             * OverlayPlugin.Core is needed due to the Logger class used in Initialize().
             * OverlayPlugin.Updater is necessary for the CefInstaller class.
             */
            if (!SanityChecker.LoadSaneAssembly("OverlayPlugin.Common") || !SanityChecker.LoadSaneAssembly("OverlayPlugin.Core") ||
                !SanityChecker.LoadSaneAssembly("OverlayPlugin.Updater"))
            {
                pluginStatusText.Text = Resources.FailedToLoadCommon;
                return;
            }

            Initialize();
        }

        // AssemblyResolver でカスタムリゾルバを追加する前に PluginMain が解決されることを防ぐために、
        // インライン展開を禁止したメソッドに処理を分離
        [MethodImpl(MethodImplOptions.NoInlining)]
        private async void Initialize()
        {
            pluginStatusText.Text = Resources.InitRuntime;

            Registry.Init();
            logger = new Logger();
            asmResolver.ExceptionOccured += (o, e) => logger.Log(LogLevel.Error, Resources.AssemblyResolverError, e.Exception);
            asmResolver.AssemblyLoaded += (o, e) => logger.Log(LogLevel.Debug, Resources.AssemblyResolverLoaded, e.LoadedAssembly.FullName);
            pluginMain = new PluginMain(pluginDirectory, logger);

            pluginStatusText.Text = Resources.InitCef;

            try
            {
                CurlWrapper.Init(pluginDirectory);
            } catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
            }

            await FinishInit();
        }

        public async Task FinishInit()
        {
            if (await CefInstaller.EnsureCef(GetCefPath()))
            {
                // Finally, load the html renderer. We load it here since HtmlRenderer depends on CEF which we can't load these before
                // the CefInstaller is done.
                if (SanityChecker.LoadSaneAssembly("HtmlRenderer"))
                {
                    // Since this is an async method, we could have switched threds. Make sure InitPlugin() runs on the ACT main thread.
                    ActGlobals.oFormActMain.Invoke((Action)(() =>
                    {
                        pluginMain.InitPlugin(pluginScreenSpace, pluginStatusText);
                    }));
                } else
                {
                    pluginStatusText.Text = Resources.CoreOrHtmlRendererInsane;
                }
            } else
            {
                pluginScreenSpace.Controls.Add(new CefMissingTab(GetCefPath(), this));
            }
        }

        public void DeInitPlugin()
        {
            if (pluginMain != null)
            {
                pluginMain.DeInitPlugin();
                Registry.Clear();
            }

            // We can't re-init CEF after shutting it down. So let's only do that when ACT closes to avoid unexpected behaviour (crash when re-enabling the plugin).
            // TODO: Figure out how to detect disabling plugin vs ACT shutting down.
            // ShutdownRenderer(null, null);
        }

        /*
        public void ShutdownRenderer(object sender, EventArgs e)
        {
            Renderer.Shutdown();

            // We can only dispose the resolver once the HtmlRenderer is shut down.
            asmResolver.Dispose();
        }
        */

        private string GetPluginDirectory()
        {
            // ACT のプラグインリストからパスを取得する
            // Assembly.CodeBase からはパスを取得できない
            var plugin = ActGlobals.oFormActMain.ActPlugins.Where(x => x.pluginObj == this).FirstOrDefault();
            if (plugin != null)
            {
                return Path.GetDirectoryName(plugin.pluginFile.FullName);
            }
            else
            {
                throw new Exception("Could not find ourselves in the plugin list!");
            }
        }

        private string GetCefPath()
        {
            return Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "OverlayPluginCef", Environment.Is64BitProcess ? "x64" : "x86");
        }
    }
}
