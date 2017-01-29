using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Win32;

namespace BingWallpaper
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            readConfig();
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        private void readConfig()
        {
            String region = ConfigurationManager.AppSettings["Region"].ToString();
            String KeepHistory = ConfigurationManager.AppSettings["KeepHistory"].ToString();
            String HistoryDays = ConfigurationManager.AppSettings["HistoryDays"].ToString();
            String DownloadPath = ConfigurationManager.AppSettings["DownloadPath"].ToString();
            String QuitWhenDone = ConfigurationManager.AppSettings["QuitWhenDone"].ToString();
            String Boot = ConfigurationManager.AppSettings["Boot"].ToString();
            if (region == "zh-CN")
                radioButton1.Checked = true;
            if (region == "en-US")
                radioButton2.Checked = true;
            if (region == "en-UK")
                radioButton3.Checked = true;
            if (region == "en-AU")
                radioButton4.Checked = true;
            if (region == "de-DE")
                radioButton6.Checked = true;
            if (region == "en-NZ")
                radioButton5.Checked = true;
            if (region == "ja-JP")
                radioButton7.Checked = true;
            checkBox2.Checked = Convert.ToBoolean(KeepHistory);
            checkBox1.Checked = Convert.ToBoolean(QuitWhenDone);
            checkBox3.Checked = Convert.ToBoolean(Boot);
            textBox1.Text = HistoryDays;
            if (DownloadPath == "root")
                textBox2.Text = "";
            else
                textBox2.Text = DownloadPath;
            if (checkBox2.Checked)
            {
                textBox2.Enabled = true;
                textBox1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
                textBox1.Enabled = false;
                button2.Enabled = false;
            }
        }

        /// <summary>
        /// 用于使textbox仅能输入数字。
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// 应用button的Handles，将当前选型存入配置文件中
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (radioButton1.Checked)
                config.AppSettings.Settings["Region"].Value = "zh-CN";
            if (radioButton2.Checked)
                config.AppSettings.Settings["Region"].Value = "en-US";
            if (radioButton3.Checked)
                config.AppSettings.Settings["Region"].Value = "en-UK";
            if (radioButton4.Checked)
                config.AppSettings.Settings["Region"].Value = "en-AU";
            if (radioButton6.Checked)
                config.AppSettings.Settings["Region"].Value = "de-DE";
            if (radioButton5.Checked)
                config.AppSettings.Settings["Region"].Value = "en-NZ";
            if (radioButton7.Checked)
                config.AppSettings.Settings["Region"].Value = "ja-JP";
            if (checkBox2.Checked)
                config.AppSettings.Settings["KeepHistory"].Value = "true";
            else
                config.AppSettings.Settings["KeepHistory"].Value = "false";
            if (checkBox1.Checked)
                config.AppSettings.Settings["QuitWhenDone"].Value = "true";
            else
                config.AppSettings.Settings["QuitWhenDone"].Value = "false";
            if (textBox2.Text == "")
                config.AppSettings.Settings["DownloadPath"].Value = "root";
            else
                config.AppSettings.Settings["DownloadPath"].Value = textBox2.Text;
            config.AppSettings.Settings["HistoryDays"].Value = textBox1.Text;           
            if (checkBox3.Checked) //设置开机自启动  
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("JcShutdown", path);
                rk2.Close();
                rk.Close();
                config.AppSettings.Settings["Boot"].Value = "true";
            }
            else //取消开机自启动  
            { 
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("JcShutdown", false);
                rk2.Close();
                rk.Close();
                config.AppSettings.Settings["Boot"].Value = "false";
            }
            config.Save(ConfigurationSaveMode.Modified);  //保存修改
            ConfigurationManager.RefreshSection("appSettings"); //必须刷新才能读取修改后的值
            button3.Text = "关闭";
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBox2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox2.Enabled = true;
                textBox1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
                textBox1.Enabled = false;
                button2.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
