﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public partial class MiniParseConfigPanel : UserControl
    {
        private MiniParseOverlayConfig config;
        private MiniParseOverlay overlay;

        static readonly List<KeyValuePair<string, MiniParseSortType>> sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
        {
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.DoNotSort), MiniParseSortType.None),
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.SortStringAscending), MiniParseSortType.StringAscending),
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.SortStringDescending), MiniParseSortType.StringDescending),
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.SortNumberAscending), MiniParseSortType.NumericAscending),
            new KeyValuePair<string, MiniParseSortType>(Localization.GetText(TextItem.SortNumberDescending), MiniParseSortType.NumericDescending)
        };

        static readonly List<KeyValuePair<string, GlobalHotkeyType>> hotkeyTypeDict = new List<KeyValuePair<string, GlobalHotkeyType>>()
        {
            new KeyValuePair<string, GlobalHotkeyType>(Localization.GetText(TextItem.ToggleVisible), GlobalHotkeyType.ToggleVisible),
            new KeyValuePair<string, GlobalHotkeyType>(Localization.GetText(TextItem.ToggleClickthru), GlobalHotkeyType.ToggleClickthru),
            new KeyValuePair<string, GlobalHotkeyType>(Localization.GetText(TextItem.ToggleLock), GlobalHotkeyType.ToggleLock),
            new KeyValuePair<string, GlobalHotkeyType>(Localization.GetText(TextItem.Screenshot), GlobalHotkeyType.Screenshot)
        };

        public MiniParseConfigPanel(MiniParseOverlay overlay)
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
            this.textMiniParseUrl.Text = config.Url;
            this.textMiniParseSortKey.Text = config.SortKey;
            this.comboMiniParseSortType.DisplayMember = "Key";
            this.comboMiniParseSortType.ValueMember = "Value";
            this.comboMiniParseSortType.DataSource = sortTypeDict;
            this.comboMiniParseSortType.SelectedValue = config.SortType;
            this.comboMiniParseSortType.SelectedIndexChanged += comboSortType_SelectedIndexChanged;
            this.nudMaxFrameRate.Value = config.MaxFrameRate;
            this.checkEnableGlobalHotkey.Checked = config.GlobalHotkeyEnabled;
            this.textGlobalHotkey.Enabled = this.checkEnableGlobalHotkey.Checked;
            this.textGlobalHotkey.Text = Util.GetHotkeyString(config.GlobalHotkeyModifiers, config.GlobalHotkey);
            this.comboHotkeyType.DisplayMember = "Key";
            this.comboHotkeyType.ValueMember = "Value";
            this.comboHotkeyType.DataSource = hotkeyTypeDict;
            this.comboHotkeyType.SelectedValue = config.GlobalHotkeyType;
            this.comboHotkeyType.SelectedIndexChanged += ComboHotkeyMode_SelectedIndexChanged;
            this.nudUpdateInterval.Value = config.OverlayUpdateInterval;
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
                    this.textMiniParseUrl.Text = e.NewUrl;
                });
            };

            this.config.SortKeyChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.textMiniParseSortKey.Text = e.NewSortKey;
                });
            };

            this.config.SortTypeChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.comboMiniParseSortType.SelectedValue = e.NewSortType;
                });
            };

            this.config.MaxFrameRateChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.nudMaxFrameRate.Value = e.NewFrameRate;
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

            this.config.GlobalHotkeyTypeChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.comboHotkeyType.SelectedValue = e.NewHotkeyType;
                });
            };

            this.config.OverlayUpdateIntervalChanged += (o, e) =>
            {
                Invoke((MethodInvoker)delegate
                {
                    this.nudUpdateInterval.Value = (int)e.Interval;
                    this.overlay.UpdateTimer.Interval = e.Interval;
                    this.overlay.UpdateStringCacheExpireInterval = TimeSpan.FromMilliseconds(e.Interval / 2);
                });
            };
        }

        private void checkWindowVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (this.config.Url == "") return;
            this.config.IsVisible = checkMiniParseVisible.Checked;
        }

        private void checkMouseClickthru_CheckedChanged(object sender, EventArgs e)
        {
            if (this.config.Url == "") return;
            this.config.IsClickThru = checkMiniParseClickthru.Checked;
        }

        private void textUrl_TextChanged(object sender, EventArgs e)
        {
            //this.config.Url = textMiniParseUrl.Text;
        }

        private void textMiniParseUrl_Leave(object sender, EventArgs e)
        {
            this.config.Url = textMiniParseUrl.Text;
        }

        private void textSortKey_TextChanged(object sender, EventArgs e)
        {
            this.config.SortKey = this.textMiniParseSortKey.Text;
        }

        private void comboSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = (MiniParseSortType)this.comboMiniParseSortType.SelectedValue;
            this.config.SortType = value;
        }

        private void ComboHotkeyMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = (GlobalHotkeyType)this.comboHotkeyType.SelectedValue;
            this.config.GlobalHotkeyType = value;
        }

        private void nudMaxFrameRate_ValueChanged(object sender, EventArgs e)
        {
            this.config.MaxFrameRate = (int)nudMaxFrameRate.Value;
        }

        private void buttonReloadBrowser_Click(object sender, EventArgs e)
        {
            this.overlay.Navigate(this.config.Url);
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.config.Url = new Uri(ofd.FileName).ToString();
            }
        }

        private void buttonMiniParseOpenDevTools_Click(object sender, EventArgs e)
        {
            this.overlay.Overlay.Renderer.ShowDevTools();
        }

        private void buttonMiniParseOpenDevTools_RClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                this.overlay.Overlay.Renderer.ShowDevTools(false);
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
            if (this.config.Url == "") return;
            this.config.IsLocked = this.checkLock.Checked;
        }

        private void nudUpdateInterval_ValueChanged(object sender, EventArgs e)
        {
            this.config.OverlayUpdateInterval = (int)this.nudUpdateInterval.Value;
        }
    }
}
