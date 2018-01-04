using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    using OverlaySet = Tuple<OverlayConfigBase, Form>;

    internal static class AutoHide
    {
        private static readonly List<OverlaySet> overlay = new List<OverlaySet>();
        private static readonly int actPid;
        private static readonly NativeMethods.WinEventDelegate winProc;
        private static IntPtr hookProc;

        static AutoHide()
        {
            winProc = new NativeMethods.WinEventDelegate(WinEventProc);
            using (var proc = Process.GetCurrentProcess())
                actPid = proc.Id;
        }
        
        public static void AddOverlay(OverlayConfigBase config, Form form)
        {
            lock (overlay)
                overlay.Add(new OverlaySet(config, form));
        }
        public static void RemoveOverlay(OverlayConfigBase config)
        {
            lock (overlay)
                overlay.RemoveAll(e => e.Item1 == config);
        }
        public static void RemoveOverlay(Form form)
        {
            lock (overlay)
                overlay.RemoveAll(e => e.Item2 == form);
        }

        private static bool enabled;
        public static bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (enabled == value)
                    return;

                enabled = value;
                if (value)
                {
                    hookProc = NativeMethods.SetWinEventHook(
                        NativeMethods.EVENT_SYSTEM_FOREGROUND,
                        NativeMethods.EVENT_SYSTEM_MINIMIZEEND,
                        IntPtr.Zero,
                        winProc,
                        0,
                        0,
                        NativeMethods.WINEVENT_OUTOFCONTEXT);
                }
                else
                {
                    NativeMethods.UnhookWinEvent(hookProc);
                    hookProc = IntPtr.Zero;
                }
            }
        }

        private static bool visibled = false;
        private static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (!enabled)
                return;

            if (eventType != NativeMethods.EVENT_SYSTEM_FOREGROUND &&
                eventType != NativeMethods.EVENT_SYSTEM_MINIMIZEEND &&
                eventType != NativeMethods.EVENT_SYSTEM_MINIMIZESTART)
                return;

            bool newVisible = false;
            bool isGameWindow = false;

            var foregroundHwnd = NativeMethods.GetForegroundWindow();
            if (!NativeMethods.IsWindow(foregroundHwnd))
                return;

            if (NativeMethods.GetWindowThreadProcessId(foregroundHwnd, out uint pid) == 0)
                return;

            if (pid == actPid)
                newVisible = true;
            else
            {
                var sb = new StringBuilder(256);
                if (NativeMethods.GetClassName(foregroundHwnd, sb, sb.MaxCapacity) > 0)
                    newVisible = sb.ToString() == "FFXIVGAME";
                else
                {
                    if (NativeMethods.GetWindowText(foregroundHwnd, sb, sb.MaxCapacity) == 0)
                        return;

                    newVisible = sb.ToString() == "FINAL FANTASY XIV";
                }

                isGameWindow = true;
            }
            
            if (visibled != newVisible)
            {
                lock (overlay)
                    overlay.ForEach(e => {
                        try
                        {
                            if (e.Item2.Visible = (e.Item1.IsVisible && newVisible))
                            {
                                if (isGameWindow && !IsOverlaysGameWindow(e.Item2.Handle, foregroundHwnd))
                                {
                                    EnsureTopMost(e.Item2.Handle);
                                }
                            }
                        }
                        catch
                        {
                        }
                    });

                visibled = newVisible;
            }
        }

        private static bool IsOverlaysGameWindow(IntPtr overlayHandle, IntPtr xivHandle)
        {
            var handle = overlayHandle;

            while (handle != IntPtr.Zero)
            {
                // Overlayウィンドウよりも前面側にFF14のウィンドウがあった
                if (handle == xivHandle)
                    return false;

                handle = NativeMethods.GetWindow(handle, NativeMethods.GW_HWNDPREV);
            }

            // 前面側にOverlayが存在する、もしくはFF14が起動していない
            return true;
        }

        private static void EnsureTopMost(IntPtr hWnd)
        {
            NativeMethods.SetWindowPos(
                hWnd,
                (IntPtr)NativeMethods.HWND_TOPMOST,
                0, 0, 0, 0,
                NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOACTIVATE);
        }

        private static class NativeMethods
        {
            public delegate void WinEventDelegate(
                IntPtr hWinEventHook,
                uint eventType,
                IntPtr hwnd,
                int idObject,
                int idChild,
                uint dwEventThread,
                uint dwmsEventTime);

            [DllImport("user32.dll")]
            public static extern uint GetWindowThreadProcessId(
                IntPtr hWnd,
                out uint lpdwProcessId);

            [DllImport("user32.dll")]
            public static extern IntPtr SetWinEventHook(
                uint eventMin,
                uint eventMax,
                IntPtr hmodWinEventProc,
                WinEventDelegate lpfnWinEventProc,
                uint idProcess,
                uint idThread,
                uint dwFlags);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool UnhookWinEvent(
                IntPtr hWinEventHook);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWindow(
                IntPtr hWnd);

            [DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern int GetClassName(
                IntPtr hWnd,
                StringBuilder lpClassName,
                int nMaxCount);

            [DllImport("user32.dll", EntryPoint = "GetWindowTextW", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern int GetWindowText(
                IntPtr hWnd,
                StringBuilder lpClassName,
                int nMaxCount);

            [DllImport("user32.dll")]
            public static extern bool SetWindowPos(
                IntPtr hWnd,
                IntPtr hWndInsertAfter,
                int X,
                int Y,
                int cx,
                int cy,
                uint uFlags);

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindow(
                IntPtr hWnd,
                uint uCmd);

            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();

            public const int EVENT_SYSTEM_FOREGROUND = 0x3;
            public const int EVENT_SYSTEM_MINIMIZESTART = 0x16;
            public const int EVENT_SYSTEM_MINIMIZEEND = 0x17;
            public const int WINEVENT_OUTOFCONTEXT = 0x0;
            
            public const int HWND_TOPMOST = -1;

            public const uint SWP_NOSIZE = 0x0001;
            public const uint SWP_NOMOVE = 0x0002;
            public const uint SWP_NOACTIVATE = 0x0010;

            public const uint GW_HWNDPREV = 0x0003;
        }
    }
}
