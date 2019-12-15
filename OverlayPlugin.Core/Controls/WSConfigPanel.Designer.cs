namespace RainbowMage.OverlayPlugin
{
    partial class WSConfigPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WSConfigPanel));
            this.ipTxt = new System.Windows.Forms.TextBox();
            this.portTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.genSslBtn = new System.Windows.Forms.Button();
            this.sslBox = new System.Windows.Forms.CheckBox();
            this.logDisplay = new System.Windows.Forms.TextBox();
            this.startBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbOverlay = new System.Windows.Forms.ComboBox();
            this.lnkOverlayUrl = new System.Windows.Forms.LinkLabel();
            this.lblUrlConfidentWarning = new System.Windows.Forms.Label();
            this.ctxMenuLink = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuLinkCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuLink.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipTxt
            // 
            resources.ApplyResources(this.ipTxt, "ipTxt");
            this.ipTxt.Name = "ipTxt";
            this.ipTxt.Leave += new System.EventHandler(this.ipTxt_Leave);
            // 
            // portTxt
            // 
            resources.ApplyResources(this.portTxt, "portTxt");
            this.portTxt.Name = "portTxt";
            this.portTxt.Leave += new System.EventHandler(this.portTxt_Leave);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // genSslBtn
            // 
            resources.ApplyResources(this.genSslBtn, "genSslBtn");
            this.genSslBtn.Name = "genSslBtn";
            this.genSslBtn.UseVisualStyleBackColor = true;
            this.genSslBtn.Click += new System.EventHandler(this.genSslBtn_Click);
            // 
            // sslBox
            // 
            resources.ApplyResources(this.sslBox, "sslBox");
            this.sslBox.Name = "sslBox";
            this.sslBox.UseVisualStyleBackColor = true;
            this.sslBox.CheckedChanged += new System.EventHandler(this.sslBox_CheckedChanged);
            // 
            // logDisplay
            // 
            resources.ApplyResources(this.logDisplay, "logDisplay");
            this.logDisplay.BackColor = System.Drawing.SystemColors.Control;
            this.logDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logDisplay.Name = "logDisplay";
            this.logDisplay.ReadOnly = true;
            // 
            // startBtn
            // 
            resources.ApplyResources(this.startBtn, "startBtn");
            this.startBtn.Name = "startBtn";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // stopBtn
            // 
            resources.ApplyResources(this.stopBtn, "stopBtn");
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // statusLabel
            // 
            resources.ApplyResources(this.statusLabel, "statusLabel");
            this.statusLabel.Name = "statusLabel";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbOverlay
            // 
            this.cbOverlay.DisplayMember = "label";
            this.cbOverlay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOverlay.FormattingEnabled = true;
            resources.ApplyResources(this.cbOverlay, "cbOverlay");
            this.cbOverlay.Name = "cbOverlay";
            this.cbOverlay.ValueMember = "overlay";
            this.cbOverlay.SelectedIndexChanged += new System.EventHandler(this.cbOverlay_SelectedIndexChanged);
            // 
            // lnkOverlayUrl
            // 
            resources.ApplyResources(this.lnkOverlayUrl, "lnkOverlayUrl");
            this.lnkOverlayUrl.ContextMenuStrip = this.ctxMenuLink;
            this.lnkOverlayUrl.Name = "lnkOverlayUrl";
            this.lnkOverlayUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOverlayUrl_LinkClicked);
            // 
            // lblUrlConfidentWarning
            // 
            resources.ApplyResources(this.lblUrlConfidentWarning, "lblUrlConfidentWarning");
            this.lblUrlConfidentWarning.Name = "lblUrlConfidentWarning";
            // 
            // ctxMenuLink
            // 
            this.ctxMenuLink.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuLinkCopy});
            this.ctxMenuLink.Name = "ctxMenuLink";
            resources.ApplyResources(this.ctxMenuLink, "ctxMenuLink");
            // 
            // ctxMenuLinkCopy
            // 
            this.ctxMenuLinkCopy.Name = "ctxMenuLinkCopy";
            resources.ApplyResources(this.ctxMenuLinkCopy, "ctxMenuLinkCopy");
            // 
            // WSConfigPanel
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.lblUrlConfidentWarning);
            this.Controls.Add(this.lnkOverlayUrl);
            this.Controls.Add(this.cbOverlay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.logDisplay);
            this.Controls.Add(this.sslBox);
            this.Controls.Add(this.genSslBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portTxt);
            this.Controls.Add(this.ipTxt);
            this.Name = "WSConfigPanel";
            resources.ApplyResources(this, "$this");
            this.ctxMenuLink.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipTxt;
        private System.Windows.Forms.TextBox portTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button genSslBtn;
        private System.Windows.Forms.CheckBox sslBox;
        private System.Windows.Forms.TextBox logDisplay;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbOverlay;
        private System.Windows.Forms.LinkLabel lnkOverlayUrl;
        private System.Windows.Forms.Label lblUrlConfidentWarning;
        private System.Windows.Forms.ContextMenuStrip ctxMenuLink;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuLinkCopy;
    }
}
