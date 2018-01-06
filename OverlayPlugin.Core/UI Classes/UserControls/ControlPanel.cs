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
            this.listViewLog.VirtualListSize = pluginMain.Logger.Logs.Count;
            this.pluginMain.Logger.Logs.ListChanged += (o, e) =>
            {
                this.listViewLog.BeginUpdate();
                this.listViewLog.VirtualListSize = pluginMain.Logger.Logs.Count;
                if (this.config.FollowLatestLog && this.listViewLog.VirtualListSize > 0)
                {
                    this.listViewLog.EnsureVisible(this.listViewLog.VirtualListSize - 1);
                }
                this.listViewLog.EndUpdate();
            };

            InitializeOverlayConfigTabs();

            screenShotPath.Text                    = config.ScreenShotSavePath;
            screenShotBackgroundPath.Text          = config.ScreenShotBackgroundPath;
            screenShotBackgroundMode.SelectedIndex = config.ScreenShotBackgroundMode;
            screenShotAutoClipping.Checked         = config.ScreenShotAutoClipping;
            screenShotMargin.Value                 = config.ScreenShotMargin;

            if (string.IsNullOrWhiteSpace(screenShotPath.Text))
            {
                screenShotPath.Text = PluginMain.DefaultScreenShotPath;
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

        private void menuLogCopy_Click(object sender, EventArgs e)
        {
            if (listViewLog.SelectedIndices.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (int index in listViewLog.SelectedIndices)
                {
                    sb.AppendFormat(
                        "{0}: {1}: {2}",
                        pluginMain.Logger.Logs[index].Time,
                        pluginMain.Logger.Logs[index].Level,
                        pluginMain.Logger.Logs[index].Message);
                    sb.AppendLine();
                }
                Clipboard.SetText(sb.ToString());
            }
        }

        private void listViewLog_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= pluginMain.Logger.Logs.Count) 
            {
                e.Item = new ListViewItem();
                return;
            };

            var log = this.pluginMain.Logger.Logs[e.ItemIndex];
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

        private void menuFollowLatestLog_Click(object sender, EventArgs e)
        {
            this.config.FollowLatestLog = menuFollowLatestLog.Checked;
        }

        private void menuClearLog_Click(object sender, EventArgs e)
        {
            this.pluginMain.Logger.Logs.Clear();
        }

        private void menuCopyLogAll_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var log in this.pluginMain.Logger.Logs)
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

        private void buttonNewOverlay_Click(object sender, EventArgs e)
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

        private void buttonRemoveOverlay_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab.Equals(this.tabPageMain))
                return;

            if (tabControl.SelectedTab == null)
                return;
            
            string selectedOverlayName = tabControl.SelectedTab.Name;
            int selectedOverlayIndex = tabControl.TabPages.IndexOf(tabControl.SelectedTab);

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

        private void checkBoxAutoHide_CheckedChanged(object sender, EventArgs e)
        {
            config.HideOverlaysWhenNotActive = checkBoxAutoHide.Checked;
        }

        private void screenShotPath_Leave(object sender, EventArgs e)
        {
            config.ScreenShotSavePath = screenShotPath.Text;
        }

        private void screenShotPathSelectButton_Click(object sender, EventArgs e)
        {
            this.fbdScreenShotPath.SelectedPath = this.screenShotPath.Text;
            
            if (this.fbdScreenShotPath.ShowDialog() == DialogResult.OK)
            {
                screenShotPath.Text = this.fbdScreenShotPath.SelectedPath;
                config.ScreenShotSavePath = this.fbdScreenShotPath.SelectedPath;
            }
        }

        private void screenShotBackgroundPath_Leave(object sender, EventArgs e)
        {
            config.ScreenShotBackgroundPath = screenShotBackgroundPath.Text;
        }

        private void screenShotBackgroundPathSelect_Click(object sender, EventArgs e)
        {
            this.ofdImage.FileName = this.screenShotBackgroundPath.Text;

            if (this.ofdImage.ShowDialog() == DialogResult.OK)
            {
                screenShotBackgroundPath.Text = this.ofdImage.FileName;
                config.ScreenShotBackgroundPath = this.ofdImage.FileName;
            }
        }

        private void screenShotBackgroundFillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.ScreenShotBackgroundMode = screenShotBackgroundMode.SelectedIndex;
        }

        private void screenShotAutoClipping_CheckedChanged(object sender, EventArgs e)
        {
            config.ScreenShotAutoClipping = screenShotAutoClipping.Checked;
        }

        private void screenShotMargin_ValueChanged(object sender, EventArgs e)
        {
            config.ScreenShotMargin = (int)screenShotMargin.Value;
        }

        private void takeScreenShotBtn_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex < 0) return;
            var selTab = tabControl.SelectedIndex;

            if (pluginMain.Overlays.Count < 1) return;
            IOverlay selectedOverlay = pluginMain.Overlays[selTab];

            try
            {
                selectedOverlay.TakeScreenShot(
                    new ScreenShotConfig
                    {
                        SavePath            = config.ScreenShotSavePath,
                        AutoClipping        = config.ScreenShotAutoClipping,
                        BackgroundImagePath = config.ScreenShotBackgroundPath,
                        BackgroundMode      = (ScreenShotBackgroundMode)config.ScreenShotBackgroundMode,
                        Margin              = config.ScreenShotMargin,
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }
        }
    }
}
