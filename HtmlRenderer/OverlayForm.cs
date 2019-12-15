using RainbowMage.HtmlRenderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Structs;
using CefSharp.Enums;
using Point = System.Drawing.Point;

namespace RainbowMage.HtmlRenderer
{
    public partial class OverlayForm : Form
    {
        private DIBitmap surfaceBuffer;
        private object surfaceBufferLocker = new object();
        private System.Threading.Timer zorderCorrector;
        private bool terminated = false;
        private bool shiftKeyPressed = false;
        private bool altKeyPressed = false;
        private bool controlKeyPressed = false;

        private const int WS_EX_TOPMOST = 0x00000008;
        private const int WS_EX_LAYERED = 0x00080000;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        public Region DraggableRegion = null;

        public Renderer Renderer { get; private set; }

        private string url;
        public string Url
        {
            get { return this.url; }
            set
            {
                this.url = value;
                this.Renderer.Load(value);
            }
        }

        private bool isClickThru;
        public bool IsClickThru
        { 
            get
            {
                return this.isClickThru;
            }
            set
            {
                if (this.isClickThru != value)
                {
                    this.isClickThru = value;
                    UpdateMouseClickThru();
                }
            }
        }

        private int maxFrameRate;
        public int MaxFrameRate
        {
            get
            {
                return this.maxFrameRate;
            }
            set
            {
                this.maxFrameRate = value;
                this.Renderer.SetMaxFramerate(value);
            }
        }

        public bool IsLoaded { get; private set; }

        public bool Locked { get; set; }

        public OverlayForm(string overlayVersion, string overlayName, string url, int maxFrameRate = 30, object api = null)
        {
            InitializeComponent();

            this.maxFrameRate = maxFrameRate;
            this.Renderer = new Renderer(overlayVersion, overlayName, this, api);
            this.MouseWheel += OverlayForm_MouseWheel;
            this.MouseDown += OverlayForm_MouseDown;
            this.MouseUp += OverlayForm_MouseUp;
            this.MouseMove += OverlayForm_MouseMove;
            this.MouseLeave += OverlayForm_MouseLeave;
            this.KeyDown += OverlayForm_KeyDown;
            this.KeyUp += OverlayForm_KeyUp;
            this.Resize += OverlayForm_Resize;
            this.VisibleChanged += OverlayForm_VisibleChanged;

            this.url = url;
            
            // Alt+Tab を押したときに表示されるプレビューから除外する
            HidePreview();

        }

        /// <summary>
        /// 指定されたフォームを Windows の Alt+Tab の切り替え候補から除外します。
        /// </summary>
        /// <param name="form"></param>
        public void HidePreview()
        {
            int ex = NativeMethods.GetWindowLong(Handle, NativeMethods.GWL_EXSTYLE);
            ex |= NativeMethods.WS_EX_TOOLWINDOW;
            NativeMethods.SetWindowLongA(Handle, NativeMethods.GWL_EXSTYLE, (IntPtr)ex);
        }

        public void SetAcceptFocus(bool accept)
        {
            int ex = NativeMethods.GetWindowLong(Handle, NativeMethods.GWL_EXSTYLE);
            if (accept)
            {
                ex &= ~WS_EX_NOACTIVATE;
            } else
            {
                ex |= WS_EX_NOACTIVATE;
            }
            NativeMethods.SetWindowLongA(Handle, NativeMethods.GWL_EXSTYLE, (IntPtr)ex);
        }

        public void Reload()
        {
            this.Renderer.Reload();
        }

        #region Layered window related stuffs
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_NOACTIVATE;
                cp.ClassStyle = cp.ClassStyle | CP_NOCLOSE_BUTTON;

                return cp;
            }
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_NCHITTEST = 0x84;
            const int HTBOTTOMRIGHT = 17;

            const int gripSize = 16;

            if (m.Msg == WM_NCHITTEST && !this.Locked)
            {
                var posisiton = new Point(m.LParam.ToInt32() & 0xFFFF, m.LParam.ToInt32() >> 16);
                posisiton = this.PointToClient(posisiton);
                if (posisiton.X >= this.ClientSize.Width - gripSize &&
                    posisiton.Y >= this.ClientSize.Height - gripSize)
                {
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                    return;
                }
            }
            
