using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RainbowMage.HtmlRenderer;

namespace RainbowMage.OverlayPlugin
{
    public abstract class OverlayBase<TConfig> : IOverlay, IEventReceiver
        where TConfig: OverlayConfigBase
    {
        private bool disableLog = false;
        private Action hotKeyCallback = null;

        protected System.Timers.Timer timer;
        /// <summary>
        /// オーバーレイがログを出力したときに発生します。
        /// </summary>
        public event EventHandler<LogEventArgs> OnLog;

        /// <summary>
        /// ユーザーが設定したオーバーレイの名前を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// オーバーレイフォームを取得します。
        /// </summary>
        public OverlayForm Overlay { get; private set; }

        /// <summary>
        /// オーバーレイの設定を取得します。
        /// </summary>
        public TConfig Config { get; private set; }

/// <summary>
        /// プラグインの設定を取得します。
        /// </summary>
        public IPluginConfig PluginConfig { get; private set; }
        IOverlayConfig IOverlay.Config { get => Config; set => Config = (TConfig)value; }
        public bool Visible {
            get
            {
                return Overlay == null ? false : Overlay.Visible;
            }
            set
            {
                if (Overlay != null) Overlay.Visible = value;
            }
        }

        protected OverlayBase(TConfig config, string name)
        {
            this.PluginConfig = Registry.Resolve<IPluginConfig>();
            this.Config = config;
            this.Name = name;

            if (this.Config == null)
            {
                this.Config = (TConfig) typeof(TConfig).GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { name });
            }

            InitializeOverlay();
            InitializeTimer();
            InitializeConfigHandlers();
        }

        public abstract Control CreateConfigControl();

        /// <summary>
        /// オーバーレイの更新を開始します。
        /// </summary>
        public virtual void Start()
        {
            if (Config == null) throw new InvalidOperationException("Configuration is missing!");

            timer.Start();
        }

        /// <summary>
        /// オーバーレイの更新を停止します。
        /// </summary>
        public virtual void Stop()
        {
            timer.Stop();
        }

