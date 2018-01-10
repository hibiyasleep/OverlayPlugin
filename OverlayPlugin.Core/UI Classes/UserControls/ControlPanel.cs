using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    public partial class ControlPanel : UserControl
    {
        PluginMain pluginMain;
        PluginConfig config;

        public ControlPanel(PluginMain pluginMain, PluginConfig config)
        {
            InitializeComponent();

            this.pluginMain = pluginMain;
            this.config = config;

            this.checkBoxAutoHide.Checked = this.config.HideOverlaysWhenNotActive;

            this.menuFollowLatestLog.Checked = this.config.FollowLatestLog;
            this.listViewLog.VirtualListSize = PluginMain.Logger.Logs.Count;
            PluginMain.Logger.Logs.ListChanged += (o, e) =>
            {
                this.listViewLog.BeginUpdate();
                this.listViewLog.VirtualListSize = PluginMain.Logger.Logs.Count;
                if (this.config.FollowLatestLog && this.listViewLog.VirtualListSize > 0)
                {
                    this.listViewLog.EnsureVisible(this.listViewLog.VirtualListSize - 1);
                }
                this.listViewLog.EndUpdate();
            };

            InitializeOverlayConfigTabs();

            screenShotPath.Text                    = config.ScreenshotSavePath;
            screenShotBackgroundPath.Text          = config.ScreenshotBackgroundPath;
            screenShotBackgroundMode.SelectedIndex = config.ScreenshotBackgroundMode;
            screenShotAutoClipping.Checked         = config.ScreenshotAutoClipping;
            screenShotMargin.Value                 = config.ScreenshotMargin;

            if (string.IsNullOrWhiteSpace(screenShotPath.Text))
            {
                screenShotPath.Text = PluginMain.DefaultScreenshotPath;
            }
        }

        private void InitializeOverlayConfigTabs()
        {
            foreach (var overlay in this.pluginMain.Overlays)
            {
                AddConfigTab(overlay);
            }
        }

        private void AddConfigTab(IOverlay overlay)
        {
            var tabPage = new TabPage
            {
                Name = overlay.Name,
                Text = overlay.GetType().Name
            };

            var addon = pluginMain.Addons.FirstOrDefault(x => x.OverlayType == overlay.GetType());
            if (addon != null)
            {
                var control = addon.CreateOverlayConfigControlInstance(overlay);
                if (control != null)
                {
                    control.Dock = DockStyle.Fill;
                    control.BackColor = SystemColors.ControlLightLight;
                    tabPage.Controls.Add(control);

                    this.tabControl.TabPages.Add(tabPage);
                    this.tabControl.SelectTab(tabPage);
                }
            }
        }

        private void MenuLogCopy_Click(object sender, EventArgs e)
        {
            if (listViewLog.SelectedIndices.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (int index in listViewLog.SelectedIndices)
                {
                    sb.AppendFormat(
                        "{0}: {1}: {2}",
                        PluginMain.Logger.Logs[index].Time,
                        PluginMain.Logger.Logs[index].Level,
                        PluginMain.Logger.Logs[index].Message);
                    sb.AppendLine();
                }
                Clipboard.SetText(sb.ToString());
            }
        }

        private void ListViewLog_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= PluginMain.Logger.Logs.Count) 
            {
                e.Item = new ListViewItem();
                return;
            };

            var log = PluginMain.Logger.Logs[e.ItemIndex];
            e.Item = new ListViewItem(log.Time.ToString());
            e.Item.UseItemStyleForSubItems = true;
            e.Item.SubItems.Add(log.Level.ToString());
            e.Item.SubItems.Add(log.Message);

            e.Item.ForeColor = Color.Black;
            if (log.Level == LogLevel.Warning)
            {
                e.Item.BackColor = Color.LightYellow;
            }
            else if (log.Level == LogLevel.Error)
            {
                e.Item.BackColor = Color.LightPink;
            }
            else
            {
                e.Item.BackColor = Color.White;
            }
        }

        private void MenwFollowLatestLog_Click(object sender, EventArgs e)
        {
            this.config.FollowLatestLog = menuFollowLatestLog.Checked;
        }

        private void MenuClearLog_Click(object sender, EventArgs e)
        {
            PluginMain.Logger.Logs.Clear();
        }

        private void MenuCopyLogAll_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var log in PluginMain.Logger.Logs)
            {
                sb.AppendFormat(
                    "{0}: {1}: {2}",
                    log.Time,
                    log.Level,
                    log.Message);
                sb.AppendLine();
            }
            Clipboard.SetText(sb.ToString());
        }

        private void ButtonNewOverlay_Click(object sender, EventArgs e)
        {
            var newOverlayDialog = new NewOverlayDialog(pluginMain);
            newOverlayDialog.NameValidator = (name) =>
                {
                    // 空もしくは空白文字のみの名前は許容しない
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        MessageBox.Show("Name must not be empty or white space only.");
                        return false;
                    }
                    // 名前の重複も許容しない
                    else if (config.Overlays.Where(x => x.Name == name).Any())
                    {
                        MessageBox.Show("Name should be unique.");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                };
            
            if (newOverlayDialog.ShowDialog(this.ParentForm) == DialogResult.OK)
            {
                if (this.tabControl.TabCount == 1 && this.tabControl.TabPages[0].Equals(this.tabPageMain))
                {
                    this.tabControl.TabPages.Remove(this.tabPageMain);
                }
                CreateAndRegisterOverlay(newOverlayDialog.SelectedOverlayType, newOverlayDialog.OverlayName);
            }
            
            newOverlayDialog.Dispose();
        }

        private IOverlay CreateAndRegisterOverlay(IOverlayAddon overlayType, string name)
        {
            var config = overlayType.CreateOverlayConfigInstance(name);
            this.config.Overlays.Add(config);

            var overlay = overlayType.CreateOverlayInstance(config);
            pluginMain.RegisterOverlay(overlay);

            AddConfigTab(overlay);

            return overlay;
        }

        private void ButtonRemoveOverlay_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab.Equals(this.tabPageMain))
                return;

            if (tabControl.SelectedTab == null)
                return;
            
            var selectedOverlayName = tabControl.SelectedTab.Name;
            var selectedOverlayIndex = tabControl.TabPages.IndexOf(tabControl.SelectedTab);

            // コンフィグ削除
            var configs = this.config.Overlays.Where(x => x.Name == selectedOverlayName);
            foreach (var config in configs.ToArray())
            {
                this.config.Overlays.Remove(config);
            }

            // 動作中のオーバーレイを停止して削除
            var overlays = this.pluginMain.Overlays.Where(x => x.Name == selectedOverlayName);
            foreach (var overlay in overlays)
            {
                overlay.Dispose();
            }
            foreach (var overlay in overlays.ToArray())
            {
                this.pluginMain.Overlays.Remove(overlay);
            }

            // タブページを削除
            this.tabControl.TabPages.RemoveByKey(selectedOverlayName);

            // タープカントロールが
            if (this.tabControl.TabCount == 0)
            {
                this.tabControl.TabPages.Add(this.tabPageMain);
            }
            // 
            if (selectedOverlayIndex > 0)
            {
                this.tabControl.SelectTab(selectedOverlayIndex - 1);
            }

            // タープを更新
            this.tabControl.Update();
        }

        private void CheckBoxAutoHide_CheckedChanged(object sender, EventArgs e)
        {
            config.HideOverlaysWhenNotActive = checkBoxAutoHide.Checked;
        }

        private void ScreenshotPath_Leave(object sender, EventArgs e)
        {
            config.ScreenshotSavePath = screenShotPath.Text;
        }

        private void ScreenshotPathSelectButton_Click(object sender, EventArgs e)
        {
            this.fbdScreenshotPath.SelectedPath = this.screenShotPath.Text;
            
            if (this.fbdScreenshotPath.ShowDialog() == DialogResult.OK)
            {
                screenShotPath.Text = this.fbdScreenshotPath.SelectedPath;
                config.ScreenshotSavePath = this.fbdScreenshotPath.SelectedPath;
            }
        }

        private void ScreenshotBackgroundPath_Leave(object sender, EventArgs e)
        {
            config.ScreenshotBackgroundPath = screenShotBackgroundPath.Text;
        }

        private void ScreenshotBackgroundPathSelect_Click(object sender, EventArgs e)
        {
            this.ofdImage.FileName = this.screenShotBackgroundPath.Text;

            if (this.ofdImage.ShowDialog() == DialogResult.OK)
            {
                screenShotBackgroundPath.Text = this.ofdImage.FileName;
                config.ScreenshotBackgroundPath = this.ofdImage.FileName;
            }
        }

        private void ScreenshotBackgroundFillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.ScreenshotBackgroundMode = screenShotBackgroundMode.SelectedIndex;
        }

        private void ScreenshotAutoClipping_CheckedChanged(object sender, EventArgs e)
        {
            config.ScreenshotAutoClipping = screenShotAutoClipping.Checked;
        }

        private void ScreenshotMargin_ValueChanged(object sender, EventArgs e)
        {
            config.ScreenshotMargin = (int)screenShotMargin.Value;
        }

        private void TakeScreenshotBtn_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex < 0) return;
            var selTab = tabControl.SelectedIndex;

            if (pluginMain.Overlays.Count < 1) return;
            var selectedOverlay = pluginMain.Overlays[selTab];

            try
            {
                selectedOverlay.TakeScreenshot(
                    new ScreenshotConfig
                    {
                        SavePath            = config.ScreenshotSavePath,
                        AutoClipping        = config.ScreenshotAutoClipping,
                        BackgroundImagePath = config.ScreenshotBackgroundPath,
                        BackgroundMode      = (ScreenshotBackgroundMode)config.ScreenshotBackgroundMode,
                        Margin              = config.ScreenshotMargin,
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }
        }
    }
}
