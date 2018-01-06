using System;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public partial class LabelOverlayConfigPanel : UserControl
    {
        private LabelOverlayConfig config;
        private LabelOverlay overlay;

        public LabelOverlayConfigPanel(LabelOverlay overlay)
        {
            InitializeComponent();

            this.overlay = overlay;
            this.config = overlay.Config;

            SetupControlProperties();
            SetupConfigEventHandlers();
        }

        private void SetupControlProperties()
        {
            this.checkMiniParseVisible.Checked = config.IsVisible;
            this.checkMiniParseClickthru.Checked = config.IsClickThru;
            this.checkLock.Checked = config.IsLocked;
            this.textUrl.Text = config.Url;
            this.checkEnableGlobalHotkey.Checked = config.GlobalHotkeyEnabled;
            this.textGlobalHotkey.Enabled = this.checkEnableGlobalHotkey.Checked;
            this.textGlobalHotkey.Text = Util.GetHotkeyString(config.GlobalHotkeyModifiers, config.GlobalHotkey);
            this.textBox.Text = config.Text;
            this.checkHTML.Checked = config.HtmlModeEnabled;
        }

        private void SetupConfigEventHandlers()
        {
            this.config.VisibleChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.checkMiniParseVisible.Checked = e.IsVisible;
                });
            };
            this.config.ClickThruChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.checkMiniParseClickthru.Checked = e.IsClickThru;
                });
            };
            this.config.UrlChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.textUrl.Text = e.NewUrl;
                });
            };
            this.config.GlobalHotkeyEnabledChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.checkEnableGlobalHotkey.Checked = e.NewGlobalHotkeyEnabled;
                    this.textGlobalHotkey.Enabled = this.checkEnableGlobalHotkey.Checked;
                });
            };
            this.config.GlobalHotkeyChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.textGlobalHotkey.Text = Util.GetHotkeyString(this.config.GlobalHotkeyModifiers, e.NewHotkey);
                });
            };
            this.config.GlobalHotkeyModifiersChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.textGlobalHotkey.Text = Util.GetHotkeyString(e.NewHotkey, this.config.GlobalHotkey);
                });
            };
            this.config.LockChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.checkLock.Checked = e.IsLocked;
                });
            };
            this.config.TextChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.textBox.Text = e.Text;
                });
            };
            this.config.HTMLModeChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.checkHTML.Checked = e.NewState;
                });
            };
        }

        private void checkWindowVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (this.config.Url == "" && this.config.Text == "") return;
            this.config.IsVisible = checkMiniParseVisible.Checked;
        }

        private void checkMouseClickthru_CheckedChanged(object sender, EventArgs e)
        {
            if (this.config.Url == "" && this.config.Text == "") return;
            this.config.IsClickThru = checkMiniParseClickthru.Checked;
        }

        private void textUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonReloadBrowser_Click(object sender, EventArgs e)
        {
            this.overlay.Navigate(this.config.Url);
        }

        private void buttonOpenDevTools_Click(object sender, EventArgs e)
        {
            this.overlay.Overlay.Renderer.showDevTools();
        }

        private void buttonOpenDevTools_RClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                this.overlay.Overlay.Renderer.showDevTools(false);
        }

        private void buttonCopyActXiv_Click(object sender, EventArgs e)
        {
            var json = overlay.CreateJson();
            if (!string.IsNullOrWhiteSpace(json))
            {
                Clipboard.SetText(json);
            }
        }

        private void checkBoxEnableGlobalHotkey_CheckedChanged(object sender, EventArgs e)
        {
            this.config.GlobalHotkeyEnabled = this.checkEnableGlobalHotkey.Checked;
            this.textGlobalHotkey.Enabled = this.config.GlobalHotkeyEnabled;
        }

        private void textBoxGlobalHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            var key = Util.RemoveModifiers(e.KeyCode, e.Modifiers);
            this.config.GlobalHotkey = key;
            this.config.GlobalHotkeyModifiers = e.Modifiers;
        }

        private void checkLock_CheckedChanged(object sender, EventArgs e)
        {
            if (this.config.Url == "" || this.config.Text == "") return;
            this.config.IsLocked = this.checkLock.Checked;
        }

        private void checkHTML_CheckedChanged(object sender, EventArgs e)
        {
            this.config.HtmlModeEnabled = checkHTML.Checked;
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.config.Url = new Uri(ofd.FileName).ToString();
            }
        }

        private void textUrl_Leave(object sender, EventArgs e)
        {
            this.config.Url = textUrl.Text;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            this.config.Text = textBox.Text;
        }
    }
}
