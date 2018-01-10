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
            this.contextMenuLogList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopyLogAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFollowLatestLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageMain = new RainbowMage.OverlayPlugin.OverlayPageUI();
            this.label_ListEmpty = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fbdScreenshotPath = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdImage = new System.Windows.Forms.OpenFileDialog();
            this.overlayTabUI1 = new RainbowMage.OverlayPlugin.OverlayTabUI();
            this.overlaysTabPage = new RainbowMage.OverlayPlugin.OverlayPageUI();
            this.tabControl = new RainbowMage.OverlayPlugin.TabControlExt();
            this.panel1 = new System.Windows.Forms.Panel();
            this.takeScreenshotBtn = new System.Windows.Forms.Button();
            this.checkBoxAutoHide = new System.Windows.Forms.CheckBox();
            this.buttonNewOverlay = new System.Windows.Forms.Button();
            this.buttonRemoveOverlay = new System.Windows.Forms.Button();
            this.overlayLogsTabPage = new RainbowMage.OverlayPlugin.OverlayPageUI();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.overlaySettingsTabPage = new RainbowMage.OverlayPlugin.OverlayPageUI();
            this.screenShotPathGroupbox = new System.Windows.Forms.GroupBox();
            this.screenShotTable = new System.Windows.Forms.TableLayoutPanel();
            this.screenShotMarginlabel = new System.Windows.Forms.Label();
            this.screenShotAutoClippingLabel = new System.Windows.Forms.Label();
            this.screenShotBackgroundModeLabel = new System.Windows.Forms.Label();
            this.screenShotBackgroundPathLabel = new System.Windows.Forms.Label();
            this.screenShotPathPanel = new System.Windows.Forms.Panel();
            this.screenShotPath = new System.Windows.Forms.TextBox();
            this.screenShotPathSelectButton = new System.Windows.Forms.Button();
            this.screenShotBackgroundPathPanel = new System.Windows.Forms.Panel();
            this.screenShotBackgroundPath = new System.Windows.Forms.TextBox();
            this.screenShotBackgroundPathSelect = new System.Windows.Forms.Button();
            this.screenShotAutoClipping = new System.Windows.Forms.CheckBox();
            this.screenShotBackgroundMode = new System.Windows.Forms.ComboBox();
            this.screenShotPathLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.screenShotMargin = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.overlayInformationTabPage = new RainbowMage.OverlayPlugin.OverlayPageUI();
            this.overlayPluginInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.repo_dev_info_special = new System.Windows.Forms.Label();
            this.special_thanks = new System.Windows.Forms.Label();
            this.repo_dev_info = new System.Windows.Forms.Label();
            this.developers = new System.Windows.Forms.Label();
            this.contextMenuLogList.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.overlayTabUI1.SuspendLayout();
            this.overlaysTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.overlayLogsTabPage.SuspendLayout();
            this.panel2.SuspendLayout();
            this.overlaySettingsTabPage.SuspendLayout();
            this.screenShotPathGroupbox.SuspendLayout();
            this.screenShotTable.SuspendLayout();
            this.screenShotPathPanel.SuspendLayout();
            this.screenShotBackgroundPathPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenShotMargin)).BeginInit();
            this.overlayInformationTabPage.SuspendLayout();
            this.overlayPluginInformationGroupBox.SuspendLayout();
            this.SuspendLayout();
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
            this.menuCopyLogAll.Click += new System.EventHandler(this.MenuCopyLogAll_Click);
            // 
            // menuLogCopy
            // 
            this.menuLogCopy.Name = "menuLogCopy";
            resources.ApplyResources(this.menuLogCopy, "menuLogCopy");
            this.menuLogCopy.Click += new System.EventHandler(this.MenuLogCopy_Click);
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
            this.menuFollowLatestLog.Click += new System.EventHandler(this.MenwFollowLatestLog_Click);
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
            this.menuClearLog.Click += new System.EventHandler(this.MenuClearLog_Click);
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.label_ListEmpty);
            resources.ApplyResources(this.tabPageMain, "tabPageMain");
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.TabImage = null;
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
            // ofdImage
            // 
            resources.ApplyResources(this.ofdImage, "ofdImage");
            // 
            // overlayTabUI1
            // 
            this.overlayTabUI1.Controls.Add(this.overlaysTabPage);
            this.overlayTabUI1.Controls.Add(this.overlayLogsTabPage);
            this.overlayTabUI1.Controls.Add(this.overlaySettingsTabPage);
            this.overlayTabUI1.Controls.Add(this.overlayInformationTabPage);
            resources.ApplyResources(this.overlayTabUI1, "overlayTabUI1");
            this.overlayTabUI1.Name = "overlayTabUI1";
            this.overlayTabUI1.SelectedIndex = 0;
            // 
            // overlaysTabPage
            // 
            this.overlaysTabPage.Controls.Add(this.tabControl);
            this.overlaysTabPage.Controls.Add(this.panel1);
            resources.ApplyResources(this.overlaysTabPage, "overlaysTabPage");
            this.overlaysTabPage.Name = "overlaysTabPage";
            this.overlaysTabPage.TabImage = ((System.Drawing.Image)(resources.GetObject("overlaysTabPage.TabImage")));
            this.overlaysTabPage.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.takeScreenshotBtn);
            this.panel1.Controls.Add(this.checkBoxAutoHide);
            this.panel1.Controls.Add(this.buttonNewOverlay);
            this.panel1.Controls.Add(this.buttonRemoveOverlay);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // takeScreenshotBtn
            // 
            resources.ApplyResources(this.takeScreenshotBtn, "takeScreenshotBtn");
            this.takeScreenshotBtn.Name = "takeScreenshotBtn";
            this.takeScreenshotBtn.UseVisualStyleBackColor = true;
            this.takeScreenshotBtn.Click += new System.EventHandler(this.TakeScreenshotBtn_Click);
            // 
            // checkBoxAutoHide
            // 
            resources.ApplyResources(this.checkBoxAutoHide, "checkBoxAutoHide");
            this.checkBoxAutoHide.Name = "checkBoxAutoHide";
            this.checkBoxAutoHide.UseVisualStyleBackColor = true;
            this.checkBoxAutoHide.CheckedChanged += new System.EventHandler(this.CheckBoxAutoHide_CheckedChanged);
            // 
            // buttonNewOverlay
            // 
            resources.ApplyResources(this.buttonNewOverlay, "buttonNewOverlay");
            this.buttonNewOverlay.Name = "buttonNewOverlay";
            this.buttonNewOverlay.UseVisualStyleBackColor = true;
            this.buttonNewOverlay.Click += new System.EventHandler(this.ButtonNewOverlay_Click);
            // 
            // buttonRemoveOverlay
            // 
            resources.ApplyResources(this.buttonRemoveOverlay, "buttonRemoveOverlay");
            this.buttonRemoveOverlay.Name = "buttonRemoveOverlay";
            this.buttonRemoveOverlay.UseVisualStyleBackColor = true;
            this.buttonRemoveOverlay.Click += new System.EventHandler(this.ButtonRemoveOverlay_Click);
            // 
            // overlayLogsTabPage
            // 
            this.overlayLogsTabPage.Controls.Add(this.listViewLog);
            this.overlayLogsTabPage.Controls.Add(this.panel2);
            resources.ApplyResources(this.overlayLogsTabPage, "overlayLogsTabPage");
            this.overlayLogsTabPage.Name = "overlayLogsTabPage";
            this.overlayLogsTabPage.TabImage = ((System.Drawing.Image)(resources.GetObject("overlayLogsTabPage.TabImage")));
            this.overlayLogsTabPage.UseVisualStyleBackColor = true;
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
            this.listViewLog.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ListViewLog_RetrieveVirtualItem);
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
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.MenuClearLog_Click);
            // 
            // overlaySettingsTabPage
            // 
            this.overlaySettingsTabPage.Controls.Add(this.screenShotPathGroupbox);
            resources.ApplyResources(this.overlaySettingsTabPage, "overlaySettingsTabPage");
            this.overlaySettingsTabPage.Name = "overlaySettingsTabPage";
            this.overlaySettingsTabPage.TabImage = ((System.Drawing.Image)(resources.GetObject("overlaySettingsTabPage.TabImage")));
            this.overlaySettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // screenShotPathGroupbox
            // 
            resources.ApplyResources(this.screenShotPathGroupbox, "screenShotPathGroupbox");
            this.screenShotPathGroupbox.Controls.Add(this.screenShotTable);
            this.screenShotPathGroupbox.Name = "screenShotPathGroupbox";
            this.screenShotPathGroupbox.TabStop = false;
            // 
            // screenShotTable
            // 
            resources.ApplyResources(this.screenShotTable, "screenShotTable");
            this.screenShotTable.Controls.Add(this.screenShotMarginlabel, 0, 4);
            this.screenShotTable.Controls.Add(this.screenShotAutoClippingLabel, 0, 3);
            this.screenShotTable.Controls.Add(this.screenShotBackgroundModeLabel, 0, 2);
            this.screenShotTable.Controls.Add(this.screenShotBackgroundPathLabel, 0, 1);
            this.screenShotTable.Controls.Add(this.screenShotPathPanel, 1, 0);
            this.screenShotTable.Controls.Add(this.screenShotBackgroundPathPanel, 1, 1);
            this.screenShotTable.Controls.Add(this.screenShotAutoClipping, 1, 3);
            this.screenShotTable.Controls.Add(this.screenShotBackgroundMode, 1, 2);
            this.screenShotTable.Controls.Add(this.screenShotPathLabel, 0, 0);
            this.screenShotTable.Controls.Add(this.tableLayoutPanel1, 1, 4);
            this.screenShotTable.Name = "screenShotTable";
            // 
            // screenShotMarginlabel
            // 
            resources.ApplyResources(this.screenShotMarginlabel, "screenShotMarginlabel");
            this.screenShotMarginlabel.Name = "screenShotMarginlabel";
            // 
            // screenShotAutoClippingLabel
            // 
            resources.ApplyResources(this.screenShotAutoClippingLabel, "screenShotAutoClippingLabel");
            this.screenShotAutoClippingLabel.Name = "screenShotAutoClippingLabel";
            // 
            // screenShotBackgroundModeLabel
            // 
            resources.ApplyResources(this.screenShotBackgroundModeLabel, "screenShotBackgroundModeLabel");
            this.screenShotBackgroundModeLabel.Name = "screenShotBackgroundModeLabel";
            // 
            // screenShotBackgroundPathLabel
            // 
            resources.ApplyResources(this.screenShotBackgroundPathLabel, "screenShotBackgroundPathLabel");
            this.screenShotBackgroundPathLabel.Name = "screenShotBackgroundPathLabel";
            // 
            // screenShotPathPanel
            // 
            this.screenShotPathPanel.Controls.Add(this.screenShotPath);
            this.screenShotPathPanel.Controls.Add(this.screenShotPathSelectButton);
            resources.ApplyResources(this.screenShotPathPanel, "screenShotPathPanel");
            this.screenShotPathPanel.Name = "screenShotPathPanel";
            // 
            // screenShotPath
            // 
            resources.ApplyResources(this.screenShotPath, "screenShotPath");
            this.screenShotPath.Name = "screenShotPath";
            // 
            // screenShotPathSelectButton
            // 
            resources.ApplyResources(this.screenShotPathSelectButton, "screenShotPathSelectButton");
            this.screenShotPathSelectButton.Name = "screenShotPathSelectButton";
            this.screenShotPathSelectButton.UseVisualStyleBackColor = true;
            this.screenShotPathSelectButton.Click += new System.EventHandler(this.ScreenshotPathSelectButton_Click);
            // 
            // screenShotBackgroundPathPanel
            // 
            this.screenShotBackgroundPathPanel.Controls.Add(this.screenShotBackgroundPath);
            this.screenShotBackgroundPathPanel.Controls.Add(this.screenShotBackgroundPathSelect);
            resources.ApplyResources(this.screenShotBackgroundPathPanel, "screenShotBackgroundPathPanel");
            this.screenShotBackgroundPathPanel.Name = "screenShotBackgroundPathPanel";
            // 
            // screenShotBackgroundPath
            // 
            resources.ApplyResources(this.screenShotBackgroundPath, "screenShotBackgroundPath");
            this.screenShotBackgroundPath.Name = "screenShotBackgroundPath";
            // 
            // screenShotBackgroundPathSelect
            // 
            resources.ApplyResources(this.screenShotBackgroundPathSelect, "screenShotBackgroundPathSelect");
            this.screenShotBackgroundPathSelect.Name = "screenShotBackgroundPathSelect";
            this.screenShotBackgroundPathSelect.UseVisualStyleBackColor = true;
            this.screenShotBackgroundPathSelect.Click += new System.EventHandler(this.ScreenshotBackgroundPathSelect_Click);
            // 
            // screenShotAutoClipping
            // 
            resources.ApplyResources(this.screenShotAutoClipping, "screenShotAutoClipping");
            this.screenShotAutoClipping.Name = "screenShotAutoClipping";
            this.screenShotAutoClipping.UseVisualStyleBackColor = true;
            this.screenShotAutoClipping.CheckedChanged += new System.EventHandler(this.ScreenshotAutoClipping_CheckedChanged);
            // 
            // screenShotBackgroundMode
            // 
            resources.ApplyResources(this.screenShotBackgroundMode, "screenShotBackgroundMode");
            this.screenShotBackgroundMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.screenShotBackgroundMode.FormattingEnabled = true;
            this.screenShotBackgroundMode.Items.AddRange(new object[] {
            resources.GetString("screenShotBackgroundMode.Items"),
            resources.GetString("screenShotBackgroundMode.Items1"),
            resources.GetString("screenShotBackgroundMode.Items2"),
            resources.GetString("screenShotBackgroundMode.Items3"),
            resources.GetString("screenShotBackgroundMode.Items4"),
            resources.GetString("screenShotBackgroundMode.Items5")});
            this.screenShotBackgroundMode.Name = "screenShotBackgroundMode";
            this.screenShotBackgroundMode.SelectedIndexChanged += new System.EventHandler(this.ScreenshotBackgroundFillType_SelectedIndexChanged);
            // 
            // screenShotPathLabel
            // 
            resources.ApplyResources(this.screenShotPathLabel, "screenShotPathLabel");
            this.screenShotPathLabel.Name = "screenShotPathLabel";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.screenShotMargin, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // screenShotMargin
            // 
            resources.ApplyResources(this.screenShotMargin, "screenShotMargin");
            this.screenShotMargin.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.screenShotMargin.Name = "screenShotMargin";
            this.screenShotMargin.ValueChanged += new System.EventHandler(this.ScreenshotMargin_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // overlayInformationTabPage
            // 
            this.overlayInformationTabPage.Controls.Add(this.overlayPluginInformationGroupBox);
            resources.ApplyResources(this.overlayInformationTabPage, "overlayInformationTabPage");
            this.overlayInformationTabPage.Name = "overlayInformationTabPage";
            this.overlayInformationTabPage.TabImage = ((System.Drawing.Image)(resources.GetObject("overlayInformationTabPage.TabImage")));
            this.overlayInformationTabPage.UseVisualStyleBackColor = true;
            // 
            // overlayPluginInformationGroupBox
            // 
            this.overlayPluginInformationGroupBox.Controls.Add(this.repo_dev_info_special);
            this.overlayPluginInformationGroupBox.Controls.Add(this.special_thanks);
            this.overlayPluginInformationGroupBox.Controls.Add(this.repo_dev_info);
            this.overlayPluginInformationGroupBox.Controls.Add(this.developers);
            resources.ApplyResources(this.overlayPluginInformationGroupBox, "overlayPluginInformationGroupBox");
            this.overlayPluginInformationGroupBox.Name = "overlayPluginInformationGroupBox";
            this.overlayPluginInformationGroupBox.TabStop = false;
            // 
            // repo_dev_info_special
            // 
            resources.ApplyResources(this.repo_dev_info_special, "repo_dev_info_special");
            this.repo_dev_info_special.Name = "repo_dev_info_special";
            // 
            // special_thanks
            // 
            resources.ApplyResources(this.special_thanks, "special_thanks");
            this.special_thanks.Name = "special_thanks";
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
            // ControlPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.overlayTabUI1);
            this.Name = "ControlPanel";
            this.contextMenuLogList.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.overlayTabUI1.ResumeLayout(false);
            this.overlaysTabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.overlayLogsTabPage.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.overlaySettingsTabPage.ResumeLayout(false);
            this.overlaySettingsTabPage.PerformLayout();
            this.screenShotPathGroupbox.ResumeLayout(false);
            this.screenShotPathGroupbox.PerformLayout();
            this.screenShotTable.ResumeLayout(false);
            this.screenShotPathPanel.ResumeLayout(false);
            this.screenShotPathPanel.PerformLayout();
            this.screenShotBackgroundPathPanel.ResumeLayout(false);
            this.screenShotBackgroundPathPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.screenShotMargin)).EndInit();
            this.overlayInformationTabPage.ResumeLayout(false);
            this.overlayPluginInformationGroupBox.ResumeLayout(false);
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
        private OverlayPageUI tabPageMain;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonRemoveOverlay;
        private System.Windows.Forms.Button buttonNewOverlay;
        private System.Windows.Forms.CheckBox checkBoxAutoHide;
        private Label label_ListEmpty;
        private OverlayTabUI overlayTabUI1;
        private OverlayPageUI overlaysTabPage;
        private OverlayPageUI overlayLogsTabPage;
        private OverlayPageUI overlayInformationTabPage;
        private Panel panel1;
        private Button takeScreenshotBtn;
        private FolderBrowserDialog fbdScreenshotPath;
        private OpenFileDialog ofdImage;
        private OverlayPageUI overlaySettingsTabPage;
        private GroupBox screenShotPathGroupbox;
        private TableLayoutPanel screenShotTable;
        private Label screenShotMarginlabel;
        private Label screenShotAutoClippingLabel;
        private Label screenShotBackgroundModeLabel;
        private Label screenShotBackgroundPathLabel;
        private Panel screenShotPathPanel;
        private TextBox screenShotPath;
        private Button screenShotPathSelectButton;
        private Panel screenShotBackgroundPathPanel;
        private TextBox screenShotBackgroundPath;
        private Button screenShotBackgroundPathSelect;
        private CheckBox screenShotAutoClipping;
        private ComboBox screenShotBackgroundMode;
        private NumericUpDown screenShotMargin;
        private Label screenShotPathLabel;
        private GroupBox overlayPluginInformationGroupBox;
        private Label repo_dev_info_special;
        private Label special_thanks;
        private Label repo_dev_info;
        private Label developers;
        private Panel panel2;
        private Button button1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
    }
}
