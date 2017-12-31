using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xilium.CefGlue;

namespace RainbowMage.HtmlRenderer
{
    public class Renderer : IDisposable
    {
        public event EventHandler<RenderEventArgs> Render;
        public event EventHandler<BrowserErrorEventArgs> BrowserError;
        public event EventHandler<BrowserLoadEventArgs> BrowserLoad;
        public event EventHandler<BrowserConsoleLogEventArgs> BrowserConsoleLog;

        public static event EventHandler<BroadcastMessageEventArgs> BroadcastMessage;
        public static event EventHandler<SendMessageEventArgs> SendMessage;
        public static event EventHandler<SendMessageEventArgs> OverlayMessage;
        public static event EventHandler<RendererFeatureRequestEventArgs> RendererFeatureRequest;

        // Guards access to |allBrowsers| across threads.
        private System.Threading.SemaphoreSlim allBrowsersSemaphore = new System.Threading.SemaphoreSlim(1);
        // Set to empty on the main thread, and to non-empty on the Cef thread. When an addon opens a window
        // via javascript it is added to this list on the Cef thread, and it is removed there when the window
        // is closed. The first browser in this list is stable and can be accessed from the main thread, since
        // it will only be destroyed/removed on the main thread. Any other browser in the list can only be
        // accessed while the |allBrowsersSemaphore| semaphore is held, so we do not provide a way to read
        // or copy this list directly to clients.
        private List<CefBrowser> allBrowsers = new List<CefBrowser>();

        // When a navigation occurs, the Browser is destroyed and recreated. The Browser becomes
        // null immediately on the addon main thread, and will become non-null later on the Cef
        // thread. Once it becomes non-null, the pointer will not change until another
        // navigation.
        public CefBrowser Browser
        {
            get
            {
                allBrowsersSemaphore.Wait();
                CefBrowser b = this.allBrowsers.FirstOrDefault();
                allBrowsersSemaphore.Release();
                return b;
            }
        }

        public string OverlayVersion
        {
            get { return overlayVersion; }
        }

        public string OverlayName
        {
            get { return overlayName; }
        }

        private Client Client { get; set; }

        private int clickCount;
        private CefMouseButtonType lastClickButton;
        private DateTime lastClickTime;
        private int lastClickPosX;
        private int lastClickPosY;
        private string overlayVersion;
        private string overlayName;

        public Renderer(string overlayVersion, string overlayName)
        {
            this.overlayVersion = overlayVersion;
            this.overlayName = overlayName;
        }

        public void BeginRender(int width, int height, string url, int maxFrameRate = 30)
        {
            EndRender();

            var cefWindowInfo = CefWindowInfo.Create();
            cefWindowInfo.SetAsWindowless(IntPtr.Zero, true);

            var cefBrowserSettings = new CefBrowserSettings();
            cefBrowserSettings.WindowlessFrameRate = maxFrameRate;

            this.Client = new Client(this, width, height);

            CefBrowserHost.CreateBrowser(
                cefWindowInfo,
                this.Client,
                cefBrowserSettings,
                url);
        }

        public void EndRender()
        {
            // We hold the semaphore and clear |this.allBrowsers| to ensure the first browser in the
            // list is gone immediately on the main thread. It may later be added on the Cef thread.
            this.allBrowsersSemaphore.Wait();
            if (this.allBrowsers.Count > 0)
            {
                foreach (var browser in this.allBrowsers)
                {
                    var host = browser.GetHost();
                    if (host != null)
                    {
                        host.CloseBrowser(true);
                        host.Dispose();
                    }
                    browser.Dispose();
                }
                this.allBrowsers.Clear();
            }
            this.allBrowsersSemaphore.Release();
        }

        public void Reload()
        {
            if (this.Browser != null)
            {
                this.Browser.Reload();
            }
        }

        public void Resize(int width, int height)
        {
            if (this.Client != null && this.Browser != null)
            {
                this.Client.ResizeView(width, height);
                this.Browser.GetHost().WasResized();
            }
        }

        public void SendMouseMove(int x, int y, CefMouseButtonType button)
        {
            if (this.Browser != null)
            {
                var host = this.Browser.GetHost();
                var mouseEvent = new CefMouseEvent { X = x, Y = y };
                if (button == CefMouseButtonType.Left)
                {
                    mouseEvent.Modifiers = CefEventFlags.LeftMouseButton;
                }
                else if (button == CefMouseButtonType.Middle)
                {
                    mouseEvent.Modifiers = CefEventFlags.MiddleMouseButton;
                }
                else if (button == CefMouseButtonType.Right)
                {
                    mouseEvent.Modifiers = CefEventFlags.RightMouseButton;
                }

                host.SendMouseMoveEvent(mouseEvent, false);
            }
        }

        public void SendMouseUpDown(int x, int y, CefMouseButtonType button, bool isMouseUp)
        {
            if (this.Browser != null)
            {
                var host = this.Browser.GetHost();

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

                var mouseEvent = new CefMouseEvent { X = x, Y = y };
                host.SendMouseClickEvent(mouseEvent, button, isMouseUp, clickCount);

                lastClickPosX = x;
                lastClickPosY = y;
                lastClickButton = button;
                lastClickTime = DateTime.Now;
            }
        }

        public void SendMouseWheel(int x, int y, int delta, bool isVertical)
        {
            if (this.Browser != null)
            {
                var host = this.Browser.GetHost();
                var mouseEvent = new CefMouseEvent { X = x, Y = y };
                host.SendMouseWheelEvent(mouseEvent, isVertical ? delta : 0, !isVertical ? delta : 0);
            }
        }

