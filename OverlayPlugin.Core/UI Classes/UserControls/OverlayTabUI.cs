using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace RainbowMage.OverlayPlugin
{
    public class OverlayTabUI : TabControl
    {
        /*
         * Part of SRT4: https://github.com/laiglinne-ff/SRT4/blob/master/Client/SRinnLib/Design/MaterialColorTab.cs
         * Copyright (c) 2014 Laighlinne; Licensed GNU General Public License 3.0.
         */

        private System.ComponentModel.IContainer components = null;
        private const int TCM_FIRST = 0x1300;
        private const uint TCM_ADJUSTRECT = (TCM_FIRST + 40);
        private Brush LightGray = new SolidBrush(Color.FromArgb(225, 228, 232));
        private Brush LightCloud = new SolidBrush(Color.FromArgb(250, 251, 252));
        private Brush LodestoneTabFocusColor = new SolidBrush(Color.FromArgb(71, 105, 179));
        private Brush TopLiner = new SolidBrush(Color.FromArgb(227, 98, 9));
        private Image[] grayImages;
        private Image[] blackImages;

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
            
            var gray = Color.FromArgb(36, 41, 46);
            var black = Color.FromArgb(182, 184, 187);

            grayImages = new Image[] { ColorOverlayer.InformationIcon(gray), ColorOverlayer.LogsIcon(gray), ColorOverlayer.OverlaysIcon(gray) };
            blackImages = new Image[] { ColorOverlayer.InformationIcon(black), ColorOverlayer.LogsIcon(black), ColorOverlayer.OverlaysIcon(black) };
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen LightGrayPen = new Pen(LightGray, 1f);

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

                var black = new SolidBrush(Color.FromArgb(36, 41, 46));
                var gray = new SolidBrush(Color.FromArgb(88, 96, 105));

                var selectedBrush = gray;
                var img = blackImages[i];
                if (i == SelectedIndex)
                {
                    img = grayImages[i];
                    selectedBrush = black;
                }

                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                Font f = new Font("Microsoft NeoGothic", 13.75f, FontStyle.Regular, GraphicsUnit.Pixel);
                var strFormat = new StringFormat((StringFormatFlags)(0x1000 | 0x4000));
                strFormat.Alignment = StringAlignment.Far;

                e.Graphics.DrawImage(img, new Point(TR.Left + 10, 11));
                e.Graphics.DrawString(TabPages[i].Text, f, selectedBrush, new Rectangle(TR.Left + 30, TR.Top + 9, TR.Width - 45, 20), strFormat);
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
