namespace LineEndingFixer
{
    partial class frmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.rule = new System.Windows.Forms.ComboBox();
            this.path = new System.Windows.Forms.TextBox();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.encoding = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(691, 13);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 83);
            this.button1.TabIndex = 0;
            this.button1.Text = "Execute";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ExecuteButtonClick);
            // 
            // rule
            // 
            this.rule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rule.FormattingEnabled = true;
            this.rule.Items.AddRange(new object[] {
            "CR LF (Windows)",
            "LF (Unix)"});
            this.rule.Location = new System.Drawing.Point(119, 13);
            this.rule.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rule.Name = "rule";
            this.rule.Size = new System.Drawing.Size(566, 24);
            this.rule.TabIndex = 1;
            // 
            // path
            // 
            this.path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.path.Location = new System.Drawing.Point(119, 73);
            this.path.Name = "path";
            this.path.Size = new System.Drawing.Size(566, 23);
            this.path.TabIndex = 2;
            this.path.Text = "..\\..\\";
            // 
            // logs
            // 
            this.logs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.logs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logs.Location = new System.Drawing.Point(12, 103);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(754, 286);
            this.logs.TabIndex = 3;
            this.logs.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Line Ending Rule";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "TargetPath";
            // 
            // encoding
            // 
            this.encoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.encoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encoding.FormattingEnabled = true;
            this.encoding.Items.AddRange(new object[] {
            "UTF-8",
            "Ascii",
            "Default"});
            this.encoding.Location = new System.Drawing.Point(119, 43);
            this.encoding.Name = "encoding";
            this.encoding.Size = new System.Drawing.Size(566, 24);
            this.encoding.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Encoding";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 401);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.encoding);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.path);
            this.Controls.Add(this.rule);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft NeoGothic", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMain";
            this.Text = "Tool";
            this.Load += new System.EventHandler(this.FormLoadEvent);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox path;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox encoding;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox rule;
    }
}

