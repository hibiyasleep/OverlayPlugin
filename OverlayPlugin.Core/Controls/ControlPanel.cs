using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace RainbowMage.OverlayPlugin
{
    public partial class ControlPanel : UserControl
    {
        PluginMain pluginMain;
        PluginConfig config;
        TabPage generalTab;

        static Dictionary<string, string> esNames = new Dictionary<string, string>
        {
            { "MiniParseEventSource", Resources.MapESMiniParse },
        };
        static Dictionary<string, string> overlayNames = new Dictionary<string, string>
        {
            { "LabelOverlay", Resources.MapOverlayLabel },
            { "MiniParseOverlay", Resources.MapOverlayMiniParse },
            { "SpellTimerOverlay", Resources.MapOverlaySpellTimer },
        };

        public ControlPanel(PluginMain pluginMain, PluginConfig config)
        {
            InitializeComponent();
            tableLayoutPanel0.PerformLayout();
            splitContainer1.SplitterDistance = splitContainer1.Height - 180;

            this.pluginMain = pluginMain;
            this.config = config;

            this.checkBoxFollowLog.Checked = this.config.FollowLatestLog;

            generalTab = new ConfigTabPage
            {
                Name = Resources.GeneralTab,
                Text = "",
            };
            generalTab.Controls.Add(new GeneralConfigTab());

            PluginMain.Logger.RegisterListener(AddLogEntry);
            Registry.EventSourceRegistered += (o, e) => Invoke((Action)(() => AddEventSourceTab(o, e)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
                Registry.EventSourceRegistered -= AddEventSourceTab;
                PluginMain.Logger.ClearListener();
            }

            base.Dispose(disposing);
        }

        private void AddLogEntry(LogEntry entry)
        {
            var msg = $"[{entry.Time}] {entry.Level}: {entry.Message}" + Environment.NewLine;

            if (checkBoxFollowLog.Checked)
            {
                logBox.AppendText(msg);
            }
            else
            {
                // This is based on https://stackoverflow.com/q/1743448
                bool bottomFlag = false;
                int sbOffset;
                int savedVpos;

                // Win32 magic to keep the textbox scrolling to the newest append to the textbox unless
                // the user has moved the scrollbox up
                sbOffset = (int)((logBox.ClientSize.Height - SystemInformation.HorizontalScrollBarHeight) / (logBox.Font.Height));
                savedVpos = NativeMethods.GetScrollPos(logBox.Handle, NativeMethods.SB_VERT);
                NativeMethods.GetScrollRange(logBox.Handle, NativeMethods.SB_VERT, out _, out int VSmax);

                if (savedVpos >= (VSmax - sbOffset - 1))
                    bottomFlag = true;

                logBox.AppendText(msg);

                if (bottomFlag)
                {
                    NativeMethods.GetScrollRange(logBox.Handle, NativeMethods.SB_VERT, out _, out VSmax);
                    savedVpos = VSmax - sbOffset;
                }
                NativeMethods.SetScrollPos(logBox.Handle, NativeMethods.SB_VERT, savedVpos, true);
                NativeMethods.PostMessageA(logBox.Handle, NativeMethods.WM_VSCROLL, NativeMethods.SB_THUMBPOSITION + 0x10000 * savedVpos, 0);
            }
        }

        private void AddEventSourceTab(object sender, EventSourceRegisteredEventArgs e)
        {
            AddConfigTab(e.EventSource);
        }

        public void InitializeOverlayConfigTabs()
        {
            tabControl.TabPages.Clear();
            tabControl.TabPages.Add(generalTab);

            foreach (var source in Registry.EventSources)
            {
                AddConfigTab(source);
            }

            foreach (var overlay in this.pluginMain.Overlays)
            {
                AddConfigTab(overlay);
            }

            if (tabControl.TabCount == 0)
            {
                tabControl.TabPages.Add(this.tabPageMain);
            }
        }

        private void AddConfigTab(IOverlay overlay)
        {
            var label = overlay.GetType().Name;
            if (overlayNames.ContainsKey(label))
                label = overlayNames[label];

            var tabPage = new ConfigTabPage
            {
                Name = overlay.Name,
                Text = label,
                IsOverlay = true,
            };

            var control = overlay.CreateConfigControl();
            if (control != null)
            {
                control.Dock = DockStyle.Fill;
                control.BackColor = SystemColors.ControlLightLight;
                tabPage.Controls.Add(control);

                this.tabControl.TabPages.Add(tabPage);
            }
        }

        private void AddConfigTab(IEventSource source)
        {
            var label = source.GetType().Name;
            if (esNames.ContainsKey(label))
                label = esNames[label];

            var tabPage = new ConfigTabPage
            {
                Name = source.Name,
                Text = Resources.TabsESLabel + " " + label,
                IsEventSource = true,
            };

            var control = source.CreateConfigControl();
            if (control != null)
            {
                control.Dock = DockStyle.Fill;
                control.BackColor = SystemColors.ControlLightLight;
                tabPage.Controls.Add(control);

                var index = 0;
                foreach (var page in this.tabControl.TabPages)
                {
                    if (index == 0 || ((ConfigTabPage) page).IsEventSource)
                    {
                        index++;
                    } else
                    {
                        break;
                    }
                }

                this.tabControl.TabPages.Insert(index, tabPage);
            }
        }

        private void buttonNewOverlay_Click(object sender, EventArgs e)
        {
            var newOverlayDialog = new NewOverlayDialog(pluginMain);
            newOverlayDialog.NameValidator = (name) =>
                {
                    // 空もしくは空白文字のみの名前は許容しない
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        MessageBox.Show(Resources.ErrorOverlayNameEmpty);
                        return false;
                    }
                    // 名前の重複も許容しない
                    else if (config.Overlays.Where(x => x.Name == name).Any())
                    {
                        MessageBox.Show(Resources.ErrorOverlayNameNotUnique);
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

        private IOverlay CreateAndRegisterOverlay(Type overlayType, string name)
        {
            var parameters = new NamedParameterOverloads();
            parameters["config"] = null;
            parameters["name"] = name;

            var overlay = (IOverlay) Registry.Container.Resolve(overlayType, parameters);

            config.Overlays.Add(overlay.Config);
            pluginMain.RegisterOverlay(overlay);

            AddConfigTab(overlay);

            return overlay;
        }

        private void buttonRemoveOverlay_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) // ???
                tabControl.SelectedTab = tabControl.TabPages[0];

            var subLabel = tabControl.SelectedTab.Text;
            if (!((ConfigTabPage) tabControl.SelectedTab).IsOverlay)
            {
                return;
            }

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
            this.tabControl.TabPages.Remove(tabControl.SelectedTab);

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

        private void CheckBoxFollowLog_CheckedChanged(object sender, EventArgs e)
        {
            config.FollowLatestLog = checkBoxFollowLog.Checked;
        }

        private void ButtonClearLog_Click(object sender, EventArgs e)
        {
            logBox.Clear();
        }

        private class ConfigTabPage : TabPage
        {
            public bool IsOverlay = false;
            public bool IsEventSource = false;
        }
    }
}
