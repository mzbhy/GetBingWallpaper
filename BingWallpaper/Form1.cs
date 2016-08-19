using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BingWallpaper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        BingInfo WallpaperInfo = new BingInfo();

        public class BingInfo
        {
            private System.DateTime startTime;
            private string urlSmall;
            private string urlBig;
            private string copyright;

            public BingInfo()
            {
                startTime = System.DateTime.Now;
                urlSmall = null;
                copyright = null;
            }

            public string UrlSmall
            {
                get { return urlSmall; }
                set { urlSmall = value; }
            }
            public string UrlBig
            {
                get { return urlBig; }
                set { urlBig = value; }
            }

            public string Copyright
            {
                get { return copyright; }
                set { copyright = value; }
            }
            public System.DateTime StartTime
            {
                get { return startTime; }
                set { startTime = value; }
            }
            public void GetBingInfo()
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"http://cn.bing.com/HPImageArchive.aspx?idx=0&n=1");
                XmlNode xn = doc.SelectSingleNode("images");
                xn = xn.SelectSingleNode("image");
                XmlNode xn_url = xn.SelectSingleNode("url");
                UrlSmall = "http://cn.bing.com/" + xn_url.InnerText;
                UrlBig = UrlSmall.Replace("1366x768", "1920x1080");
                XmlNode xn_copyright = xn.SelectSingleNode("copyright");
                Copyright = xn_copyright.InnerText;
                XmlNode xn_startdata = xn.SelectSingleNode("startdate");
                StartTime = System.DateTime.ParseExact(xn_startdata.InnerText, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WallpaperInfo.GetBingInfo();
            string WallpaperUrl = "http://cn.bing.com/" + WallpaperInfo.UrlSmall;
            pictureBox1.ImageLocation = WallpaperUrl;         
        }

        public static string DownloadWallpaper(string url)
        {
            string filename = null;
            try
            {
                WebClient client = new WebClient();
                filename = Path.GetFileName(url);
                client.DownloadFile(url, filename);
                return filename;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "Download Fail!";
            }
        }

        public static void SetWallpaper(BingInfo WallpaperInfo, int width)
        {
            string WallpaperUrl = null;
            WallpaperInfo.GetBingInfo();
            if (width == 1920)
                WallpaperUrl = WallpaperInfo.UrlBig;
            else if (width == 1366)
                WallpaperUrl = WallpaperInfo.UrlSmall;
            else
                MessageBox.Show("Resolution Error!");
            string filename = DownloadWallpaper(WallpaperUrl);
            string filepath = Directory.GetCurrentDirectory() + "\\" + filename;
            int SetResult = SystemParametersInfo(20, 0, filepath, 0x01 | 0x02);
            if (SetResult != 0)
                File.Delete(filepath);
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
        int uAction,
        int uParam,
        string lpvParam,
        int fuWinIni
        );

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WallpaperInfo.GetBingInfo();
            ImageShow WallpaperPreviewFrm = new ImageShow(WallpaperInfo);
            WallpaperPreviewFrm.Show();
        }

        private void setAsWallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetWallpaper(WallpaperInfo, Screen.GetWorkingArea(this).Width);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 myabout = new AboutBox1();
            myabout.Show();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
