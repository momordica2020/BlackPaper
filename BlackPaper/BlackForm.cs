using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Xml.Linq;


namespace BlackPaper
{
    public partial class BlackForm : Form
    {
        private AppConfig config;
        private WindowController wc;
        ////是否开机自动启动
        //private bool isAutoRun = false;
        ////是否自动调整亮度，默认打开程序时为开
        //private bool isAutoChange = true;





        public BlackForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigManager cm = new ConfigManager();

            config = cm.LoadConfig();
            this.TopMost = true;
            wc = new WindowController(this);
            wc.SetPenetrate();

            // auto-start checked
            config.isAutoRun = wc.checkIfAutoRun(config.appSubkey,config.appName);
            if (config.isAutoChange) setLightLevel(-1);
            else setLightLevel(config.beginLight);

            开机自动启动ToolStripMenuItem.Checked = config.isAutoRun;

            notifyIcon2.Icon = new Icon(Application.StartupPath + "/icon.ico");
        }

        private void 开机自动启动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (config.isAutoRun == true)
            {
                // cancel auto-start
                wc.AutoRun(config.appSubkey, config.appName, false);
                config.isAutoRun = false;
            }
            else
            {
                // set auto-start
                wc.AutoRun(config.appSubkey, config.appName, true);
                config.isAutoRun = true;
            }
            开机自动启动ToolStripMenuItem.Checked = config.isAutoRun;
        }

        /// <summary>
        /// 设置亮度。0~1为手动设置，01为自动按时调整
        /// </summary>
        /// <param name="level"></param>
        private void setLightLevel(double level)
        {
            if (level >= 0)
            {
                // manual
                this.Opacity = level;
                config.isAutoChange = false;
                config.beginLight = level;
            }
            else if(level < 0 ){
                // auto
                int nowHour = DateTime.Now.Hour;
                //int nowMinute = DateTime.Now.Minute;
                if (nowHour >= 8 && nowHour <= 18)
                {
                    //白天透明
                    level = 0;
                }
                else if (nowHour > 18 && nowHour <= 22)
                {
                    //傍晚四分之一
                    level = 0.2;
                }
                else  // 22~8
                {
                    //晚上半暗
                    level = 0.4;
                }
                this.Opacity = level;
                config.isAutoChange = true;
                config.beginLight = -1;
                
            }
            ToolStripMenuItem_state.Text = $"当前亮度：{(level*100).ToString("F0")}%";
            if (toolStripComboBox1.SelectedIndex != (int)(level * 5)) toolStripComboBox1.SelectedIndex = (int)(level * 5);
            自动调整亮度ToolStripMenuItem.Checked = config.isAutoChange;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigManager cfm=new ConfigManager();
            cfm.SaveConfig(config);
            this.Dispose();
        }


        private void 自动调整亮度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setLightLevel(-1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //定时准备自动调整亮度
            if (config.isAutoChange) setLightLevel(-1);
        }


        private void 关闭菜单ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            contextMenuStrip1.Hide();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            double op = (double)toolStripComboBox1.SelectedIndex / 5;
            setLightLevel(op);        
        }

        private void notifyIcon2_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