            if (m.Msg == NativeMethods.WM_KEYDOWN ||
                m.Msg == NativeMethods.WM_KEYUP ||
                m.Msg == NativeMethods.WM_CHAR ||
                m.Msg == NativeMethods.WM_SYSKEYDOWN ||
                m.Msg == NativeMethods.WM_SYSKEYUP ||
                m.Msg == NativeMethods.WM_SYSCHAR)
            {
                OnKeyEvent(ref m);
            }
        }

        private void UpdateLayeredWindowBitmap()
        {
            if (surfaceBuffer.IsDisposed || this.terminated) { return; }

            var blend = new NativeMethods.BlendFunction
            {
                BlendOp = NativeMethods.AC_SRC_OVER,
                BlendFlags = 0,
                SourceConstantAlpha = 255,
                AlphaFormat = NativeMethods.AC_SRC_ALPHA
            };
            var windowPosition = new NativeMethods.Point
            {
                X = this.Left,
                Y = this.Top
            };
            var surfaceSize = new NativeMethods.Size
            {
                Width = surfaceBuffer.Width,
                Height = surfaceBuffer.Height
            };
            var surfacePosition = new NativeMethods.Point
            {
                X = 0,
                Y = 0
            };

            IntPtr handle = IntPtr.Zero;
            try
            {
                if (!this.terminated)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            handle = this.Handle;
                        }));
                    }
                    else
                    {
                        handle = this.Handle;
                    }

                    NativeMethods.UpdateLayeredWindow(
                        handle,
                        IntPtr.Zero,
                        ref windowPosition,
                        ref surfaceSize,
                        surfaceBuffer.DeviceContext,
                        ref surfacePosition,
                        0,
                        ref blend,
                        NativeMethods.ULW_ALPHA);
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
        #endregion

        #region Mouse click-thru related

        private void UpdateMouseClickThru()
        {
            if (this.IsLoaded)
            {
                if (this.isClickThru)
                {
                    EnableMouseClickThru();
                }
                else
                {
                    DisableMouseClickThru();
                }
            }
        }

        private void EnableMouseClickThru()
        {
            NativeMethods.SetWindowLong(
                this.Handle,
                NativeMethods.GWL_EXSTYLE,
                NativeMethods.GetWindowLong(this.Handle, NativeMethods.GWL_EXSTYLE) | NativeMethods.WS_EX_TRANSPARENT);
        }

        private void DisableMouseClickThru()
        {
            NativeMethods.SetWindowLong(
                this.Handle,
                NativeMethods.GWL_EXSTYLE,
                NativeMethods.GetWindowLong(this.Handle, NativeMethods.GWL_EXSTYLE) & ~NativeMethods.WS_EX_TRANSPARENT);
        }

        #endregion

        public void RenderFrame(Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            if (!this.terminated)
            {
                try
                {
                    if (surfaceBuffer != null &&
                        (surfaceBuffer.Width != width || surfaceBuffer.Height != height))
                    {
                        surfaceBuffer.Dispose();
                        surfaceBuffer = null;
                    }

                    if (surfaceBuffer == null)
                    {
                        surfaceBuffer = new DIBitmap(width, height);
                    }

                    // TODO: DirtyRect に対応
                    surfaceBuffer.SetSurfaceData(buffer, (uint)(width * height * 4));

                    UpdateLayeredWindowBitmap();
                }
                catch
                {

                }
            }
        }

        public void UpdateRender()
        {
            if (this.Renderer != null)
            {
                this.Renderer.BeginRender(this.Width, this.Height, this.Url, this.maxFrameRate);
            }
        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            this.IsLoaded = true;

            UpdateMouseClickThru();

            if (zorderCorrector == null)
            {
                zorderCorrector = new System.Threading.Timer((state) =>
                {
                    if (this.Visible)
                    {
                        if (!this.IsOverlaysGameWindow())
                        {
                            this.EnsureTopMost();
                        }
                    }
                }, null, 0, 1000);
            }
        }

        private void OverlayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Renderer.EndRender();
            terminated = true;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (zorderCorrector != null)
            {
                zorderCorrector.Dispose();
            }
            
            if (this.surfaceBuffer != null)
            {
                this.surfaceBuffer.Dispose();
            }

            if (this.Renderer != null)
            {
                this.Renderer.Dispose();
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OverlayForm_Resize(object sender, EventArgs e)
        {
            if (this.Renderer != null)
            {
                this.Renderer.Resize(this.Width, this.Height);
            }
        }

        private void OverlayForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Renderer != null)
            {
                this.Renderer.SetVisible(this.Visible);
            }
        }

        bool isDragging;
        bool hasDragged;
        Point offset;

        private void OverlayForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.Locked && !isDragging)
            {
                // If we have a draggable region defined, start dragging only if we're inside the region.
                if (DraggableRegion == null || DraggableRegion.IsVisible(e.Location))
                {
                    isDragging = true;
                    hasDragged = false;
                    offset = e.Location;
                }
            }

            this.Renderer.SendMouseUpDown(e.X, e.Y, GetMouseButtonType(e), false);
        }

        private void OverlayForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                if (!hasDragged)
                {
                    // Only notify once
                    hasDragged = true;
                    this.Renderer.NotifyMoveStarted();
                }

                this.Location = new Point(
                    e.X - offset.X + this.Location.X,
                    e.Y - offset.Y + this.Location.Y);
            }
            else
            {
                this.Renderer.SendMouseMove(e.X, e.Y, GetMouseButtonType(e));
            }
        }

        private void OverlayForm_MouseLeave(object sender, EventArgs e)
        {
            this.Renderer.SendMouseLeave();
        }

        private void OverlayForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;

            this.Renderer.SendMouseUpDown(e.X, e.Y, GetMouseButtonType(e), true);
        }

        private void OverlayForm_MouseWheel(object sender, MouseEventArgs e)
        {
            var flags = GetMouseEventFlags(e);

            this.Renderer.SendMouseWheel(e.X, e.Y, e.Delta, shiftKeyPressed);
        }

        private MouseButtonType GetMouseButtonType(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                return MouseButtonType.Left;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                return MouseButtonType.Middle;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                return MouseButtonType.Right;
            }
            else
            {
                return MouseButtonType.Left; // 非対応のボタンは左クリックとして扱う
            }
        }

        private CefEventFlags GetMouseEventFlags(MouseEventArgs e)
        {
            var flags = CefEventFlags.None;

            if (e.Button == MouseButtons.Left)
            {
                flags |= CefEventFlags.LeftMouseButton;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                flags |= CefEventFlags.MiddleMouseButton;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                flags |= CefEventFlags.RightMouseButton;
            }

            if (shiftKeyPressed)
            {
                flags |= CefEventFlags.ShiftDown;
            }
            if (altKeyPressed)
            {
                flags |= CefEventFlags.AltDown;
            }
            if (controlKeyPressed)
            {
                flags |= CefEventFlags.ControlDown;
            }

            return flags;
        }

        private bool IsOverlaysGameWindow()
        {
            var xivHandle = GetGameWindowHandle();
            var handle = this.Handle;

            while (handle != IntPtr.Zero)
            {
                // Overlayウィンドウよりも前面側にFF14のウィンドウがあった
                if (handle == xivHandle)
                {
                    return false;
                }

                handle = NativeMethods.GetWindow(handle, NativeMethods.GW_HWNDPREV);
            }

            // 前面側にOverlayが存在する、もしくはFF14が起動していない
            return true;
        }

        private void EnsureTopMost()
        {
            NativeMethods.SetWindowPos(
                this.Handle,
                NativeMethods.HWND_TOPMOST,
                0, 0, 0, 0,
                NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOACTIVATE);
        }

        private static object xivProcLocker = new object();
        private static Process xivProc;
        private static DateTime lastTry;
        private static TimeSpan tryInterval = new TimeSpan(0, 0, 15);

        private static IntPtr GetGameWindowHandle()
        {
            lock (xivProcLocker)
            {
                try
                {
                    // プロセスがすでに終了してるならプロセス情報をクリア
                    if (xivProc != null && xivProc.HasExited)
                    {
                        xivProc = null;
                    }

                    // プロセス情報がなく、tryIntervalよりも時間が経っているときは新たに取得を試みる
                    if (xivProc == null && DateTime.Now - lastTry > tryInterval)
                    {
                        xivProc = Process.GetProcessesByName("ffxiv").FirstOrDefault();
                        if (xivProc == null)
                        {
                            xivProc = Process.GetProcessesByName("ffxiv_dx11").FirstOrDefault();
                        }
                        lastTry = DateTime.Now;
                    }

                    if (xivProc != null)
                    {
                        return xivProc.MainWindowHandle;
                    }
                }
                catch (System.ComponentModel.Win32Exception) { }

                return IntPtr.Zero;
            }
        }

        private void OverlayForm_KeyDown(object sender, KeyEventArgs e)
        {
            this.altKeyPressed = e.Alt;
            this.shiftKeyPressed = e.Shift;
            this.controlKeyPressed = e.Control;
        }

        private void OverlayForm_KeyUp(object sender, KeyEventArgs e)
        {
            this.altKeyPressed = e.Alt;
            this.shiftKeyPressed = e.Shift;
            this.controlKeyPressed = e.Control;
        }

        private void OnKeyEvent(ref Message m)
        {

            var keyEvent = new KeyEvent();
            keyEvent.WindowsKeyCode = m.WParam.ToInt32();
            keyEvent.NativeKeyCode = (int)m.LParam.ToInt64();
            keyEvent.IsSystemKey = m.Msg == NativeMethods.WM_SYSCHAR ||
                                   m.Msg == NativeMethods.WM_SYSKEYDOWN ||
                                   m.Msg == NativeMethods.WM_SYSKEYUP;

            if (m.Msg == NativeMethods.WM_KEYDOWN || m.Msg == NativeMethods.WM_SYSKEYDOWN)
            {
                keyEvent.Type = KeyEventType.RawKeyDown;
            }
            else if (m.Msg == NativeMethods.WM_KEYUP || m.Msg == NativeMethods.WM_SYSKEYUP)
            {
                keyEvent.Type = KeyEventType.KeyUp;
            }
            else
            {
                keyEvent.Type = KeyEventType.Char;
            }
            keyEvent.Modifiers = GetKeyboardModifiers(ref m);

            if (this.Renderer != null)
            {
                this.Renderer.SendKeyEvent(keyEvent);
            }
        }

        private CefEventFlags GetKeyboardModifiers(ref Message m)
        {
            var modifiers = CefEventFlags.None;

            if (shiftKeyPressed) modifiers |= CefEventFlags.ShiftDown;
            if (controlKeyPressed) modifiers |= CefEventFlags.ControlDown;
            if (altKeyPressed) modifiers |= CefEventFlags.AltDown;

            if (IsKeyToggled(Keys.NumLock)) modifiers |= CefEventFlags.NumLockOn;
            if (IsKeyToggled(Keys.Capital)) modifiers |= CefEventFlags.CapsLockOn;

            switch ((Keys)m.WParam)
            {
                case Keys.Return:
                    if (((m.LParam.ToInt64() >> 48) & 0x0100) != 0)
                        modifiers |= CefEventFlags.IsKeyPad;
                    break;
                case Keys.Insert:
                case Keys.Delete:
                case Keys.Home:
                case Keys.End:
                case Keys.Prior:
                case Keys.Next:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    if (!(((m.LParam.ToInt64() >> 48) & 0x0100) != 0))
                        modifiers |= CefEventFlags.IsKeyPad;
                    break;
                case Keys.NumLock:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Divide:
                case Keys.Multiply:
                case Keys.Subtract:
                case Keys.Add:
                case Keys.Decimal:
                case Keys.Clear:
                    modifiers |= CefEventFlags.IsKeyPad;
                    break;
                case Keys.Shift:
                    if (shiftKeyPressed) modifiers |= CefEventFlags.IsLeft;
                    else modifiers |= CefEventFlags.IsRight;
                    break;
                case Keys.Control:
                    if (controlKeyPressed) modifiers |= CefEventFlags.IsLeft;
                    else modifiers |= CefEventFlags.IsRight;
                    break;
                case Keys.Alt:
                    if (altKeyPressed) modifiers |= CefEventFlags.IsLeft;
                    else modifiers |= CefEventFlags.IsRight;
                    break;
                case Keys.LWin:
                    modifiers |= CefEventFlags.IsLeft;
                    break;
                case Keys.RWin:
                    modifiers |= CefEventFlags.IsRight;
                    break;
            }

            return modifiers;
        }

        private bool IsKeyDown(Keys key)
        {
            return (NativeMethods.GetKeyState((int)key) & 0x8000) != 0;
        }

        private bool IsKeyToggled(Keys key)
        {
            return (NativeMethods.GetKeyState((int)key) & 1) == 1;
        }
    }
}
