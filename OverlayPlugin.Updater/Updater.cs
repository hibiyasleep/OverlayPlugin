using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace RainbowMage.OverlayPlugin.Updater
{
    public class Updater
    {
        const string REL_URL = "https://api.github.com/repos/ngld/OverlayPlugin/releases";
        const string DL = "https://github.com/ngld/OverlayPlugin/releases/download/v{VERSION}/OverlayPlugin-{VERSION}.7z";

        public static Task<(bool, Version, string)> CheckForUpdate(Control parent)
        {
            return Task.Run(() =>
            {
                var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                Version remoteVersion;
                string response;
                try
                {
                    response = CurlWrapper.Get(REL_URL);
                }
                catch (CurlException ex)
                {
                    MessageBox.Show(string.Format(Resources.UpdateCheckException, ex.ToString()), Resources.UpdateCheckTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (false, null, "");
                }

                var releaseNotes = "";
                try
                {
                    // JObject doesn't accept arrays so we have to package the response in a JSON object.
                    var tmp = JObject.Parse("{\"content\":" + response + "}");
                    remoteVersion = Version.Parse(tmp["content"][0]["tag_name"].ToString().Substring(1));

                    foreach (var rel in tmp["content"])
                    {
                        var version = Version.Parse(rel["tag_name"].ToString().Substring(1));
                        if (version.CompareTo(currentVersion) < 1) break;

                        releaseNotes += "---\n\n# " + rel["name"].ToString() + "\n\n" + rel["body"].ToString() + "\n\n";
                    }

                    if (releaseNotes.Length > 5)
                    {
                        releaseNotes = releaseNotes.Substring(5);
                    }
                }
                catch (Exception ex)
                {
                    parent.Invoke((Action) (() =>
                    {
                        MessageBox.Show(string.Format(Resources.UpdateParseVersionError, ex.ToString()), Resources.UpdateTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                    return (false, null, null);
                }

                try
                {
                    releaseNotes = Regex.Replace(releaseNotes, @"<!-- TRAILER BEGIN -->(?:[^<]|<(?!!-- TRAILER END -->))+<!-- TRAILER END -->", "");
                }
                catch (Exception ex)
                {
                    Registry.Resolve<ILogger>().Log(LogLevel.Error, $"Failed to remove trailers from release notes: {ex}");
                }

                return (remoteVersion.CompareTo(currentVersion) > 0, remoteVersion, releaseNotes);
            });
        }

        public static async Task<bool> InstallUpdate(Version version, string pluginDirectory)
        {
            var url = DL.Replace("{VERSION}", version.ToString());

            var result = await Installer.Run(url, pluginDirectory, true);
            if (!result)
            {
                var response = MessageBox.Show(
                    Resources.UpdateFailedError,
                    Resources.ErrorTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (response == DialogResult.Yes)
                {
                    return await InstallUpdate(version, pluginDirectory);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show(
                    Resources.UpdateSuccess,
                    Resources.UpdateTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return true;
            }
        }

        public static async void PerformUpdateIfNecessary(Control parent, string pluginDirectory, bool manualCheck = false)
        {
            var config = Registry.Resolve<IPluginConfig>();

            // Only check once per day.
            if (!manualCheck && config.LastUpdateCheck != null && (DateTime.Now - config.LastUpdateCheck).TotalDays < 1)
            {
                return;
            }

            var (newVersion, remoteVersion, releaseNotes) = await CheckForUpdate(parent);

            if (remoteVersion != null)
            {
                config.LastUpdateCheck = DateTime.Now;
            }

            if (newVersion)
            {
                // Make sure we open the UpdateQuestionForm on a UI thread.
                parent.Invoke((Action)(async () =>
                 {
                     var dialog = new UpdateQuestionForm(releaseNotes);
                     var result = dialog.ShowDialog();
                     dialog.Dispose();

                     if (result == DialogResult.Yes)
                     {
                         await InstallUpdate(remoteVersion, pluginDirectory);
                     }
                 }));
            } else if (manualCheck && remoteVersion != null)
            {
                parent.Invoke((Action)(() =>
                {
                    MessageBox.Show(Resources.UpdateAlreadyLatest, Resources.UpdateTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
        }
    }
}
