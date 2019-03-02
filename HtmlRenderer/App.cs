using Xilium.CefGlue;

namespace RainbowMage.HtmlRenderer
{
    public class App : CefApp
    {
        private readonly RenderProcessHandler renderProcessHandler;

        public App()
        {
            this.renderProcessHandler = new RenderProcessHandler();
        }

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return this.renderProcessHandler;
        }
    }
}
