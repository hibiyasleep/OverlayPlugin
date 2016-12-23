using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public partial class MiniParseExtConfigPanel : UserControl
    {
        private MiniParseExtOverlayConfig config;
        private MiniParseExtOverlay overlay;

        static readonly List<KeyValuePair<string, MiniParseSortType>> sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
        {
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.DoNotSort), MiniParseSortType.None),
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.SortStringAscending), MiniParseSortType.StringAscending),
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.SortStringDescending), MiniParseSortType.StringDescending),
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.SortNumberAscending), MiniParseSortType.NumericAscending),
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.SortNumberDescending), MiniParseSortType.NumericDescending)
        };

        public MiniParseExtConfigPanel(MiniParseExtOverlay overlay)
        {
            InitializeComponent();

            this.overlay = overlay;
            config = overlay.Config;

            SetupControlProperties();
            SetupConfigEventHandlers();
        }

        private void SetupControlProperties()
        {
            checkMiniParseExtVisible.Checked = config.IsVisible;
            checkMiniParseExtClickthru.Checked = config.IsClickThru;
            checkLock.Checked = config.IsLocked;
            textMiniParseExtUrl.Text = config.Url;
            textMiniParseExtSortKey.Text = config.SortKey;
            comboMiniParseExtSortType.DisplayMember = "Key";
            comboMiniParseExtSortType.ValueMember = "Value";
            comboMiniParseExtSortType.DataSource = sortTypeDict;
            comboMiniParseExtSortType.SelectedValue = config.SortType;
            comboMiniParseExtSortType.SelectedIndexChanged += comboSortType_SelectedIndexChanged;
            nudMaxFrameRate.Value = config.MaxFrameRate;
            checkEnableGlobalHotkey.Checked = config.GlobalHotkeyEnabled;
            textGlobalHotkey.Enabled = checkEnableGlobalHotkey.Checked;
            textGlobalHotkey.Text = Utility.GetHotkeyString(config.GlobalHotkeyModifiers, config.GlobalHotkey);
        }

        private void SetupConfigEventHandlers()
        {
            config.ScrPathChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {

                });
            };
            config.VisibleChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    checkMiniParseExtVisible.Checked = e.IsVisible;
                });
            };
            config.ClickThruChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    checkMiniParseExtClickthru.Checked = e.IsClickThru;
                });
            };
            config.UrlChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    textMiniParseExtUrl.Text = e.NewUrl;
                });
            };
            config.SortKeyChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    textMiniParseExtSortKey.Text = e.NewSortKey;
                });
            };
            config.SortTypeChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    comboMiniParseExtSortType.SelectedValue = e.NewSortType;
                });
            };
            config.MaxFrameRateChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    nudMaxFrameRate.Value = e.NewFrameRate;
                });
            };
            config.GlobalHotkeyEnabledChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    checkEnableGlobalHotkey.Checked = e.NewGlobalHotkeyEnabled;
                    textGlobalHotkey.Enabled = checkEnableGlobalHotkey.Checked;
                });
            };
            config.GlobalHotkeyChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    textGlobalHotkey.Text = Utility.GetHotkeyString(config.GlobalHotkeyModifiers, e.NewHotkey);
                });
            };
            config.GlobalHotkeyModifiersChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    textGlobalHotkey.Text = Utility.GetHotkeyString(e.NewHotkey, config.GlobalHotkey);
                });
            };
            config.LockChanged += (o, e) =>
            {
                InvokeIfRequired(() =>
                {
                    checkLock.Checked = e.IsLocked;
                });
            };
        }

        public void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void checkWindowVisible_CheckedChanged(object sender, EventArgs e)
        {
            config.IsVisible = checkMiniParseExtVisible.Checked;
        }

        private void checkMouseClickthru_CheckedChanged(object sender, EventArgs e)
        {
            config.IsClickThru = checkMiniParseExtClickthru.Checked;
        }

        private void textUrl_TextChanged(object sender, EventArgs e)
        {
            //config.Url = textMiniParseExtUrl.Text;
        }

        private void textMiniParseExtUrl_Leave(object sender, EventArgs e)
        {
            config.Url = textMiniParseExtUrl.Text;
        }

        private void textSortKey_TextChanged(object sender, EventArgs e)
        {
            config.SortKey = textMiniParseExtSortKey.Text;
        }

        private void comboSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = (MiniParseSortType)comboMiniParseExtSortType.SelectedValue;
            config.SortType = value;
        }

        private void nudMaxFrameRate_ValueChanged(object sender, EventArgs e)
        {
            config.MaxFrameRate = (int)nudMaxFrameRate.Value;
        }

        private void buttonReloadBrowser_Click(object sender, EventArgs e)
        {
            overlay.Navigate(config.Url);
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {

        }

        private void buttonMiniParseExtOpenDevTools_Click(object sender, EventArgs e)
        {
            overlay.Overlay.Renderer.showDevTools();
        }

        private void buttonMiniParseExtOpenDevTools_RClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                overlay.Overlay.Renderer.showDevTools(false);
        }

        private void buttonCopyActXiv_Click(object sender, EventArgs e)
        {
            var json = overlay.CreateJsonData();
            if (!string.IsNullOrWhiteSpace(json))
            {
                Clipboard.SetText(json);
            }
        }

        private void checkBoxEnableGlobalHotkey_CheckedChanged(object sender, EventArgs e)
        {
            config.GlobalHotkeyEnabled = checkEnableGlobalHotkey.Checked;
            textGlobalHotkey.Enabled = config.GlobalHotkeyEnabled;
        }

        private void textBoxGlobalHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            var key = Utility.RemoveModifiers(e.KeyCode, e.Modifiers);
            config.GlobalHotkey = key;
            config.GlobalHotkeyModifiers = e.Modifiers;
        }

        private void checkLock_CheckedChanged(object sender, EventArgs e)
        {
            config.IsLocked = checkLock.Checked;
        }

        private void buttonMiniParseSelectFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                config.Url = new Uri(ofd.FileName).ToString();
            }
        }

        private void buttonMiniparseScreenshotFolderSelect_Click(object sender, EventArgs e)
        {
            var ofd = new FolderBrowserDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                config.ScrPath = ofd.SelectedPath;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MiniParseExtConfigPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
