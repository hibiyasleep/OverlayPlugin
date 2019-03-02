using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    public class PluginLoader : IActPluginV1
    {
        public static string primaryUser = "YOU";
        PluginMain pluginMain;
        Logger logger;
        AssemblyResolver asmResolver;
        string pluginDirectory;

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginDirectory = GetPluginDirectory();

            var directories = new List<string>();
            directories.Add(pluginDirectory);
            directories.Add(Path.Combine(pluginDirectory, "addon"));
            asmResolver = new AssemblyResolver(directories);

            ActGlobals.oFormActMain.BeforeLogLineRead += BeforeLogLineRead;

            Initialize(pluginScreenSpace, pluginStatusText);
            AddExportVariable();
        }

        // AssemblyResolver でカスタムリゾルバを追加する前に PluginMain が解決されることを防ぐために、
        // インライン展開を禁止したメソッドに処理を分離
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Initialize(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            logger = new Logger();
            asmResolver.ExceptionOccured += (o, e) => logger.Log(LogLevel.Error, "AssemblyResolver: Error: {0}", e.Exception);
            asmResolver.AssemblyLoaded += (o, e) => logger.Log(LogLevel.Debug, "AssemblyResolver: Loaded: {0}", e.LoadedAssembly.FullName);
            pluginMain = new PluginMain(pluginDirectory, logger);
            pluginMain.InitPlugin(pluginScreenSpace, pluginStatusText);
            PluginMain.PrimaryUser = "YOU";
        }

        public void DeInitPlugin()
        {
            ActGlobals.oFormActMain.BeforeLogLineRead -= BeforeLogLineRead;
            pluginMain.DeInitPlugin();
            asmResolver.Dispose();
        }

        public string GetPluginDirectory()
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
                throw new Exception();
            }
        }

        /// <summary>
        /// ACT BeforeLogLineRead Event getting Change Primary Player Name
        /// </summary>
        /// <param name="isImport"></param>
        /// <param name="logInfo"></param>
        private void BeforeLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            if (logInfo.logLine.IndexOf("02:Changed") > -1)
            {
                primaryUser = logInfo.logLine;
                primaryUser = primaryUser.Replace("02:Changed primary player to ", "").Replace(".", "");
                PluginMain.PrimaryUser = primaryUser = primaryUser.Substring(primaryUser.IndexOf("]") + 2);
            }
        }
        
        public void AddExportVariable()
        {
            if (!EncounterData.ExportVariables.ContainsKey("PrimaryUser"))
            {
                EncounterData.ExportVariables.Add("PrimaryUser",
                new EncounterData.TextExportFormatter("PrimaryUser", "Primary Current Username", "Using ACT Current Charname 'YOU' almost get Current Username from User Input, but this Force Attach Current Username.", (Data, Extra, Format) => { return getPrimaryUserName(); }));
            }
        }

        public string getPrimaryUserName()
        {
            return primaryUser;
        }
    }
}
