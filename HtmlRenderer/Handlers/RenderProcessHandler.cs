using Xilium.CefGlue;

namespace RainbowMage.HtmlRenderer
{
    class RenderProcessHandler : CefRenderProcessHandler
    {
        private readonly BuiltinFunctionHandler builtinFunctionHandler;

        public RenderProcessHandler()
        {
            this.builtinFunctionHandler = new BuiltinFunctionHandler();
            this.builtinFunctionHandler.BroadcastMessage += (o, e) =>
            {
                Renderer.OnBroadcastMessage(o, e);
            };
            this.builtinFunctionHandler.SendMessage += (o, e) =>
            {
                Renderer.OnSendMessage(o, e);
            };
            this.builtinFunctionHandler.OverlayMessage += (o, e) =>
            {
                Renderer.OnOverlayMessage(o, e);
            };

            this.builtinFunctionHandler.EndEncounter += EndEncounterEvent;
            this.builtinFunctionHandler.TakeScreenshot += TakeScreenshotEvent;
        }

        private void EndEncounterEvent(object sender, EndEncounterEventArgs e)
        {
            Renderer.OnRendererFeatureRequest(sender, new RendererFeatureRequestEventArgs("EndEncounter"));
        }

        private void TakeScreenshotEvent(object sender, TakeScreenshotEventArgs e)
        {
            Renderer.OnTakeScreenshot(sender, e);
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            if (message.Name == "SetOverlayAPI")
            {
                // 対象のフレームを取得
                var frameName = message.Arguments.GetString(0);
                var frame = GetFrameByName(browser, frameName);
                var overlayName = message.Arguments.GetString(1);
                var overlayVersion = message.Arguments.GetString(2);

                // API を設定
                if (frame != null && frame.V8Context.Enter())
                {
                    var apiObject = CefV8Value.CreateObject(null);

                    var takeScreenshotFunction = CefV8Value.CreateFunction(
                         BuiltinFunctionHandler.TakeScreenshotFunctionName,
                         builtinFunctionHandler);

                    var broadcastMessageFunction = CefV8Value.CreateFunction(
                        BuiltinFunctionHandler.BroadcastMessageFunctionName,
                        builtinFunctionHandler); 
                    var sendMessageFunction = CefV8Value.CreateFunction(
                         BuiltinFunctionHandler.SendMessageFunctionName,
                         builtinFunctionHandler);
                    var overlayMessageFunction = CefV8Value.CreateFunction(
                         BuiltinFunctionHandler.OverlayMessageFunctionName,
                         builtinFunctionHandler);
                    var endEncounterFunction = CefV8Value.CreateFunction(
                         BuiltinFunctionHandler.EndEncounterFunctionName,
                         builtinFunctionHandler);

                    apiObject.SetValue("version", CefV8Value.CreateString(overlayVersion), CefV8PropertyAttribute.ReadOnly);
                    apiObject.SetValue("overlayName", CefV8Value.CreateString(overlayName), CefV8PropertyAttribute.ReadOnly);

                    apiObject.SetValue(
                        BuiltinFunctionHandler.TakeScreenshotFunctionName,
                        takeScreenshotFunction,
                        CefV8PropertyAttribute.ReadOnly);

                    apiObject.SetValue(
                        BuiltinFunctionHandler.BroadcastMessageFunctionName,
                        broadcastMessageFunction,
                        CefV8PropertyAttribute.ReadOnly);
                    apiObject.SetValue(
                        BuiltinFunctionHandler.SendMessageFunctionName,
                        sendMessageFunction,
                        CefV8PropertyAttribute.ReadOnly);
                    apiObject.SetValue(
                        BuiltinFunctionHandler.OverlayMessageFunctionName,
                        overlayMessageFunction,
                        CefV8PropertyAttribute.ReadOnly);
                    apiObject.SetValue(
                        BuiltinFunctionHandler.EndEncounterFunctionName,
                        endEncounterFunction,
                        CefV8PropertyAttribute.ReadOnly);

                    frame.V8Context.GetGlobal().SetValue("OverlayPluginApi", apiObject, CefV8PropertyAttribute.ReadOnly);

                    frame.V8Context.Exit();
                }
                return true;
            }

            return base.OnProcessMessageReceived(browser, sourceProcess, message);
        }

        private CefFrame GetFrameByName(CefBrowser browser, string frameName)
        {
            CefFrame frame = null;
            if (frameName == null)
            {
                frame = browser.GetMainFrame();
            }
            else
            {
                frame = browser.GetFrame(frameName);
            }

            return frame;
        }
    }
}
