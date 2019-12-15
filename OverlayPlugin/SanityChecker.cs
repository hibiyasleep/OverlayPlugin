using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    public static class SanityChecker
    {
        /**
         * The assembly load order is as follows:
         * 
         *   * OverlayPlugin
         *   * OverlayPlugin.Common (PluginLoader.InitPlugin: version check, Logger & Registry)
         *   * OverlayPlugin.Updater (PluginLoader.InitPluginCore: CEF installer / updater)
         *   * OverlayPlugin.Core (PluginLoader.InitPluginCore)
         *   * OverlayPlugin.Core (PluginMain.InitPlugin)
         */

        public static bool LoadSaneAssembly(string name)
        {
            var loaderVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var asm = Assembly.Load(name);
            var asmVersion = asm.GetName().Version;

            if (loaderVersion != asmVersion)
            {
                MessageBox.Show(
                    string.Format(Resources.AssemblyMismatch, asm.Location, asmVersion, loaderVersion),
                    "OverlayPlugin Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }

            return true;
        }
    }
}
