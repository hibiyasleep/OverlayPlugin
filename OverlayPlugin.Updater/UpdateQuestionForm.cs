using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Markdig;

namespace RainbowMage.OverlayPlugin.Updater
{
    public partial class UpdateQuestionForm : Form
    {
        private static string MarkdownStyle = @"<style>
body {
    font-family: Segoe UI, Helvetica, Arial;
    font-size: 16px;
    color: #24292e;
    line-height: 1.5;
}

a {
    color: #0366d6;
    text-decoration: none;
}
a:hover { text-decoration: underline; }

code {
    font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, monospace;
    padding: 0.2em 0.4em;
    margin: 0;
    font-size: 85%;
    background-color: #f3f4f4;
    border-radius: 3px;
}
</style>";

        public UpdateQuestionForm(string description)
        {
            InitializeComponent();

            webBrowser1.DocumentText = MarkdownStyle + Markdown.ToHtml(description);
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString() != "about:blank")
            {
                Process.Start(e.Url.ToString());
                e.Cancel = true;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnDeny_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}
