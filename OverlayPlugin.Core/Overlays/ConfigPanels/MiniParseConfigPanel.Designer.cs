﻿namespace RainbowMage.OverlayPlugin.Overlays
{
    partial class MiniParseConfigPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiniParseConfigPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();

            this.checkMiniParseVisible = new System.Windows.Forms.CheckBox();
            this.checkMiniParseClickthru = new System.Windows.Forms.CheckBox();
            this.checkEnableGlobalHotkey = new System.Windows.Forms.CheckBox();
            this.checkLock = new System.Windows.Forms.CheckBox();

            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();

            this.nudUpdateInterval = new System.Windows.Forms.NumericUpDown();
            this.textMiniParseSortKey = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonMiniParseCopyActXiv = new System.Windows.Forms.Button();
            this.buttonMiniParseOpenDevTools = new System.Windows.Forms.Button();
            this.buttonMiniParseReloadBrowser = new System.Windows.Forms.Button();
            this.textMiniParseUrl = new System.Windows.Forms.TextBox();
            this.buttonMiniParseSelectFile = new System.Windows.Forms.Button();
            this.comboMiniParseSortType = new System.Windows.Forms.ComboBox();
            this.nudMaxFrameRate = new System.Windows.Forms.NumericUpDown();
            this.textGlobalHotkey = new System.Windows.Forms.TextBox();
            this.comboHotkeyType = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateInterval)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFrameRate)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.nudUpdateInterval, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.textMiniParseSortKey, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkMiniParseVisible, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkMiniParseClickthru, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboMiniParseSortType, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.nudMaxFrameRate, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label13, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkLock, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.checkEnableGlobalHotkey, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.textGlobalHotkey, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.comboHotkeyType, 1, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // nudUpdateInterval
            // 
            resources.ApplyResources(this.nudUpdateInterval, "nudUpdateInterval");
            this.nudUpdateInterval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudUpdateInterval.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudUpdateInterval.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudUpdateInterval.Name = "nudUpdateInterval";
            this.nudUpdateInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudUpdateInterval.ValueChanged += new System.EventHandler(this.nudUpdateInterval_ValueChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label11
            // 
            // resources.ApplyResources(this.label11, "label11");
            // this.label11.Name = "label11";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // textMiniParseSortKey
            // 
            resources.ApplyResources(this.textMiniParseSortKey, "textMiniParseSortKey");
            this.textMiniParseSortKey.Name = "textMiniParseSortKey";
            this.textMiniParseSortKey.TextChanged += new System.EventHandler(this.textSortKey_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // checkMiniParseVisible
            // 
            resources.ApplyResources(this.checkMiniParseVisible, "checkMiniParseVisible");
            this.checkMiniParseVisible.Name = "checkMiniParseVisible";
            this.checkMiniParseVisible.UseVisualStyleBackColor = true;
            this.checkMiniParseVisible.CheckedChanged += new System.EventHandler(this.checkWindowVisible_CheckedChanged);
            // 
            // checkMiniParseClickthru
            // 
            resources.ApplyResources(this.checkMiniParseClickthru, "checkMiniParseClickthru");
            this.checkMiniParseClickthru.Name = "checkMiniParseClickthru";
            this.checkMiniParseClickthru.UseVisualStyleBackColor = true;
            this.checkMiniParseClickthru.CheckedChanged += new System.EventHandler(this.checkMouseClickthru_CheckedChanged);
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.buttonMiniParseCopyActXiv, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonMiniParseOpenDevTools, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonMiniParseReloadBrowser, 2, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // buttonMiniParseCopyActXiv
            // 
            resources.ApplyResources(this.buttonMiniParseCopyActXiv, "buttonMiniParseCopyActXiv");
            this.buttonMiniParseCopyActXiv.Name = "buttonMiniParseCopyActXiv";
            this.buttonMiniParseCopyActXiv.UseVisualStyleBackColor = true;
            this.buttonMiniParseCopyActXiv.Click += new System.EventHandler(this.buttonCopyActXiv_Click);
            // 
            // buttonMiniParseOpenDevTools
            // 
            resources.ApplyResources(this.buttonMiniParseOpenDevTools, "buttonMiniParseOpenDevTools");
            this.buttonMiniParseOpenDevTools.Name = "buttonMiniParseOpenDevTools";
            this.buttonMiniParseOpenDevTools.UseVisualStyleBackColor = true;
            this.buttonMiniParseOpenDevTools.Click += new System.EventHandler(this.buttonMiniParseOpenDevTools_Click);
            this.buttonMiniParseOpenDevTools.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonMiniParseOpenDevTools_RClick);
            // 
            // buttonMiniParseReloadBrowser
            // 
            resources.ApplyResources(this.buttonMiniParseReloadBrowser, "buttonMiniParseReloadBrowser");
            this.buttonMiniParseReloadBrowser.Name = "buttonMiniParseReloadBrowser";
            this.buttonMiniParseReloadBrowser.UseVisualStyleBackColor = true;
            this.buttonMiniParseReloadBrowser.Click += new System.EventHandler(this.buttonReloadBrowser_Click);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.textMiniParseUrl, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonMiniParseSelectFile, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // textMiniParseUrl
            // 
            resources.ApplyResources(this.textMiniParseUrl, "textMiniParseUrl");
            this.textMiniParseUrl.Name = "textMiniParseUrl";
            this.textMiniParseUrl.TextChanged += new System.EventHandler(this.textUrl_TextChanged);
            this.textMiniParseUrl.Leave += new System.EventHandler(this.textMiniParseUrl_Leave);
            // 
            // buttonMiniParseSelectFile
            // 
            this.buttonMiniParseSelectFile.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.buttonMiniParseSelectFile, "buttonMiniParseSelectFile");
            this.buttonMiniParseSelectFile.Name = "buttonMiniParseSelectFile";
            this.buttonMiniParseSelectFile.UseVisualStyleBackColor = true;
            this.buttonMiniParseSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // comboMiniParseSortType
            // 
            resources.ApplyResources(this.comboMiniParseSortType, "comboMiniParseSortType");
            this.comboMiniParseSortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMiniParseSortType.FormattingEnabled = true;
            this.comboMiniParseSortType.Name = "comboMiniParseSortType";
            // 
            // nudMaxFrameRate
            // 
            resources.ApplyResources(this.nudMaxFrameRate, "nudMaxFrameRate");
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
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
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
            this.textGlobalHotkey.Name = "textGlobalHotkey";
            this.textGlobalHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxGlobalHotkey_KeyDown);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // checkLock
            // 
            resources.ApplyResources(this.checkLock, "checkLock");
            this.checkLock.Name = "checkLock";
            this.checkLock.UseVisualStyleBackColor = true;
            this.checkLock.CheckedChanged += new System.EventHandler(this.checkLock_CheckedChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // comboHotkeyType
            // 
            resources.ApplyResources(this.comboHotkeyType, "comboHotkeyType");
            this.comboHotkeyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHotkeyType.FormattingEnabled = true;
            this.comboHotkeyType.Name = "comboHotkeyType";
            // 
            // MiniParseConfigPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MiniParseConfigPanel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateInterval)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFrameRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textMiniParseSortKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkMiniParseVisible;
        private System.Windows.Forms.CheckBox checkMiniParseClickthru;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonMiniParseReloadBrowser;
        private System.Windows.Forms.Button buttonMiniParseOpenDevTools;
        private System.Windows.Forms.Button buttonMiniParseCopyActXiv;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textMiniParseUrl;
        private System.Windows.Forms.Button buttonMiniParseSelectFile;
        private System.Windows.Forms.ComboBox comboMiniParseSortType;
        private System.Windows.Forms.NumericUpDown nudMaxFrameRate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkEnableGlobalHotkey;
        private System.Windows.Forms.TextBox textGlobalHotkey;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkLock;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboHotkeyType;
        private System.Windows.Forms.NumericUpDown nudUpdateInterval;
        private System.Windows.Forms.Label label12;
    }
}
