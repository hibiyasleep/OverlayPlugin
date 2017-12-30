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

        public Rectangle Rect { get { return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top); } }

        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x,
                            y,
                            x + width,
                            y + height);
        }

        public static RECT FromRectangle(Rectangle rect)
        {
            return new RECT(rect.Left,
                             rect.Top,
                             rect.Right,
                             rect.Bottom);
        }
    }
}
