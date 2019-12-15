using RainbowMage.OverlayPlugin;
using System.Windows.Forms;

namespace AddonExample
{
    public class AddonExampleOverlay : OverlayBase<AddonExampleOverlayConfig>
    {
        public AddonExampleOverlay(AddonExampleOverlayConfig config, string name) : base(config, name)
        {

        }

        public override Control CreateConfigControl()
        {
            return new AddonExampleOverlayConfigPanel();
        }

        public override void Start()
        {
            // Start the embedded timer when using it.
            // Call base.Start() or timer.Start() to start the embedded timer manually.
            base.Start();
        }

        public override void Stop()
        {
            // Stop the embedded timer when using it.
            // Call base.Stop() or timer.Stop() to stop the embedded timer manually.
            base.Stop();
        }

        protected override void Update()
        {

        }
    }
}
