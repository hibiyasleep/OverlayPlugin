using System.Drawing;
using System.Runtime.InteropServices;

namespace RainbowMage.OverlayPlugin
{
    /*
     * Part of SRT4: https://github.com/laiglinne-ff/SRT4/blob/master/Client/SRinnLib/Design/MaterialColorTab.cs
     * Copyright (c) 2014 Laighlinne; Licensed GNU General Public License 3.0.
     */
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public static explicit operator Rectangle(RECT value)
        {
            return new Rectangle(value.left, value.top, value.right - value.left, value.bottom - value.top);
        }
        public static explicit operator RECT(Rectangle value)
        {
            return new RECT(value.Left, value.Top, value.Right, value.Bottom);
        }

        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x,
                            y,
                            x + width,
                            y + height);
        }
    }
}
