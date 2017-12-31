using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace LineEndingFixer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void FormLoadEvent(object sender, EventArgs e)
        {
            logs.AppendText("Target Path:\n" + Path.GetFullPath(path.Text));

            encoding.SelectedIndex = 0;
            rule.SelectedIndex = 0;
        }

        private void ExecuteButtonClick(object sender, EventArgs e)
        {
            logs.Text = "";
            var TargetPath = path.Text;
            var EncodingType = Encoding.UTF8;
            var LineEndingType = true;

            if (encoding.SelectedIndex != -1)
            {
                switch(encoding.SelectedIndex)
                {
                    case 1:
                        EncodingType = Encoding.ASCII;
                        break;
                    case 2:
                        EncodingType = Encoding.Default;
                        break;
                    default:

                        break;
                }
            }

            if (rule.SelectedIndex == 1)
                LineEndingType = false;

            new Thread((ThreadStart)delegate
            {
                FixLineEndingWork(TargetPath, EncodingType, LineEndingType);
            }).Start();
        }

        private void FixLineEndingWork(string path, Encoding encoding, bool CRLF)
        {
            string[] fixItems = new string[] { "*.cs", "*.vb", "*.csproj", "*.vbproj" };
            string lineEnding = CRLF ? "\r\n" : "\n";

            path = Path.GetFullPath(path);

            foreach (var fixItem in fixItems)
                foreach (var filepath in Directory.EnumerateFiles(path, fixItem, SearchOption.AllDirectories))
                {
                    Invoke((MethodInvoker)delegate { logs.AppendText("\n" + filepath); });

                    var lines = File.ReadAllLines(filepath);

                    using (FileStream stream = File.Open(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (StreamWriter writer = new StreamWriter(stream, encoding))
                        {
                            writer.NewLine = lineEnding;

                            foreach (var text in lines)
                            {
                                writer.WriteLine(text);
                            }

                            writer.Dispose();
                        }

                        stream.Dispose();
                    }
                }
        }
    }
}
