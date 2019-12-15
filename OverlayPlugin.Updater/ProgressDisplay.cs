using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin.Updater
{
    public partial class ProgressDisplay : Form
    {
        CancellationTokenSource _cancel;

        public ProgressDisplay()
        {
            InitializeComponent();

            progressBar.Maximum = 1000;
            cancelBtn.Enabled = false;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();

                if (_cancel != null)
                    _cancel.Dispose();
            }
            base.Dispose(disposing);
        }

        public void UpdateStatus(double percent, string msg)
        {
            progressBar.Value = Math.Min(1000, (int) Math.Round(percent * 1000));
            label.Text = msg;
        }

        public void Log(string text)
        {
            logBox.AppendText(text + "\r\n");
        }

        public CancellationToken GetCancelToken()
        {
            if (_cancel == null)
                _cancel = new CancellationTokenSource();

            cancelBtn.Enabled = true;
            return _cancel.Token;
        }

        public void DisposeCancelSource()
        {
            if (_cancel != null) _cancel.Dispose();

            _cancel = null;
            cancelBtn.Enabled = false;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            _cancel.Cancel();
            cancelBtn.Enabled = false;
        }
    }
}

