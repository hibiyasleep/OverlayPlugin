namespace RainbowMage.OverlayPlugin.Overlays
{
    partial class MiniParseExtConfigPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiniParseExtConfigPanel));
            this.label6 = new System.Windows.Forms.Label();
            this.checkMiniParseExtVisible = new System.Windows.Forms.CheckBox();
            this.checkMiniParseExtClickthru = new System.Windows.Forms.CheckBox();
            this.buttonMiniParseExtReloadBrowser = new System.Windows.Forms.Button();
            this.textMiniParseExtUrl = new System.Windows.Forms.TextBox();
            this.nudMaxFrameRate = new System.Windows.Forms.NumericUpDown();
            this.checkEnableGlobalHotkey = new System.Windows.Forms.CheckBox();
            this.textGlobalHotkey = new System.Windows.Forms.TextBox();
            this.checkLock = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.buttonMiniParseSelectFile = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.developerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDevToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyTheUpdateVariableToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboMiniParseExtSortType = new System.Windows.Forms.ComboBox();
            this.textMiniParseExtSortKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFrameRate)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // checkMiniParseExtVisible
            // 
            resources.ApplyResources(this.checkMiniParseExtVisible, "checkMiniParseExtVisible");
            this.checkMiniParseExtVisible.BackColor = System.Drawing.SystemColors.Control;
            this.checkMiniParseExtVisible.Name = "checkMiniParseExtVisible";
            this.checkMiniParseExtVisible.UseVisualStyleBackColor = false;
            this.checkMiniParseExtVisible.CheckedChanged += new System.EventHandler(this.checkWindowVisible_CheckedChanged);
            // 
            // checkMiniParseExtClickthru
            // 
            resources.ApplyResources(this.checkMiniParseExtClickthru, "checkMiniParseExtClickthru");
            this.checkMiniParseExtClickthru.BackColor = System.Drawing.SystemColors.Control;
            this.checkMiniParseExtClickthru.Name = "checkMiniParseExtClickthru";
            this.checkMiniParseExtClickthru.UseVisualStyleBackColor = false;
            this.checkMiniParseExtClickthru.CheckedChanged += new System.EventHandler(this.checkMouseClickthru_CheckedChanged);
            // 
            // buttonMiniParseExtReloadBrowser
            // 
            resources.ApplyResources(this.buttonMiniParseExtReloadBrowser, "buttonMiniParseExtReloadBrowser");
            this.buttonMiniParseExtReloadBrowser.Name = "buttonMiniParseExtReloadBrowser";
            this.buttonMiniParseExtReloadBrowser.UseVisualStyleBackColor = true;
            this.buttonMiniParseExtReloadBrowser.Click += new System.EventHandler(this.buttonReloadBrowser_Click);
            // 
            // textMiniParseExtUrl
            // 
            resources.ApplyResources(this.textMiniParseExtUrl, "textMiniParseExtUrl");
            this.textMiniParseExtUrl.Name = "textMiniParseExtUrl";
            this.textMiniParseExtUrl.TextChanged += new System.EventHandler(this.textUrl_TextChanged);
            this.textMiniParseExtUrl.Leave += new System.EventHandler(this.textMiniParseExtUrl_Leave);
            // 
            // nudMaxFrameRate
            // 
            resources.ApplyResources(this.nudMaxFrameRate, "nudMaxFrameRate");
            this.nudMaxFrameRate.BackColor = System.Drawing.SystemColors.Control;
            this.nudMaxFrameRate.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudMaxFrameRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxFrameRate.Name = "nudMaxFrameRate";
            this.nudMaxFrameRate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxFrameRate.ValueChanged += new System.EventHandler(this.nudMaxFrameRate_ValueChanged);
            // 
            // checkEnableGlobalHotkey
            // 
            resources.ApplyResources(this.checkEnableGlobalHotkey, "checkEnableGlobalHotkey");
            this.checkEnableGlobalHotkey.Name = "checkEnableGlobalHotkey";
            this.checkEnableGlobalHotkey.UseVisualStyleBackColor = true;
            this.checkEnableGlobalHotkey.CheckedChanged += new System.EventHandler(this.checkBoxEnableGlobalHotkey_CheckedChanged);
            // 
            // textGlobalHotkey
            // 
            resources.ApplyResources(this.textGlobalHotkey, "textGlobalHotkey");
            this.textGlobalHotkey.BackColor = System.Drawing.SystemColors.Control;
            this.textGlobalHotkey.Name = "textGlobalHotkey";
            this.textGlobalHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxGlobalHotkey_KeyDown);
            // 
            // checkLock
            // 
            resources.ApplyResources(this.checkLock, "checkLock");
            this.checkLock.Name = "checkLock";
            this.checkLock.UseVisualStyleBackColor = true;
            this.checkLock.CheckedChanged += new System.EventHandler(this.checkLock_CheckedChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.buttonMiniParseExtReloadBrowser);
            this.groupBox1.Controls.Add(this.checkMiniParseExtVisible);
            this.groupBox1.Controls.Add(this.checkMiniParseExtClickthru);
            this.groupBox1.Controls.Add(this.checkLock);
            this.groupBox1.Controls.Add(this.nudMaxFrameRate);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.label6);
            this.panel2.Name = "panel2";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.buttonMiniParseSelectFile);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Controls.Add(this.textMiniParseExtUrl);
            this.panel4.Name = "panel4";
            // 
            // buttonMiniParseSelectFile
            // 
            resources.ApplyResources(this.buttonMiniParseSelectFile, "buttonMiniParseSelectFile");
            this.buttonMiniParseSelectFile.Name = "buttonMiniParseSelectFile";
            this.buttonMiniParseSelectFile.UseVisualStyleBackColor = true;
            this.buttonMiniParseSelectFile.Click += new System.EventHandler(this.buttonMiniParseSelectFile_Click);
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.checkEnableGlobalHotkey);
            this.groupBox5.Controls.Add(this.textGlobalHotkey);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Name = "panel1";
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.developerToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // developerToolStripMenuItem
            // 
            resources.ApplyResources(this.developerToolStripMenuItem, "developerToolStripMenuItem");
            this.developerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDevToolsToolStripMenuItem,
            this.copyTheUpdateVariableToClipboardToolStripMenuItem});
            this.developerToolStripMenuItem.Name = "developerToolStripMenuItem";
            // 
            // openDevToolsToolStripMenuItem
            // 
            resources.ApplyResources(this.openDevToolsToolStripMenuItem, "openDevToolsToolStripMenuItem");
            this.openDevToolsToolStripMenuItem.Name = "openDevToolsToolStripMenuItem";
            this.openDevToolsToolStripMenuItem.Click += new System.EventHandler(this.buttonMiniParseExtOpenDevTools_Click);
            // 
            // copyTheUpdateVariableToClipboardToolStripMenuItem
            // 
            resources.ApplyResources(this.copyTheUpdateVariableToClipboardToolStripMenuItem, "copyTheUpdateVariableToClipboardToolStripMenuItem");
            this.copyTheUpdateVariableToClipboardToolStripMenuItem.Name = "copyTheUpdateVariableToClipboardToolStripMenuItem";
            this.copyTheUpdateVariableToClipboardToolStripMenuItem.Click += new System.EventHandler(this.buttonCopyActXiv_Click);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.comboMiniParseExtSortType);
            this.panel3.Controls.Add(this.textMiniParseExtSortKey);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Name = "panel3";
            // 
            // comboMiniParseExtSortType
            // 
            resources.ApplyResources(this.comboMiniParseExtSortType, "comboMiniParseExtSortType");
            this.comboMiniParseExtSortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMiniParseExtSortType.FormattingEnabled = true;
            this.comboMiniParseExtSortType.Name = "comboMiniParseExtSortType";
            // 
            // textMiniParseExtSortKey
            // 
            resources.ApplyResources(this.textMiniParseExtSortKey, "textMiniParseExtSortKey");
            this.textMiniParseExtSortKey.Name = "textMiniParseExtSortKey";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // MiniParseExtConfigPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MiniParseExtConfigPanel";
            this.Load += new System.EventHandler(this.MiniParseExtConfigPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFrameRate)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkMiniParseExtVisible;
        private System.Windows.Forms.CheckBox checkMiniParseExtClickthru;
        private System.Windows.Forms.Button buttonMiniParseExtReloadBrowser;
        private System.Windows.Forms.TextBox textMiniParseExtUrl;
        private System.Windows.Forms.NumericUpDown nudMaxFrameRate;
        private System.Windows.Forms.CheckBox checkEnableGlobalHotkey;
        private System.Windows.Forms.TextBox textGlobalHotkey;
        private System.Windows.Forms.CheckBox checkLock;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button buttonMiniParseSelectFile;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem developerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDevToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyTheUpdateVariableToClipboardToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textMiniParseExtSortKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboMiniParseExtSortType;
    }
}
