using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    [System.ComponentModel.DesignerCategory("CODE")]
    public class OverlayPageUI : TabPage
    {
        private Image m_image;
        public Image TabImage
        {
            get
            {
                return m_image;
            }
            set
            {
                if (m_image != value)
                {
                    GrayImage?.Dispose();
                    BlackImage?.Dispose();

                    m_image = value;

                    GrayImage = MixImage(value, Color.FromArgb(36, 41, 46));
                    BlackImage = MixImage(value, Color.FromArgb(182, 184, 187));
                }
            }
        }
        public Image GrayImage { get; private set; }
        public Image BlackImage { get; private set; }

        private static Image MixImage(Image image, Color color)
        {
            var bmp = image as Bitmap;
            var img = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            for (var x = 0; x < image.Width; x++)
                for (var y = 0; y < image.Height; y++)
                    img.SetPixel(x, y, Color.FromArgb(bmp.GetPixel(x, y).A, color));

            return img;
        }
    }
}
