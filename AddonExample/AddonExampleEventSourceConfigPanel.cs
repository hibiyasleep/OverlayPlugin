using System;
using System.Windows.Forms;

namespace AddonExample
{
    public partial class AddonExampleEventSourceConfigPanel : UserControl
    {
        private AddonExampleEventSourceConfig config;
        private AddonExampleEventSource source;
        public AddonExampleEventSourceConfigPanel(AddonExampleEventSource source)
        {
            InitializeComponent();

            this.source = source;
            this.config = source.Config;

            SetupControlProperties();
            SetupConfigEventHandlers();
        }

        private void SetupControlProperties()
        {
            this.textBox_ExampleString.Text = config.ExampleString;
        }

        private void SetupConfigEventHandlers()
        {
        }

        private void InvokeIfRequired(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void textBox_ExampleString_TextChanged(object sender, EventArgs e)
        {
            this.config.ExampleString = this.textBox_ExampleString.Text;
        }
    }
}
