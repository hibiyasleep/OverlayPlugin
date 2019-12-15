using System;
using System.Collections.Generic;
using CefSharp.OffScreen;
using CefSharp.Structs;
using CefSharp.Enums;
using CefSharp;
using CefSharp.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace RainbowMage.HtmlRenderer
{
    public class Renderer : IDisposable
    {
        const string CRASH_SERVER = "https://sentry.gruenprint.de/api/12/minidump/?sentry_key=5742b37c4b71484e9ba590c6a6a0e137";

        public event EventHandler<BrowserErrorEventArgs> BrowserError;
        public event EventHandler<BrowserLoadEventArgs> BrowserStartLoading;
        public event EventHandler<BrowserLoadEventArgs> BrowserLoad;
        public event EventHandler<BrowserConsoleLogEventArgs> BrowserConsoleLog;

        private ChromiumWebBrowser _browser;
        private OverlayForm _form;
        private object _api;
        private List<String> scriptQueue = new List<string>();
        private string urlToLoad = null;
        
        public string OverlayVersion {
            get { return overlayVersion; }
        }

        public string OverlayName {
          get { return overlayName; }
        }

        private int clickCount;
        private MouseButtonType lastClickButton;
        private DateTime lastClickTime;
        private int lastClickPosX;
        private int lastClickPosY;
        private string overlayVersion;
        private string overlayName;

        public Renderer(string overlayVersion, string overlayName, OverlayForm form, object api)
        {
            this.overlayVersion = overlayVersion;
            this.overlayName = overlayName;
            this._form = form;
            this._api = api;

            InitBrowser();
        }

        public void InitBrowser()
        {
            this._browser = new BrowserWrapper("about:blank", automaticallyCreateBrowser: false, form: _form);
            _browser.RequestHandler = new CustomRequestHandler(this);
            _browser.MenuHandler = new ContextMenuHandler();
            _browser.DragHandler = new DragHandler(_form);
            _browser.BrowserInitialized += _browser_BrowserInitialized;
            _browser.FrameLoadStart += Browser_FrameLoadStart;
            _browser.FrameLoadEnd += Browser_FrameLoadEnd;
            _browser.LoadError += Browser_LoadError;
            _browser.ConsoleMessage += Browser_ConsoleMessage;

            if (_api != null)
            {
                _browser.JavascriptObjectRepository.Register("OverlayPluginApi", _api, isAsync: true);
            }
        }

        private void _browser_BrowserInitialized(object sender, EventArgs e)
        {
            // Make sure we use the correct size for rendering. CEF sometimes ignores the size passed in WindowInfo (see BeginRender()).
            Resize(_form.Width, _form.Height);
        }

        private void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            var initScript = @"(async () => {
                await CefSharp.BindObjectAsync('OverlayPluginApi');
                OverlayPluginApi.overlayName = " + JsonConvert.SerializeObject(this.overlayName) + @";
                OverlayPluginApi.ready = true;
            })();";
            e.Frame.ExecuteJavaScriptAsync(initScript, "init");

            foreach (var item in this.scriptQueue)
            {
                e.Frame.ExecuteJavaScriptAsync(item, "injectOnLoad");
            }
            this.scriptQueue.Clear();

            try
            {
                BrowserStartLoading?.Invoke(this, new BrowserLoadEventArgs(0, e.Url));
            } catch(Exception ex)
            {
                BrowserConsoleLog?.Invoke(this, new BrowserConsoleLogEventArgs(ex.ToString(), "", 1));
            }
        }

        private void Browser_LoadError(object sender, LoadErrorEventArgs e)
        {
            try
            {
                BrowserError?.Invoke(sender, new BrowserErrorEventArgs(e.ErrorCode, e.ErrorText, e.FailedUrl));
            } catch(Exception ex)
            {
                BrowserConsoleLog?.Invoke(this, new BrowserConsoleLogEventArgs(ex.ToString(), "", 1));
            }
        }

        public void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            BrowserConsoleLog?.Invoke(sender, new BrowserConsoleLogEventArgs(e.Message, e.Source, e.Line));
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (urlToLoad != null)
            {
                _browser.Load(urlToLoad);
                urlToLoad = null;
            }

            try
            {
                BrowserLoad?.Invoke(sender, new BrowserLoadEventArgs(e.HttpStatusCode, e.Url));
            } catch(Exception ex)
            {
                BrowserConsoleLog?.Invoke(this, new BrowserConsoleLogEventArgs(ex.ToString(), "", 1));
            }
        }

        public void Load(string url)
        {
            if (this._browser != null && _browser.IsBrowserInitialized)
            {
                this._browser.Load(url);
            } else
            {
                urlToLoad = url;
            }
        }

        public void BeginRender()
        {
            _form.UpdateRender();
        }

        public void BeginRender(int width, int height, string url, int maxFrameRate = 30)
        {
            EndRender();

            var cefWindowInfo = new WindowInfo();
            cefWindowInfo.SetAsWindowless(IntPtr.Zero);
            cefWindowInfo.Width = width;
            cefWindowInfo.Height = height;

            var cefBrowserSettings = new BrowserSettings();
            cefBrowserSettings.WindowlessFrameRate = maxFrameRate;
            _browser.CreateBrowser(cefWindowInfo, cefBrowserSettings);

            cefBrowserSettings.Dispose();
            cefWindowInfo.Dispose();

            urlToLoad = url;
        }

        public void EndRender()
        {
            if (_browser != null && _browser.IsBrowserInitialized)
            {
                _browser.GetBrowser().CloseBrowser(true);
            }
        }

        public void SetMaxFramerate(int fps)
        {
            if (_browser != null && _browser.IsBrowserInitialized)
            {
                _browser.GetBrowserHost().WindowlessFrameRate = fps;
            }
        }

        public void Reload()
        {
            if (this._browser != null && _browser.IsBrowserInitialized)
            {
                this._browser.Reload();
            }
        }

        public void Resize(int width, int height)
        {
            if (this._browser != null)  // && _browser.IsBrowserInitialized)
            {
                this._browser.Size = new System.Drawing.Size(width, height);
            }
        }

        public void SetZoomLevel(double zoom)
        {
            if (this._browser != null && this._browser.IsBrowserInitialized)
            {
                this._browser.SetZoomLevel(zoom);
            }
        }

        public void NotifyMoveStarted()
        {
            if (this._browser != null && this._browser.IsBrowserInitialized)
            {
                this._browser.GetBrowserHost().NotifyMoveOrResizeStarted();
            }
        }

        public void SetVisible(bool visible)
        {
            if (this._browser != null && this._browser.IsBrowserInitialized)
            {
                // Sometimes, CEF stops emitting OnPaint events after this call.
                // Not sure why, probably a timing issue.
                // TODO: Debug this further. Will probably require debugging libcef.dll directly.
                //       Modifying _browser.Size immidiately after the WasHidden() call leads to a crash in libcef.dll,
                //       that might be a good place to start investigating.
                //Task.Run(() => this._browser.GetBrowserHost().WasHidden(!visible));
            }
        }

        public void SendMouseMove(int x, int y, MouseButtonType button)
        {
            if (this._browser != null && this._browser.IsBrowserInitialized)
            {
                var host = this._browser.GetBrowserHost();
                var modifiers = CefEventFlags.None;
                if (button == MouseButtonType.Left)
                {
                    modifiers = CefEventFlags.LeftMouseButton;
                }
                else if (button == MouseButtonType.Middle)
                {
                    modifiers = CefEventFlags.MiddleMouseButton;
                }
                else if (button == MouseButtonType.Right)
                {
                    modifiers = CefEventFlags.RightMouseButton;
                }

                var mouseEvent = new MouseEvent(x, y, modifiers);
                host.SendMouseMoveEvent(mouseEvent, false);
            }
        }

        public void SendMouseLeave()
        {
            if (this._browser != null && this._browser.IsBrowserInitialized)
            {
                this._browser.GetBrowserHost().SendMouseMoveEvent(new MouseEvent(0, 0, 0), true);
            }
        }

        public void SendMouseUpDown(int x, int y, MouseButtonType button, bool isMouseUp)
        {
            if (this._browser != null && this._browser.IsBrowserInitialized)
            {
                var host = this._browser.GetBrowserHost();

                if (!isMouseUp)
                {
                    if (IsContinuousClick(x, y, button))
                    {
                        clickCount++;
                    }
                    else
                    {
                        clickCount = 1;
                    }
                }

                var mouseEvent = new MouseEvent(x, y, CefEventFlags.None);
                host.SendMouseClickEvent(mouseEvent, button, isMouseUp, clickCount);

                lastClickPosX = x;
                lastClickPosY = y;
                lastClickButton = button;
                lastClickTime = DateTime.Now;
            }
        }

        public void SendMouseWheel(int x, int y, int delta, bool isVertical)
        {
            if (this._browser != null && this._browser.IsBrowserInitialized)
            {
                var host = this._browser.GetBrowserHost();
                var mouseEvent = new MouseEvent(x, y, CefEventFlags.None);
                host.SendMouseWheelEvent(mouseEvent, isVertical ? delta : 0, !isVertical ? delta : 0);
            }
        }

        private bool IsContinuousClick(int x, int y, MouseButtonType button)
        {
            // ダブルクリックとして認識するクリックの間隔よりも大きかった場合は継続的なクリックとみなさない
            var delta = (DateTime.Now - lastClickTime).TotalMilliseconds;
            if (delta > System.Windows.Forms.SystemInformation.DoubleClickTime)
            {
                return false;
            }

            // クリック位置が違う、もしくはボタンが違う場合にも継続的なクリックとはみなさない
            if (lastClickPosX != x || lastClickPosY != y || lastClickButton != button)
            {
                return false;
            }

            return true;
        }

        public void SendKeyEvent(KeyEvent keyEvent)
        {
            if (this._browser != null && this._browser.IsBrowserInitialized)
            {
                var host = this._browser.GetBrowserHost();

                host.SendKeyEvent(keyEvent);
            }
        }

        public void showDevTools(bool firstwindow = true)
        {
            if (_browser != null && _browser.IsBrowserInitialized)
            {
                WindowInfo wi = new WindowInfo();
                wi.SetAsPopup(_browser.GetBrowserHost().GetWindowHandle(), "DevTools");
                _browser.GetBrowserHost().ShowDevTools(wi);
            }
        }

        public void Dispose()
        {
            if (this._browser != null)
            {
                this._browser.Dispose();
                this._browser = null;
            }
        }

        static bool initialized = false;

        public static void Initialize(string pluginDirectory, string appDataDirectory, bool reportErrors)
        {
            if (!initialized)
            {
                if (reportErrors)
                {
                    try
                    {
                        // Enable the Crashpad reporter. This *HAS* to happen before libcef.dll is loaded.
                        EnableErrorReports(appDataDirectory);
                    } catch (Exception e)
                    {
                        // TODO: Log this exception.
                    }
                }

                var lang = System.Globalization.CultureInfo.CurrentCulture.Name;
                var langPak = Path.Combine(appDataDirectory, "OverlayPluginCef", Environment.Is64BitProcess ? "x64" : "x86",
                    "locales", lang + ".pak");

                // Fall back to en-US if we can't find the current locale.
                if (!File.Exists(langPak))
                {
                    lang = "en-US";
                }

                var cefSettings = new CefSettings
                {
                    WindowlessRenderingEnabled = true,
                    Locale = lang,
                    CachePath = Path.Combine(appDataDirectory, "OverlayPluginCache"),
                    MultiThreadedMessageLoop = true,
                    LogFile = Path.Combine(appDataDirectory, "OverlayPluginCEF.log"),
#if DEBUG
                    LogSeverity = LogSeverity.Info,
#else
                    LogSeverity = LogSeverity.Error,
#endif
                    BrowserSubprocessPath = Path.Combine(appDataDirectory,
                                           "OverlayPluginCef",
                                           Environment.Is64BitProcess ? "x64" : "x86",
                                           "CefSharp.BrowserSubprocess.exe"),
                };

                // Necessary to avoid input lag with a framerate limit below 60.
                cefSettings.CefCommandLineArgs["enable-begin-frame-scheduling"] = "1";

                // Allow websites to play sound even if the user never interacted with that site (pretty common for our overlays)
                cefSettings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";

                // Disable Flash. We don't need it and it can cause issues.
                cefSettings.CefCommandLineArgs.Remove("enable-system-flash");

                cefSettings.EnableAudio();

                // Enables software compositing instead of GPU compositing -> less CPU load but no WebGL
                cefSettings.SetOffScreenRenderingBestPerformanceArgs();

                Cef.Initialize(cefSettings, performDependencyCheck: true, browserProcessHandler: null);

                initialized = true;
            }
        }

        public static void Shutdown()
        {
            if (initialized)
            {
                Cef.Shutdown();
            }
        }

        public static void EnableErrorReports(string appDataDirectory)
        {
            // Unfortunately, we can't change the path to this configuration file without using our own .exe for
            // all overlays (which would add IPC overhead) or recompiling CEF.
            // For details, see this https://bitbucket.org/chromiumembedded/cef/wiki/CrashReporting.md

            var subProcess = Path.Combine(appDataDirectory,
                                          "OverlayPluginCef",
                                          Environment.Is64BitProcess ? "x64" : "x86");
            var subCfgPath = Path.Combine(subProcess, "crash_reporter.cfg");

            var cfgPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            cfgPath = Path.Combine(cfgPath, "crash_reporter.cfg");

            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            var cfgText = $@"
# This file was generated by OverlayPlugin to configure CEF's Crahspad reporter.
# For details on the purpose of this file see https://bitbucket.org/chromiumembedded/cef/wiki/CrashReporting.md.
# Please don't modify this file manually. Instead, disable it in OverlayPlugin's settings.

[Config]
ProductName=OverlayPlugin
ProductVersion={version}
AppName=ACT_OverlayPlugin
ExternalHandler={subProcess}\CefSharp.BrowserSubprocess.exe
ServerURL={CRASH_SERVER}

# Disable rate limiting so that all crashes are uploaded.
RateLimitEnabled=false
MaxUploadsPerDay=0
";
            File.WriteAllText(cfgPath, cfgText);
            File.WriteAllText(subCfgPath, cfgText);
        }

        public static void DisableErrorReports(string appDataDirectory)
        {
            var subProcess = Path.Combine(appDataDirectory,
                                          "OverlayPluginCef",
                                          Environment.Is64BitProcess ? "x64" : "x86");
            var subCfgPath = Path.Combine(subProcess, "crash_reporter.cfg");

            var cfgPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            cfgPath = Path.Combine(cfgPath, "crash_reporter.cfg");

            if (File.Exists(cfgPath))
                File.Delete(cfgPath);

            if (File.Exists(subCfgPath))
                File.Delete(subCfgPath);
        }

        public void ExecuteScript(string script)
        {
            if (_browser != null && _browser.IsBrowserInitialized)
            {
                _browser.GetMainFrame().ExecuteJavaScriptAsync(script, "injected");
            }
            else
            {
                this.scriptQueue.Add(script);
            }
        }

        // IJavascriptCallback can't be used outside HtmlRenderer. This helper allows other code
        // to invoke callbacks regardless.

        public static void ExecuteCallback(object callback, object param)
        {
            if (callback.GetType().GetInterface("IJavascriptCallback") == null)
            {
                throw new Exception("Invalid parameter passed for callback!");
            }

            var cb = (IJavascriptCallback) callback;
            using (cb)
            {
                if (cb.CanExecute)
                {
                    cb.ExecuteAsync(param);
                }
            }
        }
    }

    public class BrowserErrorEventArgs : EventArgs
    {
        public string ErrorCode { get; private set; }
        public string ErrorText { get; private set; }
        public string Url { get; private set; }
        public BrowserErrorEventArgs(CefErrorCode errorCode, string errorText, string url)
        {
            this.ErrorCode = errorCode.ToString();
            this.ErrorText = errorText;
            this.Url = url;
        }
    }
    
    public class BrowserLoadEventArgs : EventArgs
    {
        public int HttpStatusCode { get; private set; }
        public string Url { get; private set; }
        public BrowserLoadEventArgs(int httpStatusCode, string url)
        {
            this.HttpStatusCode = httpStatusCode;
            this.Url = url;
        }
    }

    public class BrowserConsoleLogEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public string Source { get; private set; }
        public int Line { get; private set; }
        public BrowserConsoleLogEventArgs(string message, string source, int line)
        {
            this.Message = message;
            this.Source = source;
            this.Line = line;
        }
    }

    public class RendererFeatureRequestEventArgs : EventArgs
    {
        public string Request { get; private set; }
        public RendererFeatureRequestEventArgs(string request)
        {
            this.Request = request;
        }
    }

    public class HandlerCallEventArgs : EventArgs
    {
        public JObject Payload { get; private set; }
        public Action<JObject> Callback { get; private set; }
        public HandlerCallEventArgs(JObject payload, Action<JObject> callback)
        {
            this.Payload = payload;
            this.Callback = callback;
        }
    }

    internal class BrowserWrapper : ChromiumWebBrowser, IRenderWebBrowser
    {
        OverlayForm form;

        public BrowserWrapper(string address = "", BrowserSettings browserSettings = null,
            RequestContext requestContext = null, bool automaticallyCreateBrowser = true, OverlayForm form = null) :
            base(address, browserSettings, requestContext, automaticallyCreateBrowser)
        {
            this.form = form;
        }

        void IRenderWebBrowser.OnPaint(PaintElementType type, Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            form.RenderFrame(dirtyRect, buffer, width, height);
        }

        void IRenderWebBrowser.OnCursorChange(IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {
            form.Cursor = new Cursor(cursor);
        }

        bool IRenderWebBrowser.GetScreenPoint(int contentX, int contentY, out int screenX, out int screenY)
        {
            screenX = (int) (contentX + form.Location.X);
            screenY = (int) (contentY + form.Location.Y);

            return true;
        }
    }

    internal class ContextMenuHandler : IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
        }

        public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
        }

        public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            // Suppress the context menu.
            return true;
        }
    }
}
