namespace RainbowMage.OverlayPlugin
{
    partial class GeneralConfigTab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralConfigTab));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbErrorReports = new System.Windows.Forms.CheckBox();
            this.cbUpdateCheck = new System.Windows.Forms.CheckBox();
            this.btnUpdateCheck = new System.Windows.Forms.Button();
            this.cbHideOverlaysWhenNotActive = new System.Windows.Forms.CheckBox();
            this.cbHideOverlaysDuringCutscene = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.cbErrorReports, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbUpdateCheck, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnUpdateCheck, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cbHideOverlaysWhenNotActive, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbHideOverlaysDuringCutscene, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // cbErrorReports
            // 
            resources.ApplyResources(this.cbErrorReports, "cbErrorReports");
            this.cbErrorReports.Name = "cbErrorReports";
            this.cbErrorReports.UseVisualStyleBackColor = true;
            // 
            // cbUpdateCheck
            // 
            resources.ApplyResources(this.cbUpdateCheck, "cbUpdateCheck");
            this.cbUpdateCheck.Name = "cbUpdateCheck";
            this.cbUpdateCheck.UseVisualStyleBackColor = true;
            // 
            // btnUpdateCheck
            // 
            resources.ApplyResources(this.btnUpdateCheck, "btnUpdateCheck");
            this.btnUpdateCheck.Name = "btnUpdateCheck";
            this.btnUpdateCheck.UseVisualStyleBackColor = true;
            this.btnUpdateCheck.Click += new System.EventHandler(this.BtnUpdateCheck_Click);
            // 
            // cbHideOverlaysWhenNotActive
            // 
            resources.ApplyResources(this.cbHideOverlaysWhenNotActive, "cbHideOverlaysWhenNotActive");
            this.cbHideOverlaysWhenNotActive.Name = "cbHideOverlaysWhenNotActive";
            this.cbHideOverlaysWhenNotActive.UseVisualStyleBackColor = true;
            // 
            // cbHideOverlaysDuringCutscene
            // 
            resources.ApplyResources(this.cbHideOverlaysDuringCutscene, "cbHideOverlaysDuringCutscene");
            this.cbHideOverlaysDuringCutscene.Name = "cbHideOverlaysDuringCutscene";
            this.cbHideOverlaysDuringCutscene.UseVisualStyleBackColor = true;
            // 
            // GeneralConfigTab
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GeneralConfigTab";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox cbErrorReports;
        private System.Windows.Forms.CheckBox cbUpdateCheck;
        private System.Windows.Forms.Button btnUpdateCheck;
        private System.Windows.Forms.CheckBox cbHideOverlaysWhenNotActive;
        private System.Windows.Forms.CheckBox cbHideOverlaysDuringCutscene;
    }
}
