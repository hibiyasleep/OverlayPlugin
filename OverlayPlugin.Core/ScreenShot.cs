using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    internal static class ScreenShot
    {
        public static void SaveScreenShot(DIBitmap buffer, ScreenShotConfig config)
        {
            Bitmap bitmap = null;

            try
            {
                bitmap = new Bitmap(buffer.Width, buffer.Height, PixelFormat.Format32bppArgb);
                var rect = new Rectangle(Point.Empty, bitmap.Size);

                BitmapData bmpData = null;
                try
                {
                    bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    NativeMethods.CopyMemory(bmpData.Scan0, buffer.Bits, buffer.BitsCount);
                }
                finally
                {
                    if (bmpData != null)
                        bitmap.UnlockBits(bmpData);
                }

                if (config.AutoClipping)
                    bitmap = AutoClipping(bitmap);

            }
            catch (Exception ex)
            {
                bitmap?.Dispose();
                PluginMain.Logger.Log(LogLevel.Error, "OverlayPlugin Can't Take ScreenShot: {0}", ex.ToString());
                return;
            }

            using (bitmap)
            using (var src = new Bitmap(bitmap.Width + config.Margin * 2, bitmap.Height + config.Margin * 2, PixelFormat.Format32bppArgb))
            {
                if (!string.IsNullOrWhiteSpace(config.BackgroundImagePath) && File.Exists(config.BackgroundImagePath))
                {
                    try
                    {
                        DrawBackground(src, config.BackgroundImagePath, config.BackgroundMode);
                    }
                    catch (Exception ex)
                    {
                        PluginMain.Logger.Log(LogLevel.Error, "OverlayPlugin Can't Take ScreenShot: {0}", ex.ToString());
                    }
                }

                using (var g = Graphics.FromImage(src))
                {
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.DrawImageUnscaled(bitmap, config.Margin, config.Margin);
                }

                Directory.CreateDirectory(config.SavePath);

                src.Save(
                    Path.Combine(config.SavePath, DateTime.Now.ToString("'ScreenShot_'yyyy-MM-dd_HH-mm-ss.fff'.png'")),
                    ImageFormat.Png);
            }
        }

        private static void DrawBackground(Bitmap ss, string path, ScreenShotBackgroundMode mode)
        {
            if (mode == ScreenShotBackgroundMode.Hide)
                return;

            using (var bg = Image.FromFile(path))
            {
                using (var g = Graphics.FromImage(ss))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    switch (mode)
                    {
                        case ScreenShotBackgroundMode.Normal:
                            g.DrawImageUnscaled(bg, 0, 0);
                            return;

                        case ScreenShotBackgroundMode.Center:
                            g.DrawImageUnscaled(bg, (ss.Width - bg.Width) / 2 , (ss.Height - bg.Height) / 2);
                            return;

                        case ScreenShotBackgroundMode.Fill:
                            g.DrawImage(bg,
                                new Rectangle(Point.Empty, ss.Size),
                                new Rectangle(Point.Empty, bg.Size),
                                GraphicsUnit.Pixel);
                            return;

                        case ScreenShotBackgroundMode.Uniform:
                        case ScreenShotBackgroundMode.UniformToFill:
                            {
                                var scaleX = (double)ss.Width  / bg.Width;
                                var scaleY = (double)ss.Height / bg.Height;
                                var scale = mode == ScreenShotBackgroundMode.Uniform ?
                                            Math.Min(scaleX, scaleY) :
                                            Math.Max(scaleX, scaleY);
                                
                                int bg_w = (int)Math.Floor(bg.Width  * scale);
                                int bg_h = (int)Math.Floor(bg.Height * scale);

                                int ss_x = (int)Math.Ceiling((ss.Width  - bg_w) / 2d);
                                int ss_y = (int)Math.Ceiling((ss.Height - bg_h) / 2d);

                                g.DrawImage(
                                    bg,
                                    new Rectangle(ss_x, ss_y, bg_w,     bg_h),
                                    new Rectangle(0,    0,    bg.Width, bg.Height),
                                    GraphicsUnit.Pixel);
                            }
                            return;
                    }
                }
            }
        }

        private static Bitmap AutoClipping(Bitmap bitmap)
        {
            int newHeight = bitmap.Height;

            BitmapData bmpData = null;
            try
            {
                bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                byte[] strideBuffer = new byte[bmpData.Stride];

                int x;
                bool skip;
                while (--newHeight >= 1)
                {
                    Marshal.Copy(bmpData.Scan0 + bmpData.Stride * newHeight, strideBuffer, 0, bmpData.Stride);

                    skip = true;
                    for (x = 0; x < bmpData.Width; ++x)
                    {
                        if (strideBuffer[x * 4 + 3] != 0)
                        {
                            skip = false;
                            break;
                        }
                    }

                    if (!skip)
                        break;
                }
            }
            finally
            {
                if (bmpData != null)
                    bitmap.UnlockBits(bmpData);
            }

            var newBitmap = new Bitmap(bitmap.Width, newHeight, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(
                    bitmap,
                    new Rectangle(Point.Empty, newBitmap.Size),
                    new Rectangle(0, 0, bitmap.Width, newHeight),
                    GraphicsUnit.Pixel);
            }

            bitmap.Dispose();

            return newBitmap;
        }
    }
}
