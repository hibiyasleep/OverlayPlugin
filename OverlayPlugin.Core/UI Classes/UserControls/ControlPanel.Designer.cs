using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    partial class ControlPanel
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonNewOverlay = new System.Windows.Forms.Button();
            this.buttonRemoveOverlay = new System.Windows.Forms.Button();
            this.checkBoxAutoHide = new System.Windows.Forms.CheckBox();
            this.tabControl = new RainbowMage.OverlayPlugin.TabControlExt();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuLogList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopyLogAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFollowLatestLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.label_ListEmpty = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.overlayTabUI1 = new RainbowMage.OverlayPlugin.OverlayTabUI();
            this.overlaysTabPage = new System.Windows.Forms.TabPage();
            this.overlayLogsTabPage = new System.Windows.Forms.TabPage();
            this.overlayInformationTabPage = new System.Windows.Forms.TabPage();
            this.overlayPluginInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.repo_dev_info = new System.Windows.Forms.Label();
            this.developers = new System.Windows.Forms.Label();
            this.screenShotHotkeyGroupBox = new System.Windows.Forms.GroupBox();
            this.screenShotHotkeyWarningLabel = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.screenShotPathGroupbox = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel.SuspendLayout();
            this.contextMenuLogList.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.overlayTabUI1.SuspendLayout();
            this.overlaysTabPage.SuspendLayout();
            this.overlayLogsTabPage.SuspendLayout();
            this.overlayInformationTabPage.SuspendLayout();
            this.overlayPluginInformationGroupBox.SuspendLayout();
            this.screenShotHotkeyGroupBox.SuspendLayout();
            this.screenShotPathGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.buttonNewOverlay);
            this.flowLayoutPanel.Controls.Add(this.buttonRemoveOverlay);
            this.flowLayoutPanel.Controls.Add(this.checkBoxAutoHide);
            resources.ApplyResources(this.flowLayoutPanel, "flowLayoutPanel");
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            // 
            // buttonNewOverlay
            // 
            resources.ApplyResources(this.buttonNewOverlay, "buttonNewOverlay");
            this.buttonNewOverlay.Name = "buttonNewOverlay";
            this.buttonNewOverlay.UseVisualStyleBackColor = true;
            this.buttonNewOverlay.Click += new System.EventHandler(this.buttonNewOverlay_Click);
            // 
            // buttonRemoveOverlay
            // 
            resources.ApplyResources(this.buttonRemoveOverlay, "buttonRemoveOverlay");
            this.buttonRemoveOverlay.Name = "buttonRemoveOverlay";
            this.buttonRemoveOverlay.UseVisualStyleBackColor = true;
            this.buttonRemoveOverlay.Click += new System.EventHandler(this.buttonRemoveOverlay_Click);
            // 
            // checkBoxAutoHide
            // 
            resources.ApplyResources(this.checkBoxAutoHide, "checkBoxAutoHide");
            this.checkBoxAutoHide.Name = "checkBoxAutoHide";
            this.checkBoxAutoHide.UseVisualStyleBackColor = true;
            this.checkBoxAutoHide.CheckedChanged += new System.EventHandler(this.checkBoxAutoHide_CheckedChanged);
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // listViewLog
            // 
            this.listViewLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewLog.ContextMenuStrip = this.contextMenuLogList;
            resources.ApplyResources(this.listViewLog, "listViewLog");
            this.listViewLog.FullRowSelect = true;
            this.listViewLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewLog.HideSelection = false;
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            this.listViewLog.VirtualMode = true;
            this.listViewLog.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewLog_RetrieveVirtualItem);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // contextMenuLogList
            // 
            this.contextMenuLogList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopyLogAll,
            this.menuLogCopy,
            this.toolStripMenuItem1,
            this.menuFollowLatestLog,
            this.toolStripMenuItem2,
            this.menuClearLog});
            this.contextMenuLogList.Name = "contextMenuLogList";
            resources.ApplyResources(this.contextMenuLogList, "contextMenuLogList");
            // 
            // menuCopyLogAll
            // 
            this.menuCopyLogAll.Name = "menuCopyLogAll";
            resources.ApplyResources(this.menuCopyLogAll, "menuCopyLogAll");
            this.menuCopyLogAll.Click += new System.EventHandler(this.menuCopyLogAll_Click);
            // 
            // menuLogCopy
            // 
            this.menuLogCopy.Name = "menuLogCopy";
            resources.ApplyResources(this.menuLogCopy, "menuLogCopy");
            this.menuLogCopy.Click += new System.EventHandler(this.menuLogCopy_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // menuFollowLatestLog
            // 
            this.menuFollowLatestLog.CheckOnClick = true;
            this.menuFollowLatestLog.Name = "menuFollowLatestLog";
            resources.ApplyResources(this.menuFollowLatestLog, "menuFollowLatestLog");
            this.menuFollowLatestLog.Click += new System.EventHandler(this.menuFollowLatestLog_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // menuClearLog
            // 
            this.menuClearLog.Name = "menuClearLog";
            resources.ApplyResources(this.menuClearLog, "menuClearLog");
            this.menuClearLog.Click += new System.EventHandler(this.menuClearLog_Click);
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.label_ListEmpty);
            resources.ApplyResources(this.tabPageMain, "tabPageMain");
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // label_ListEmpty
            // 
            resources.ApplyResources(this.label_ListEmpty, "label_ListEmpty");
            this.label_ListEmpty.Name = "label_ListEmpty";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // overlayTabUI1
            // 
            this.overlayTabUI1.Controls.Add(this.overlaysTabPage);
            this.overlayTabUI1.Controls.Add(this.overlayLogsTabPage);
            this.overlayTabUI1.Controls.Add(this.overlayInformationTabPage);
            resources.ApplyResources(this.overlayTabUI1, "overlayTabUI1");
            this.overlayTabUI1.Name = "overlayTabUI1";
            this.overlayTabUI1.SelectedIndex = 0;
            // 
            // overlaysTabPage
            // 
            this.overlaysTabPage.Controls.Add(this.tabControl);
            this.overlaysTabPage.Controls.Add(this.flowLayoutPanel);
            resources.ApplyResources(this.overlaysTabPage, "overlaysTabPage");
            this.overlaysTabPage.Name = "overlaysTabPage";
            this.overlaysTabPage.UseVisualStyleBackColor = true;
            // 
            // overlayLogsTabPage
            // 
            this.overlayLogsTabPage.Controls.Add(this.listViewLog);
            resources.ApplyResources(this.overlayLogsTabPage, "overlayLogsTabPage");
            this.overlayLogsTabPage.Name = "overlayLogsTabPage";
            this.overlayLogsTabPage.UseVisualStyleBackColor = true;
            // 
            // overlayInformationTabPage
            // 
            this.overlayInformationTabPage.Controls.Add(this.overlayPluginInformationGroupBox);
            this.overlayInformationTabPage.Controls.Add(this.screenShotHotkeyGroupBox);
            this.overlayInformationTabPage.Controls.Add(this.screenShotPathGroupbox);
            resources.ApplyResources(this.overlayInformationTabPage, "overlayInformationTabPage");
            this.overlayInformationTabPage.Name = "overlayInformationTabPage";
            this.overlayInformationTabPage.UseVisualStyleBackColor = true;
            // 
            // overlayPluginInformationGroupBox
            // 
            this.overlayPluginInformationGroupBox.Controls.Add(this.repo_dev_info);
            this.overlayPluginInformationGroupBox.Controls.Add(this.developers);
            resources.ApplyResources(this.overlayPluginInformationGroupBox, "overlayPluginInformationGroupBox");
            this.overlayPluginInformationGroupBox.Name = "overlayPluginInformationGroupBox";
            this.overlayPluginInformationGroupBox.TabStop = false;
            // 
            // repo_dev_info
            // 
            resources.ApplyResources(this.repo_dev_info, "repo_dev_info");
            this.repo_dev_info.Name = "repo_dev_info";
            // 
            // developers
            // 
            resources.ApplyResources(this.developers, "developers");
            this.developers.Name = "developers";
            // 
            // screenShotHotkeyGroupBox
            // 
            this.screenShotHotkeyGroupBox.Controls.Add(this.screenShotHotkeyWarningLabel);
            this.screenShotHotkeyGroupBox.Controls.Add(this.textBox2);
            resources.ApplyResources(this.screenShotHotkeyGroupBox, "screenShotHotkeyGroupBox");
            this.screenShotHotkeyGroupBox.Name = "screenShotHotkeyGroupBox";
            this.screenShotHotkeyGroupBox.TabStop = false;
            // 
            // screenShotHotkeyWarningLabel
            // 
            resources.ApplyResources(this.screenShotHotkeyWarningLabel, "screenShotHotkeyWarningLabel");
            this.screenShotHotkeyWarningLabel.Name = "screenShotHotkeyWarningLabel";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // screenShotPathGroupbox
            // 
            this.screenShotPathGroupbox.Controls.Add(this.textBox1);
            this.screenShotPathGroupbox.Controls.Add(this.button1);
            resources.ApplyResources(this.screenShotPathGroupbox, "screenShotPathGroupbox");
            this.screenShotPathGroupbox.Name = "screenShotPathGroupbox";
            this.screenShotPathGroupbox.TabStop = false;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ControlPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.overlayTabUI1);
            this.Name = "ControlPanel";
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.contextMenuLogList.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.overlayTabUI1.ResumeLayout(false);
            this.overlaysTabPage.ResumeLayout(false);
            this.overlayLogsTabPage.ResumeLayout(false);
            this.overlayInformationTabPage.ResumeLayout(false);
            this.overlayPluginInformationGroupBox.ResumeLayout(false);
            this.screenShotHotkeyGroupBox.ResumeLayout(false);
            this.screenShotHotkeyGroupBox.PerformLayout();
            this.screenShotPathGroupbox.ResumeLayout(false);
            this.screenShotPathGroupbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuLogList;
        private System.Windows.Forms.ToolStripMenuItem menuLogCopy;
        private RainbowMage.OverlayPlugin.TabControlExt tabControl;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuFollowLatestLog;
        private System.Windows.Forms.ToolStripMenuItem menuCopyLogAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuClearLog;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonRemoveOverlay;
        private System.Windows.Forms.Button buttonNewOverlay;
        private System.Windows.Forms.CheckBox checkBoxAutoHide;
        private FlowLayoutPanel flowLayoutPanel;
        private Label label_ListEmpty;
        private OverlayTabUI overlayTabUI1;
        private TabPage overlaysTabPage;
        private TabPage overlayLogsTabPage;
        private TabPage overlayInformationTabPage;
        private GroupBox screenShotPathGroupbox;
        private TextBox textBox1;
        private Button button1;
        private GroupBox screenShotHotkeyGroupBox;
        private TextBox textBox2;
        private GroupBox overlayPluginInformationGroupBox;
        private Label repo_dev_info;
        private Label developers;
        private Label screenShotHotkeyWarningLabel;
    }
}