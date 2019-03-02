﻿using Xilium.CefGlue;

namespace RainbowMage.HtmlRenderer
{
    class LoadHandler : CefLoadHandler
    {
        private Renderer renderer;

        public LoadHandler(Renderer renderer)
        {
            this.renderer = renderer;
        }

        protected override void OnLoadStart(CefBrowser browser, CefFrame frame)
        {
            base.OnLoadStart(browser, frame);

            var message = CefProcessMessage.Create("SetOverlayAPI");
            message.Arguments.SetString(0, frame.Name);
            message.Arguments.SetString(1, this.renderer.OverlayName);
            message.Arguments.SetString(2, this.renderer.OverlayVersion);
            browser.SendProcessMessage(CefProcessId.Renderer, message);
        }

        protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
        {
            base.OnLoadError(browser, frame, errorCode, errorText, failedUrl);

            this.renderer.OnError(errorCode, errorText, failedUrl);
        }

        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            base.OnLoadEnd(browser, frame, httpStatusCode);

            this.renderer.OnLoad(browser, frame, httpStatusCode);
        }
    }
}