        /// <summary>
        /// オーバーレイを初期化します。
        /// </summary>
        protected virtual void InitializeOverlay()
        {
            try
            {
                // FIXME: is this *really* correct way to get version of current assembly?
                this.Overlay = new OverlayForm(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    this.Name, Config.Url, this.Config.MaxFrameRate, new OverlayApi(this));

                UpdateHotKey();

                // 画面外にウィンドウがある場合は、初期表示位置をシステムに設定させる
                if (!Util.IsOnScreen(this.Overlay))
                {
                    this.Overlay.StartPosition = FormStartPosition.WindowsDefaultLocation;
                }
                else
                {
                    this.Overlay.Location = this.Config.Position;
                }

                this.Overlay.Text = this.Name;
                this.Overlay.Size = this.Config.Size;
                this.Overlay.IsClickThru = this.Config.IsClickThru;

                // イベントハンドラを設定
                this.Overlay.Renderer.BrowserError += (o, e) =>
                {
                    Log(LogLevel.Error, "BrowserError: {0}, {1}, {2}", e.ErrorCode, e.ErrorText, e.Url);
                };
                this.Overlay.Renderer.BrowserStartLoading += (o, e) =>
                {
                    // Drop the event subscriptions from the previous page.
                    UnsubscribeAll();
                };
                this.Overlay.Renderer.BrowserLoad += (o, e) =>
                {
                    Log(LogLevel.Debug, "BrowserLoad: {0}: {1}", e.HttpStatusCode, e.Url);
                    NotifyOverlayState();
                };
                this.Overlay.Renderer.BrowserConsoleLog += (o, e) =>
                {
                    if (Config.LogConsoleMessages)
                        Log(LogLevel.Info, "BrowserConsole: {0} (Source: {1}, Line: {2})", e.Message, e.Source, e.Line);
                };
                this.Config.UrlChanged += (o, e) =>
                {
                    Navigate(e.NewUrl);
                };

                this.Overlay.UpdateRender();
                this.Overlay.Show();

                this.Overlay.Visible = this.Config.IsVisible;
                this.Overlay.Locked = this.Config.IsLocked;
                this.Overlay.MaxFrameRate = this.Config.MaxFrameRate;
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, "InitializeOverlay: {0} {1}", this.Name, ex);
            }
        }

        private ModifierKeys GetModifierKey(Keys modifier)
        {
            ModifierKeys modifiers = new ModifierKeys();
            if ((modifier & Keys.Shift) == Keys.Shift)
            {
                modifiers |= ModifierKeys.Shift;
            }
            if ((modifier & Keys.Control) == Keys.Control)
            {
                modifiers |= ModifierKeys.Control;
            }
            if ((modifier & Keys.Alt) == Keys.Alt)
            {
                modifiers |= ModifierKeys.Alt;
            }
            if ((modifier & Keys.LWin) == Keys.LWin || (modifier & Keys.RWin) == Keys.RWin)
            {
                modifiers |= ModifierKeys.Win;
            }
            return modifiers;
        }

        private void UpdateHotKey()
        {
            var hook = Registry.Resolve<KeyboardHook>();

            // Clear the old hotkey
            if (hotKeyCallback != null)
            {
                hook.UnregisterHotKey(hotKeyCallback);
            }

            if (this.Config.GlobalHotkeyEnabled)
            {
                var modifierKeys = GetModifierKey(this.Config.GlobalHotkeyModifiers);
                var key = this.Config.GlobalHotkey;
                var hotkeyType = this.Config.GlobalHotkeyType;

                if (key != Keys.None)
                {
                    switch (hotkeyType)
                    {
                        case GlobalHotkeyType.ToggleVisible:
                            hotKeyCallback = () => this.Config.IsVisible = !this.Config.IsVisible;
                            break;
                        case GlobalHotkeyType.ToggleClickthru:
                            hotKeyCallback = () => this.Config.IsClickThru = !this.Config.IsClickThru;
                            break;
                        case GlobalHotkeyType.ToggleLock:
                            hotKeyCallback = () => this.Config.IsLocked = !this.Config.IsLocked;
                            break;
                        default:
                            hotKeyCallback = () => this.Config.IsVisible = !this.Config.IsVisible;
                            break;
                    }

                    try
                    {
                        hook.RegisterHotKey(modifierKeys, key, hotKeyCallback);
                    }
                    catch (Exception e)
                    {
                        Log(LogLevel.Error, Resources.OverlayBaseRegisterHotkeyError, e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// タイマーを初期化します。
        /// </summary>
        protected virtual void InitializeTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (o, e) =>
            {
                try
                {
                    Update();
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, "Update: {0}", ex.ToString());
                }
            };
        }

        /// <summary>
        /// 設定クラスのイベントハンドラを設定します。
        /// </summary>
        protected virtual void InitializeConfigHandlers()
        {
            this.Config.VisibleChanged += (o, e) =>
            {
                if (this.Overlay != null) this.Overlay.Visible = e.IsVisible;
            };

            this.Config.ClickThruChanged += (o, e) =>
            {
                if (this.Overlay != null) this.Overlay.IsClickThru = e.IsClickThru;
            };
            this.Config.LockChanged += (o, e) =>
            {
                if (this.Overlay != null) this.Overlay.Locked = e.IsLocked;
                NotifyOverlayState();
            };
            this.Config.MaxFrameRateChanged += (o, e) =>
            {
                if (this.Overlay != null) this.Overlay.MaxFrameRate = this.Config.MaxFrameRate;
            };
            this.Config.GlobalHotkeyChanged += (o, e) => UpdateHotKey();
            this.Config.GlobalHotkeyEnabledChanged += (o, e) => UpdateHotKey();
            this.Config.GlobalHotkeyModifiersChanged += (o, e) => UpdateHotKey();
        }

        /// <summary>
        /// オーバーレイを更新します。
        /// </summary>
        protected abstract void Update();

        /// <summary>
        /// オーバーレイのインスタンスを破棄します。
        /// </summary>
        public virtual void Dispose()
        {
            try
            {
                EventDispatcher.UnsubscribeAll(this);

                if (this.timer != null)
                {
                    this.timer.Stop();
                }
                if (this.Overlay != null)
                {
                    this.Overlay.Close();
                    this.Overlay.Dispose();
                }
                if (hotKeyCallback != null)
                {
                    Registry.Resolve<KeyboardHook>().UnregisterHotKey(hotKeyCallback);
                }
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, "Dispose: {0}", ex);
            }
        }

        public virtual void Navigate(string url)
        {
            if (this.Overlay != null)
            {
                this.Overlay.Url = url;
            }
        }

        public virtual void Reload()
        {
            if (this.Overlay != null)
            {
                this.Overlay.Reload();
            }
        }

        protected void Log(LogLevel level, string message)
        {
            if (PluginMain.Logger != null && !disableLog)
            {
                if (message.Contains("Xilium.CefGlue"))
                {
                    Log(LogLevel.Error, string.Format(Resources.IncompatibleAddon, this));
                    Stop();
                    disableLog = true;
                }

                PluginMain.Logger.Log(level, "{0}: {1}", this.Name, message);
            }
        }

        protected void Log(LogLevel level, string format, params object[] args)
        {
            Log(level, string.Format(format, args));
        }


        public void SavePositionAndSize()
        {
            this.Config.Position = this.Overlay.Location;
            this.Config.Size = this.Overlay.Size;
        }

        public void ExecuteScript(string script)
        {
            if (this.Overlay != null &&
                this.Overlay.Renderer != null)
            {
                this.Overlay.Renderer.ExecuteScript(script);
            }
        }

        private void NotifyOverlayState()
        {
            ExecuteScript(string.Format(
                "document.dispatchEvent(new CustomEvent('onOverlayStateUpdate', {{ detail: {{ isLocked: {0} }} }}));",
                this.Config.IsLocked ? "true" : "false"));
        }

        public void SendMessage(string message)
        {
            ExecuteScript(string.Format(
                "document.dispatchEvent(new CustomEvent('onBroadcastMessageReceive', {{ detail: {{ message: \"{0}\" }} }}));",
                Util.CreateJsonSafeString(message)));
        }

        public virtual void OverlayMessage(string message)
        {
        }

        public virtual void SetAcceptFocus(bool accept)
        {
            Overlay.SetAcceptFocus(accept);
        }

        // Event Source stuff

        public virtual void HandleEvent(JObject e)
        {
            ExecuteScript("if(window.__OverlayCallback) __OverlayCallback(" + e.ToString(Formatting.None) + ");");
        }

        public void Subscribe(string eventType)
        {
            EventDispatcher.Subscribe(eventType, this);
        }

        public void Unsubscribe(string eventType)
        {
            EventDispatcher.Unsubscribe(eventType, this);
        }

        public void UnsubscribeAll()
        {
            EventDispatcher.UnsubscribeAll(this);
        }

        public virtual void InitModernAPI()
        {

        }
    }
}