        private bool IsContinuousClick(int x, int y, CefMouseButtonType button)
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

        public void SendKeyEvent(CefKeyEvent keyEvent)
        {
            if (this.Browser != null)
            {
                var host = this.Browser.GetHost();

                host.SendKeyEvent(keyEvent);
            }
        }

        public void showDevTools(bool firstwindow = true)
        {
            this.allBrowsersSemaphore.Wait();
            if (this.allBrowsers.Count > 0)
            {
                CefBrowser b = firstwindow ? this.allBrowsers.First() : this.allBrowsers.Last();
                CefWindowInfo wi = CefWindowInfo.Create();
                wi.SetAsPopup(b.GetHost().GetWindowHandle(), "DevTools");
                b.GetHost().ShowDevTools(wi, this.Client, new CefBrowserSettings(), new CefPoint());
            }
            this.allBrowsersSemaphore.Release();
        }

        // Runs on the Cef thread.
        internal void OnCreated(CefBrowser browser)
        {
            browser.GetHost().SendFocusEvent(true);
            this.allBrowsersSemaphore.Wait();
            this.allBrowsers.Add(browser);
            this.allBrowsersSemaphore.Release();
        }

        // Runs on the Cef thread.
        internal void OnBeforeClose(CefBrowser browser)
        {
          this.allBrowsersSemaphore.Wait();
          int index = this.allBrowsers.FindIndex((b) => b.Identifier == browser.Identifier);
          if (index > 0) {
            // The first browser window is only removed from the list on the main thread.
            System.Diagnostics.Debug.Assert(index > 0);
            this.allBrowsers.RemoveAt(index);
          }
          this.allBrowsersSemaphore.Release();
        }

        internal void OnPaint(CefBrowser browser, IntPtr buffer, int width, int height, CefRectangle[] dirtyRects)
        {
            Render?.Invoke(this, new RenderEventArgs(buffer, width, height, dirtyRects));
        }

        internal void OnError(CefErrorCode errorCode, string errorText, string failedUrl)
        {
            BrowserError?.Invoke(this, new BrowserErrorEventArgs(errorCode, errorText, failedUrl));
        }

        internal void OnLoad(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            BrowserLoad?.Invoke(this, new BrowserLoadEventArgs(httpStatusCode, frame.Url));
        }

        internal void OnConsoleLog(CefBrowser browser, string message, string source, int line)
        {
            BrowserConsoleLog?.Invoke(this, new BrowserConsoleLogEventArgs(message, source, line));
        }

        internal static void OnBroadcastMessage(object sender, BroadcastMessageEventArgs e)
        {
            BroadcastMessage?.Invoke(sender, e);
        }

        internal static void OnSendMessage(object sender, SendMessageEventArgs e)
        {
            SendMessage?.Invoke(sender, e);
        }

        internal static void OnOverlayMessage(object sender, SendMessageEventArgs e) {
            OverlayMessage?.Invoke(sender, e);
        }

        internal static void OnRendererFeatureRequest(object sender, RendererFeatureRequestEventArgs e)
        {
            RendererFeatureRequest?.Invoke(sender, e);
        }

        public void Dispose()
        {
            this.EndRender();
        }

        static bool initialized = false;

        public static void Initialize()
        {
            Initialize("");
        }

        public static void Initialize(string MainApplicationPath)
        {
            if (!initialized)
            {
                CefRuntime.Load();
                var Cache = "Cache";
                var Userdata = "UserData";

                if(MainApplicationPath != "")
                {
                    Cache = System.IO.Path.Combine(MainApplicationPath, "Cache");
                    Userdata = System.IO.Path.Combine(MainApplicationPath, "UserData");
                }

                var cefMainArgs = new CefMainArgs(new string[0]);
                var cefApp = new App();

                if (CefRuntime.ExecuteProcess(cefMainArgs, cefApp, IntPtr.Zero) != -1)
                {
                    Console.Error.WriteLine("Couldn't execute secondary process.");
                }

                var cefSettings = new CefSettings
                {
                    RemoteDebuggingPort = 9901,
                    Locale = System.Globalization.CultureInfo.CurrentCulture.Name,
                    CachePath = Cache,
                    UserDataPath = Userdata,
                    SingleProcess = true,
                    MultiThreadedMessageLoop = true,
                    LogSeverity = CefLogSeverity.Disable
                };

                CefRuntime.Initialize(cefMainArgs, cefSettings, cefApp, IntPtr.Zero);
                initialized = true;
            }
        }

        public static void Shutdown()
        {
            if (initialized)
            {
                CefRuntime.Shutdown();
            }
        }

        public void ExecuteScript(string script)
        {
            this.allBrowsersSemaphore.Wait();
            foreach (var browser in this.allBrowsers)
            {
                foreach (var frameId in browser.GetFrameIdentifiers())
                {
                    var frame = browser.GetFrame(frameId);
                    frame.ExecuteJavaScript(script, null, 0);
                }
            }
            this.allBrowsersSemaphore.Release();
        }
    }

    public class BrowserErrorEventArgs : EventArgs
    {
        public CefErrorCode ErrorCode { get; private set; }
        public string ErrorText { get; private set; }
        public string Url { get; private set; }
        public BrowserErrorEventArgs(CefErrorCode errorCode, string errorText, string url)
        {
            this.ErrorCode = errorCode;
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
}
