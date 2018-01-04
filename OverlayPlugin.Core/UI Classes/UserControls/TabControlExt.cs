using System;
using System.Drawing;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    [System.ComponentModel.DesignerCategory("CODE")]
    public class TabControlExt : TabControl
    {
        static readonly StringFormat CenterStringFormat = new StringFormat()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center,
        };

        private Font fontFSmall;
        private Font fontBold;
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;

                this.fontFSmall?.Dispose();
                this.fontFSmall = new Font(value.FontFamily, (float)(value.Size * 0.85));

                this.fontBold?.Dispose();
                this.fontBold = new Font(value, FontStyle.Bold);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.ControlLightLight);
            e.Graphics.FillRectangle(SystemBrushes.ControlLight, 4, 4, ItemSize.Height - 4, Height - 8);

            int inc = 0;

            foreach (TabPage tp in TabPages)
            {
                Brush fore = Brushes.Black;
                Font fontF = this.Font;
                Rectangle tabrect = GetTabRect(inc);
                Rectangle rect = new Rectangle(tabrect.X + 4, tabrect.Y + 4, tabrect.Width - 8, tabrect.Height - 2);
                Rectangle textrect1 = new Rectangle(tabrect.X + 4, tabrect.Y + 4, tabrect.Width - 8, tabrect.Height - 20);
                Rectangle textrect2 = new Rectangle(tabrect.X + 4, tabrect.Y + 20, tabrect.Width - 8, tabrect.Height - 20);


                if (inc == SelectedIndex)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, rect);
                    fore = SystemBrushes.HighlightText;
                    fontF = this.fontBold;
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.White, rect);
                }

                e.Graphics.DrawString(tp.Name, fontF, fore, textrect1, CenterStringFormat);
                e.Graphics.DrawString(tp.Text, fontFSmall, fore, textrect2, CenterStringFormat);
                inc++;
            }
        }

        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.OnTabIndexChanged(e);
            Invalidate();
        }

        public TabControlExt() : base()
        {
            Alignment = TabAlignment.Left;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            DoubleBuffered = true;

            ItemSize = new Size(46, 140);
            SizeMode = TabSizeMode.Fixed;
            BackColor = Color.Transparent;

            this.Font = base.Font;
        }
    }
}
