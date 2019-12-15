using Advanced_Combat_Tracker;
using RainbowMage.OverlayPlugin;
using System.Windows.Forms;

namespace AddonExample
{
    public class AddonExample : IActPluginV1, IOverlayAddonV2
    {
        public static string pluginPath = "";

        public void DeInitPlugin()
        {

        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginStatusText.Text = "Ready.";

            // We don't need a tab here.
            ((TabControl)pluginScreenSpace.Parent).TabPages.Remove(pluginScreenSpace);

            foreach (var plugin in ActGlobals.oFormActMain.ActPlugins)
            {
                if (plugin.pluginObj == this)
                {
                    pluginPath = plugin.pluginFile.FullName;
                    break;
                }
            }
        }

        public void Init()
        {
            // Register EventSource
            Registry.RegisterEventSource<AddonExampleEventSource>();

            // Register Overlay
            // Important Tip:
            //   ngld/OverlayPlugin can communicate between Javascript and EventSources.
            //   In many cases, it is sufficient to use MiniParse, and it is rarely necessary to create original Overlay.
            Registry.RegisterOverlay<AddonExampleOverlay>();
        }
    }
}
