using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace RainbowMage.OverlayPlugin
{
    public class MiniParseObject
    {
        private OverlayForm Overlay;
        public MiniParseObject(OverlayForm r)
        {
            Overlay = r;
        }

        public void OnBrowserLoaded(object sender, EventArgs e)
        {
            Overlay.Renderer.ExecuteScript("var srcEnable = true; var endEnable = true; var zoomEnable = true; var version = '1.2';");
        }

        public void SetForceZoom(string str)
        {
            string zoomSize = "1.0";
            switch (str)
            {
                case "75":
                    Overlay.Renderer.Browser.GetHost().SetZoomLevel(-1.25);
                    zoomSize = "-1.25";
                    break;
                case "80":
                    Overlay.Renderer.Browser.GetHost().SetZoomLevel(-1.2);
                    zoomSize = "-1.2";
                    break;
                case "90":
                    Overlay.Renderer.Browser.GetHost().SetZoomLevel(-1.1);
                    zoomSize = "-1.1";
                    break;
                case "10":
                    Overlay.Renderer.Browser.GetHost().SetZoomLevel(0);
                    zoomSize = "1.0";
                    break;
                case "11":
                    Overlay.Renderer.Browser.GetHost().SetZoomLevel(1.1);
                    zoomSize = "1.1";
                    break;
                case "12":
                    Overlay.Renderer.Browser.GetHost().SetZoomLevel(1.25);
                    zoomSize = "1.25";
                    break;
                case "15":
                    Overlay.Renderer.Browser.GetHost().SetZoomLevel(1.5);
                    zoomSize = "1.5";
                    break;
            }

            Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onForceZoomResized', { detail:{ take: true, size:\"" + zoomSize + "\" }}));");
        }

        public void TakeScreenshot(string str)
        {
            Bitmap bmp = new Bitmap(Overlay.Size.Width, Overlay.Size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(Overlay.Location.X, Overlay.Location.Y, 0, 0, Overlay.Size, CopyPixelOperation.SourceCopy);
            }

            Directory.CreateDirectory(Environment.CurrentDirectory + "\\scr\\");
            bmp.Save(Environment.CurrentDirectory + "\\scr\\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");

            Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onScreenShotTaked', { detail:{take: true, fileurl:\"" + Util.CreateJsonSafeString((Environment.CurrentDirectory + "\\scr\\")) + "\" }}));");
        }

        public void GetFileList(string str)
        {
            if (Directory.Exists(str))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("document.dispatchEvent(new CustomEvent('onGetFileList', {detail:{take:true, files:[");
                foreach (string dir in Directory.GetFiles(str))
                {
                    sb.Append("\"" + Util.CreateJsonSafeString(dir) + "\"");
                }

                sb.Append("]}}));");
                Overlay.Renderer.ExecuteScript(sb.ToString());
            }
            else
            {
                Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onGetFileList', {detail:{take:false, files:[]}}));");
            }
        }

        public void GetDirectoryList(string str)
        {
            if (Directory.Exists(str))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("document.dispatchEvent(new CustomEvent('onGetDirectoryList', {detail:{take:true, folders:[");
                foreach (string dir in Directory.GetFiles(str))
                {
                    sb.Append("\"" + Util.CreateJsonSafeString(dir) + "\"");
                }

                sb.Append("]}}));");
                Overlay.Renderer.ExecuteScript(sb.ToString());
            }
            else
            {
                Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onGetDirectoryList', {detail:{take:false, folders:[]}}));");
            }
        }

        public void ReadAllText(string dir)
        {
            if (File.Exists(dir))
            {
                Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onReadAllText', {detail:{take:true, file:\"" + Util.CreateJsonSafeString(File.ReadAllText(dir)) + "\"}}));");
            }
            else
            {
                Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onReadAllText', {detail:{take:false, file:\"\"}}));");
            }
        }

        public void WriteAllText(string dir, string file, bool ext = true)
        {
            if (!File.Exists(dir) || ext)
            {
                File.WriteAllText(dir, file, Encoding.UTF8);
                Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onWriteAllText', {detail:{take:true}}));");
            }
            else
            {
                Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onWriteAllText', {detail:{take:false}}));");
            }
        }
    }
}
