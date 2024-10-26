using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackPaper
{
    internal class WindowController
    {
        private Control ctl;    // 窗体控制器
        private const uint WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);
        private const int LWA_ALPHA = 0x2;

   

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(
        IntPtr hwnd,
        int nIndex,
        uint dwNewLong
        );

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(
        IntPtr hwnd,
        int nIndex
        );

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(
        IntPtr hwnd,
        int crKey,
        int bAlpha,
        int dwFlags
        );

        public WindowController(Control _ctl) => ctl = _ctl;



        ///<summary>
        /// 设置窗体具有鼠标穿透效果
        ///</summary>
        public void SetPenetrate()
        {
            
            GetWindowLong(ctl.Handle, GWL_EXSTYLE);
            SetWindowLong(ctl.Handle, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
            SetLayeredWindowAttributes(ctl.Handle, 0, 100, LWA_ALPHA);
        }



        #region 开机自动运行相关

        /// <summary>
        /// 检查是否开机自动启动，设置按钮状态等
        /// </summary>
        public bool checkIfAutoRun(string subkey, string name)
        {
            try
            {
                RegistryKey loca_chek = Registry.CurrentUser;
                RegistryKey run_Check = loca_chek.CreateSubKey(subkey);
                if (run_Check.GetValue(name) != null && run_Check.GetValue(name).ToString().ToLower() != "false")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 做开机自动启动设置，根据isAutoRun值设为或者取消开机自动启动
        /// </summary>
        public void AutoRun(string subkey, string name, bool isAutoRun)
        {
            //获取程序执行路径
            string starupPath = Application.ExecutablePath;
            //class Micosoft.Win32.RegistryKey. 表示Window注册表中项级节点,此类是注册表装.
            RegistryKey loca = Registry.CurrentUser;
            RegistryKey run = loca.CreateSubKey(subkey);
            try
            {
                //SetValue:存储值的名称
                if (isAutoRun == true)
                {
                    //设置开机运行
                    run.SetValue(name, starupPath);

                }
                else
                {
                    // cancel auto-start
                    run.DeleteValue(name);

                }
                loca.Close();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

    }
}
