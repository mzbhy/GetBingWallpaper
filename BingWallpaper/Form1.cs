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
    /// <summary>
    /// 测试用窗体
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class Form1 : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        BingInfo WallpaperInfo = new BingInfo();

        /// <summary>
        /// Bing信息类，包含了时间、大图地址、小图地址等信息
        /// </summary>
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
            /// <summary>
            /// 采用Bing的一个xml格式的API，获取数据
            /// </summary>
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

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// 测试用按键
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            WallpaperInfo.GetBingInfo();
            string WallpaperUrl = WallpaperInfo.UrlSmall;
            pictureBox1.ImageLocation = WallpaperUrl;
        }

        /// <summary>
        /// Downloads the wallpaper.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据系统分辨率，下载不同大小的壁纸，调用系统dll设置壁纸
        /// </summary>
        /// <param name="WallpaperInfo">Binginfo类</param>
        /// <param name="width">系统分辨率宽度</param>
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


        /// <summary>
        /// 从user32.dll调用，用于查询或设置系统级参数
        /// </summary>
        /// <param name="uAction">查询或设置系统级参数.</param>
        /// <param name="uParam">参考uAction常数表.</param>
        /// <param name="lpvParam">按引用调用的Integer、Long和数据结构.</param>
        /// <param name="fuWinIni">在设置系统参数的时候，是否应更新用户设置参数</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
        int uAction,
        int uParam,
        string lpvParam,
        int fuWinIni
        );

        /// <summary>
        /// 托盘右键中预览图片的Handles
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WallpaperInfo.GetBingInfo();
            ImageShow WallpaperPreviewFrm = new ImageShow(WallpaperInfo);
            WallpaperPreviewFrm.Show();
        }

        /// <summary>
        /// 托盘右键中设置桌面背景的Handles
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void setAsWallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetWallpaper(WallpaperInfo, Screen.GetWorkingArea(this).Width);
        }

        /// <summary>
        /// 托盘右键中关于的Handles
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 myabout = new AboutBox1();
            myabout.Show();

        }

        /// <summary>
        /// 托盘右键中退出的Handles
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
