﻿using Xilium.CefGlue;

namespace RainbowMage.HtmlRenderer
{
    class LifeSpanHandler : CefLifeSpanHandler
    {
        private readonly Renderer renderer;

        public LifeSpanHandler(Renderer renderer)
        {
            this.renderer = renderer;
        }
        
        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);

            this.renderer.OnCreated(browser);
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
            base.OnBeforeClose(browser);

            this.renderer.OnBeforeClose(browser);
        }
    }
}
