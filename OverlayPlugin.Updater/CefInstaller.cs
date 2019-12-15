using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin.Updater
{
    public class CefInstaller
    {
        const string CEF_DL = "https://github.com/ngld/OverlayPlugin/releases/download/v0.7.0/CefSharp-{CEF_VERSION}-{ARCH}.DO_NOT_DOWNLOAD";
        const string CEF_VERSION = "75.1.14";

        public static string GetUrl()
        {
            return CEF_DL.Replace("{CEF_VERSION}", CEF_VERSION).Replace("{ARCH}", Environment.Is64BitProcess ? "x64" : "x86");
        }

        public static async Task<bool> EnsureCef(string cefPath)
        {
            var manifest = Path.Combine(cefPath, "version.txt");

            if (File.Exists(manifest))
            {
                var installed = File.ReadAllText(manifest).Trim();
                if (installed == CEF_VERSION)
                {
                    return true;
                }
            }

            return await InstallCef(cefPath);
        }

        public static async Task<bool> InstallCef(string cefPath, string archivePath = null)
        {
            var lib = IntPtr.Zero;
            while (true)
            {
                lib = NativeMethods.LoadLibrary("msvcp140.dll");
                if (lib != IntPtr.Zero)
                {
                    NativeMethods.FreeLibrary(lib);
                    break;
                }

                var response = MessageBox.Show(
                    Resources.MsvcrtMissing,
                    Resources.OverlayPluginTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (response == DialogResult.Yes)
                {
                    var installed = await Installer.InstallMsvcrt();

                    if (!installed)
                    {
                        MessageBox.Show(
                            Resources.MsvcrtFailed,
                            Resources.OverlayPluginTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                } else
                {
                    return false;
                }
            }

            var url = GetUrl();

            var result = await Installer.Run(archivePath == null ? url : archivePath, cefPath);
            if (!result || !Directory.Exists(cefPath))
            {
                MessageBox.Show(
                    Resources.UpdateCefDlFailed,
                    Resources.ErrorTitle,
                    MessageBoxButtons.OK
                );
                
                return false;
            } else
            {
                File.WriteAllText(Path.Combine(cefPath, "version.txt"), CEF_VERSION);
                return true;
            }
        }
    }
}
