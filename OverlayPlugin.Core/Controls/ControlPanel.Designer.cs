using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    partial class ControlPanel
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel0 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl = new RainbowMage.OverlayPlugin.TabControlExt();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonNewOverlay = new System.Windows.Forms.Button();
            this.buttonRemoveOverlay = new System.Windows.Forms.Button();
            this.checkBoxFollowLog = new System.Windows.Forms.CheckBox();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.TextBox();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.label_ListEmpty = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel0.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel0);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logBox);
            // 
            // tableLayoutPanel0
            // 
            this.tableLayoutPanel0.Controls.Add(this.tabControl, 0, 0);
            this.tableLayoutPanel0.Controls.Add(this.flowLayoutPanel, 0, 1);
            resources.ApplyResources(this.tableLayoutPanel0, "tableLayoutPanel0");
            this.tableLayoutPanel0.Name = "tableLayoutPanel0";
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.buttonNewOverlay);
            this.flowLayoutPanel.Controls.Add(this.buttonRemoveOverlay);
            this.flowLayoutPanel.Controls.Add(this.checkBoxFollowLog);
            this.flowLayoutPanel.Controls.Add(this.buttonClearLog);
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
            // checkBoxFollowLog
            // 
            resources.ApplyResources(this.checkBoxFollowLog, "checkBoxFollowLog");
            this.checkBoxFollowLog.Name = "checkBoxFollowLog";
            this.checkBoxFollowLog.UseVisualStyleBackColor = true;
            this.checkBoxFollowLog.CheckedChanged += new System.EventHandler(this.CheckBoxFollowLog_CheckedChanged);
            // 
            // buttonClearLog
            // 
            resources.ApplyResources(this.buttonClearLog, "buttonClearLog");
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.ButtonClearLog_Click);
            // 
            // logBox
            // 
            this.logBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.logBox, "logBox");
            this.logBox.HideSelection = false;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
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
            // ControlPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ControlPanel";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel0.ResumeLayout(false);
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.tabPageMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private RainbowMage.OverlayPlugin.TabControlExt tabControl;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonRemoveOverlay;
        private System.Windows.Forms.Button buttonNewOverlay;
        private TableLayoutPanel tableLayoutPanel0;
        private FlowLayoutPanel flowLayoutPanel;
        private Label label_ListEmpty;
        private TextBox logBox;
        private CheckBox checkBoxFollowLog;
        private Button buttonClearLog;
    }
}