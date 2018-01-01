using System;
using System.Drawing;
using System.IO;

namespace RainbowMage.OverlayPlugin
{
    public static class ColorOverlayer
    {
        private static string Overlays_base64 = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTZEaa/1AAABOUlEQVQ4T62UMU4DMRBFjQR9DpAiXXKIFBHKAdJwFS6QIgXiBAjlEDlASo5AQQdlREVBufnPzBgv63glZEtPmvkzHtk74w1d1zWlKILWWjyJN/FlvJq2Lu2BoRDCROwFTg1yJoP9PSeEpfgQOJxoJzZiamCjESOH3GWvRlaMk3mxo5hlsVuxyPyZIAeHPemkeUG/5tG1LPYpnv9o1+JF4OyTbkEagMFV0slSUghzcVPQF+Jb4KyiZgE6h7HLNzha9+LuQuxBYGyjbyKjgbHJkx2td/F4IUajMA7RN9G7Ns2THa1aQbqPcYq+ic0LNr/yWFNqBRl0jF5TxsamWJBc4Z/rd2wsWBvsQUEtBttfS3+wLaH29K7cNn/86Vliu59DEn5O2ub3laO1EltxECcDGy02oERR/D9dOANwrQtWQmTaYwAAAABJRU5ErkJggg==";
        private static string Logs_base64 = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTZEaa/1AAAAn0lEQVQ4T62UQRKDMAhFs3DpYXpTD+sBKD/F+gfoNJIsXgYe+YwLtYnIUlI5Qypn+BytbcqhnArEE5BBduOFEHypwsELryd7oX8CMpY9e28Sh5Z3PUrIe1GB81FYnfUZfKfXXlTgfBRW/+ozeB5EBc5HYXXWezDnO732ogLnL7H8xV7+6e0QyszPYf8u/IcFO37mSaVndBlIpWd0mYi0N742X8j2SxeGAAAAAElFTkSuQmCC";
        private static string Information_base64 = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTZEaa/1AAAAgElEQVQ4T+2VQQrAIAwE/f+r/FmrlEkWsSRWjxWmLElmry3t1cZ1iN41XezwhPb65zP0WBiGS/yFPtyBHgvDMIQikJ0fkDNQBLLzA3IGikB2fkDOQBHIbn6wCj0WhqGBECGOi2QFIUIcF8kKQoQ4LpIVhAhx1sQ36NFwipO/gFJvQLKcg+cDWMMAAAAASUVORK5CYII=";

        private readonly static Bitmap OverlaysImage;
        private readonly static Bitmap LogsImage;
        private readonly static Bitmap InformationImage;

        static ColorOverlayer()
        {
            var queue = new byte[][] { Convert.FromBase64String(Overlays_base64), Convert.FromBase64String(Logs_base64), Convert.FromBase64String(Information_base64) };
            var idx = 0;

            foreach (var i in queue)
            {
                using (MemoryStream ms = new MemoryStream(i))
                {
                    switch (idx++)
                    {
                        case 0:
                            OverlaysImage = (Bitmap)Bitmap.FromStream(ms);
                            break;
                        case 1:
                            LogsImage = (Bitmap)Bitmap.FromStream(ms);
                            break;
                        case 2:
                            InformationImage = (Bitmap)Bitmap.FromStream(ms);
                            break;
                    }
                    ms.Dispose();
                }
            }
        }

        private static Bitmap colorOverlayer(Bitmap image, Color color)
        {
            Bitmap img = new Bitmap(image.Width, image.Height);

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var alpha = image.GetPixel(x, y).A;
                    img.SetPixel(x, y, Color.FromArgb(alpha, color));
                }
            }

            return img;
        }

        public static Image OverlaysIcon(Color color)
        {
            return colorOverlayer(OverlaysImage, color);
        }

        public static Image LogsIcon(Color color)
        {
            return colorOverlayer(LogsImage, color);
        }

        public static Image InformationIcon(Color color)
        {
            return colorOverlayer(InformationImage, color);
        }
    }
}
