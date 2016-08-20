﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// 用于图片的预览，可以快速设置壁纸
/// </summary>
namespace BingWallpaper
{
    public partial class ImageShow : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageShow"/> class.
        /// </summary>
        public ImageShow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageShow"/> class.
        /// </summary>
        /// <param name="Url">壁纸url</param>
        public ImageShow(string Url)
        {
            InitializeComponent();
            WallpaperUrl = Url;
            pictureBox1.ImageLocation = WallpaperUrl;
            pictureBox1.Controls.Add(pictureBox2); //用于处理pictureBox堆叠时的透明
            pictureBox1.Controls.Add(pictureBox3);
            pictureBox1.Controls.Add(pictureBox4);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageShow"/> class.
        /// </summary>
        /// <param name="NewInfo">BingInfo类</param>
        public ImageShow(Form1.BingInfo NewInfo)
        {
            InitializeComponent();
            WallpaperInfo = NewInfo;
            pictureBox1.ImageLocation = WallpaperInfo.UrlSmall;
            pictureBox1.Controls.Add(pictureBox2); //用于处理pictureBox堆叠时的透明
            pictureBox1.Controls.Add(pictureBox3);
            pictureBox1.Controls.Add(pictureBox4);
            toolTip1.SetToolTip(pictureBox2, "Set as wallpaper");
            toolTip1.SetToolTip(pictureBox3, "Close");
            toolTip1.SetToolTip(pictureBox4, WallpaperInfo.Copyright);
        }

        /// <summary>
        /// Handles the MouseDown event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            p = e.Location;
        }

        /// <summary>
        /// 使窗体可以在任意位置用鼠标拖动
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// 图片按键，设置桌面
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1.SetWallpaper(WallpaperInfo, Screen.GetWorkingArea(this).Width);
        }

        /// <summary>
        /// 图片按键，关闭当前预览窗体
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
