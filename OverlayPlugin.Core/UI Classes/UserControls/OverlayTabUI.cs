using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    [System.ComponentModel.DesignerCategory("CODE")]
    public class OverlayTabUI : TabControl
    {
        /*
         * Part of SRT4: https://github.com/laiglinne-ff/SRT4/blob/master/Client/SRinnLib/Design/MaterialColorTab.cs
         * Copyright (c) 2014 Laighlinne; Licensed GNU General Public License 3.0.
         */

        private System.ComponentModel.IContainer components = null;
        private const int TCM_FIRST = 0x1300;
        private const uint TCM_ADJUSTRECT = (TCM_FIRST + 40);
        private static readonly Brush LightGray = new SolidBrush(Color.FromArgb(225, 228, 232));
        private static readonly Brush LightCloud = new SolidBrush(Color.FromArgb(250, 251, 252));
        private static readonly Brush LodestoneTabFocusColor = new SolidBrush(Color.FromArgb(71, 105, 179));
        private static readonly Brush TopLiner = new SolidBrush(Color.FromArgb(227, 98, 9));

        public OverlayTabUI()
        {
            Initializer();
        }

        public void Initializer()
        {
            InitializeComponent();

            SetStyle((ControlStyles)(0x02 | 0x06 | 0x2000 | 0x20000), true);
            DoubleBuffered = true;
            Alignment = TabAlignment.Top;
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        static readonly Pen LightGrayPen = new Pen(Color.FromArgb(225, 228, 232), 1f);
        static readonly SolidBrush BlackBrush = new SolidBrush(Color.FromArgb(36, 41, 46));
        static readonly SolidBrush GrayBrush = new SolidBrush(Color.FromArgb(88, 96, 105));
        static readonly Font OnPaintFont = new Font("Microsoft NeoGothic", 13.75f, FontStyle.Regular, GraphicsUnit.Pixel);
        static readonly StringFormat strFormat = new StringFormat((StringFormatFlags)(0x1000 | 0x4000))
        {
            Alignment = StringAlignment.Far
        };

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.FromArgb(255, 255, 255));
            var rect = new Rectangle(0, ItemSize.Height - 2, Width, 1);
            var TBackRect = new Rectangle(0, 0, ItemSize.Height, Width);

            e.Graphics.FillRectangle(LightCloud, rect);
            e.Graphics.FillRectangle(LightGray, rect);
            for (var i = 0; i < TabCount; i++)
            {
                var TR = GetTabRect(i);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var TabRectExt = new Rectangle(TR.Left - 2, TR.Top - 2, TR.Width + 4, TR.Height + 2);
                if (i == SelectedIndex)
                {
                    GraphicsExt.FillRoundedRectangle(e.Graphics, Brushes.White, TR, 1);
                    GraphicsExt.DrawRoundedRectangle(e.Graphics, LightGrayPen, TR, 1);
                    GraphicsExt.FillRoundedRectangle(e.Graphics, TopLiner, new Rectangle(TR.Left + 1, TR.Top - 1, TR.Width - 2, TR.Height), 1);
                    e.Graphics.SmoothingMode = SmoothingMode.Default;
                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(TR.Left + 1, TR.Top + 2, TR.Width - 1, TR.Height));
                }

                e.Graphics.SmoothingMode = SmoothingMode.Default;
                
                Image img = null;
                var selectedBrush = GrayBrush;

                if (TabPages[i] is OverlayPageUI tce)
                {
                    img = tce.BlackImage;
                    if (i == SelectedIndex)
                    {
                        img = tce.GrayImage;
                        selectedBrush = BlackBrush;
                    }
                }
                
                if (img != null)
                    e.Graphics.DrawImage(img, new Point(TR.Left + 10, 11));

                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                e.Graphics.DrawString(TabPages[i].Text, OnPaintFont, selectedBrush, new Rectangle(TR.Left + 30, TR.Top + 9, TR.Width - 45, 20), strFormat);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == TCM_ADJUSTRECT))
            {
                RECT rc = (RECT)m.GetLParam(typeof(RECT));
                //Adjust these values to suit, dependant upon Appearance
                rc.left -= 5;
                rc.right += 5;
                rc.top -= 5;
                rc.bottom += 5;
                Marshal.StructureToPtr(rc, m.LParam, true);
            }
            base.WndProc(ref m);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
