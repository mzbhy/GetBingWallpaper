using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingWallpaper
{
    public partial class ImageShow : Form
    {
        public ImageShow()
        {
            InitializeComponent();
        }

        public ImageShow(string Url)
        {
            InitializeComponent();
            WallpaperUrl = Url;
            pictureBox1.ImageLocation = WallpaperUrl;
            pictureBox1.Controls.Add(pictureBox2);
            pictureBox1.Controls.Add(pictureBox3);
            pictureBox1.Controls.Add(pictureBox4);
        }

        public ImageShow(Form1.BingInfo NewInfo)
        {
            InitializeComponent();
            WallpaperInfo = NewInfo;
            pictureBox1.ImageLocation = WallpaperInfo.UrlSmall;
            pictureBox1.Controls.Add(pictureBox2);
            pictureBox1.Controls.Add(pictureBox3);
            pictureBox1.Controls.Add(pictureBox4);
            toolTip1.SetToolTip(pictureBox2, "Set as wallpaper");
            toolTip1.SetToolTip(pictureBox3, "Close");
            toolTip1.SetToolTip(pictureBox4, WallpaperInfo.Copyright);
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            p = e.Location;
        }
        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + (e.X - p.X), this.Location.Y + (e.Y - p.Y));
            }
        }

        Point p = new Point();
        string WallpaperUrl;
        Form1.BingInfo WallpaperInfo;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1.SetWallpaper(WallpaperInfo, Screen.GetWorkingArea(this).Width);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
