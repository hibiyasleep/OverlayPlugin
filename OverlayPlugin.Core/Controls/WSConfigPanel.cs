using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace RainbowMage.OverlayPlugin
{
    public partial class WSConfigPanel : UserControl
    {
        const string MKCERT_DOWNLOAD = "https://github.com/FiloSottile/mkcert/releases/download/v1.3.0/mkcert-v1.3.0-windows-amd64.exe";

        PluginConfig Config;

        public WSConfigPanel(PluginConfig cfg)
        {
            InitializeComponent();

            Config = cfg;
            ipTxt.Text = Config.WSServerIP;
            portTxt.Text = "" + Config.WSServerPort;
            sslBox.Checked = Config.WSServerSSL;
            sslBox.Enabled = WSServer.IsSSLPossible();

            UpdateStatus(null, new WSServer.StateChangedArgs(WSServer.IsRunning(), WSServer.IsFailed()));
            WSServer.OnStateChanged += UpdateStatus;

            lblUrlConfidentWarning.Visible = false;
            Registry.Resolve<PluginMain>().OverlaysChanged += (o, e) =>
            {
                RebuildOverlayOptions();
            };

            ctxMenuLinkCopy.Click += (o, e) =>
            {
                Clipboard.SetText(lnkOverlayUrl.Text);
            };
        }

        private void UpdateStatus(object sender, WSServer.StateChangedArgs e)
        {
            startBtn.Enabled = true;
            stopBtn.Enabled = false;

            if (e.Running)
            {
                statusLabel.Text = "Running";
                statusLabel.ForeColor = Color.ForestGreen;

                startBtn.Enabled = false;
                stopBtn.Enabled = true;
            }
            else if (e.Failed)
            {
                statusLabel.Text = "Failed";
                statusLabel.ForeColor = Color.DarkRed;
            }
            else
            {
                statusLabel.Text = "Stopped";
                statusLabel.ForeColor = Color.Gray;
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            Config.WSServerRunning = true;
            WSServer.Initialize();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            Config.WSServerRunning = false;
            WSServer.Stop();
        }

        private void genSslBtn_Click(object sender, EventArgs e)
        {
            genSslBtn.Enabled = false;
            logDisplay.Text = "Generating SSL Certificate. Please wait...\r\n";
            
            Task.Run(GenSsl);
        }

        private void GenSsl()
        {
            try
            {
                var mkcertPath = Path.Combine(PluginMain.PluginDirectory, "mkcert.exe");

                if (!File.Exists(mkcertPath))
                {
                    logDisplay.AppendText("Downloading mkcert...\r\n");

                    if ((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls12) != SecurityProtocolType.Tls12)
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    }
                    var client = new WebClient();

                    try
                    {
                        client.DownloadFile(MKCERT_DOWNLOAD, mkcertPath);
                    }
                    catch (Exception e)
                    {
                        logDisplay.AppendText(string.Format("\nFailed: {0}", e));
                        genSslBtn.Enabled = true;
                        return;
                    }
                }

                logDisplay.AppendText("Installing CA...\r\n");
                if (!RunLogCmd(mkcertPath, "-install"))
                {
                    logDisplay.AppendText("\r\nFailed!\r\n");
                    genSslBtn.Enabled = true;
                    return;
                }

                logDisplay.AppendText("Generating certificate...\r\n");
                if (!RunLogCmd(mkcertPath, string.Format("-pkcs12 -p12-file \"{0}\" localhost 127.0.0.1 ::1", WSServer.GetCertPath())))
                {
                    logDisplay.AppendText("\r\nFailed!\r\n");
                    genSslBtn.Enabled = true;
                    return;
                }

                logDisplay.AppendText("\r\nDone.\r\n");

                sslBox.Enabled = WSServer.IsSSLPossible();
                sslBox.Checked = sslBox.Enabled;
                Config.WSServerSSL = sslBox.Enabled;
                genSslBtn.Enabled = true;
            }
            catch (Exception e)
            {
                logDisplay.AppendText(string.Format("\r\nException: {0}", e));
                genSslBtn.Enabled = true;
            }
        }

        private bool RunLogCmd(string file, string args)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = file;
                p.StartInfo.Arguments = args;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;

                DataReceivedEventHandler showLine = (sender, e) =>
                {
                    if (e.Data != null) logDisplay.AppendText(e.Data.Replace("\n", "\r\n") + "\r\n");
                };

                p.OutputDataReceived += showLine;
                p.ErrorDataReceived += showLine;

                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                p.WaitForExit(10000);

                return p.ExitCode == 0;
            }
        }

        private void sslBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.WSServerSSL = sslBox.Checked;
        }

        private void portTxt_Leave(object sender, EventArgs e)
        {
            var valid = true;
            int port = 0;
            try
            {
                port = int.Parse(portTxt.Text);
            }
            catch
            {
                valid = false;
            }

            if (valid && (port < 1 || port > 65535))
            {
                valid = false;
            }

            if (valid)
            {
                Config.WSServerPort = port;
            }
            else
            {
                MessageBox.Show(
                    string.Format("{0} is not a valid port. Should be a number between 1 and 65535.", portTxt.Text),
                    "Invalid Port",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        private void ipTxt_Leave(object sender, EventArgs e)
        {
            IPAddress addr = null;
            if (IPAddress.TryParse(ipTxt.Text, out addr))
            {
                Config.WSServerIP = ipTxt.Text;
            }
            else
            {
                MessageBox.Show(
                    string.Format("{0} is not a IP address.", ipTxt.Text),
                    "Invalid Address",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        private void RebuildOverlayOptions()
        {
            cbOverlay.Items.Clear();

            foreach (var overlay in Registry.Resolve<PluginMain>().Overlays)
            {
                if (overlay.GetType() == typeof(Overlays.MiniParseOverlay))
                {
                    cbOverlay.Items.Add(new
                    {
                        label = overlay.Config.Name,
                        overlay
                    });
                }
            }
        }

        private void cbOverlay_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = cbOverlay.Items[cbOverlay.SelectedIndex];
            var overlay = (Overlays.MiniParseOverlay) item.GetType().GetProperty("overlay").GetValue(item);
            if (overlay == null) return;

            var (confident, url) = WSServer.GetUrl(overlay);

            lblUrlConfidentWarning.Visible = !confident;
            lnkOverlayUrl.Text = url;
        }

        private void lnkOverlayUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lnkOverlayUrl.Text);
        }

        private class ShowLineArgs : EventArgs
        {
            public string Data { get; private set; }
            public ShowLineArgs(string d)
            {
                Data = d;
            }
        }
    }
}
