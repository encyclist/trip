using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace trip
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        string txtPath =  @"\trip.txt";
        string backPath =  @"\backtrip.txt";
        string settingPath =  @"\tripinfo.ini";

        public MainWindow()
        {
            string appStartupPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            txtPath = appStartupPath + @"\trip.txt";
            backPath = appStartupPath + @"\backtrip.txt";
            settingPath = appStartupPath + @"\tripinfo.ini";


            InitializeComponent();

            Readinfo();
            Readtext();
            if(IsRunAsAdmin())
                RegRun();

        }

        //保存当前位置
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }

            Writeinfo();
        }

        //自动保存文本
        private void Text_KeyUp(object sender, KeyEventArgs e)
        {
            //创建一个文件流，用以写入或者创建一个StreamWriter
            FileStream fs = new FileStream(txtPath, FileMode.Truncate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.Flush();
            //  使用StreamWriter来往文件中写入内容
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            //  把richTextBox1中的内容写入文件
            m_streamWriter.Write(texxt.Text);
            //关闭此文件
            Zhizuo_hosttrip();
            m_streamWriter.Flush();
            m_streamWriter.Close();
        }

        //写历史文件
        void Zhizuo_hosttrip()
        {
            String hostpath = backPath;//文件名;
            FileStream fs = new FileStream(hostpath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.Flush();
            //  使用StreamWriter来往文件中写入内容
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            //  把richTextBox1中的内容写入文件
            m_streamWriter.Write(texxt.Text);
            m_streamWriter.Write("\r\n--------------------------------------------------------------\r\n");
            //关闭此文件
            m_streamWriter.Flush();
            m_streamWriter.Close();
        }

        //开机自动启动
        void RegRun()
        {
            //可获得当前执行的exe的文件名。
            string str1 = Process.GetCurrentProcess().MainModule.FileName;
            //获取和设置当前目录(即该进程从中启动的目录)的完全限定路径。 备注 按照定义，如果该进程在本地或网络驱动器的根目录中启动，则此属性的值为驱动器名称后跟一个尾部反斜杠(如“C:\”)。如果该进程在子目录中启动，则此属性的值为不带尾部反斜杠的驱动器和子目录路径(如“C:\mySubDirectory”)。
            //string str2 = Environment.CurrentDirectory;
            //获取应用程序的当前工作目录。
            //string str3 = Directory.GetCurrentDirectory();
            ////获取基目录，它由程序集冲突解决程序用来探测程序集。
            ////string str4 = AppDomain.CurrentDomain.BaseDirectory;
            ////获取或设置包含该应用程序的目录的名称。
            ////string str7 = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            //RegistryKey rgkRun = null;
            //try
            //{
            //    rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //}
            //catch
            //{

            //}
            //if (rgkRun == null)
            //{
            //    rgkRun = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            //}
            //rgkRun.SetValue("trip", str1);

            if (!IsExistKey("trip"))
            {
                SelfRunning(true, "trip", str1);
            }
        }

        //读取配置信息
        void Readinfo()
        {
            FileStream fs2 = new FileStream(settingPath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader m_streamWriter2 = new StreamReader(fs2);

            string leftinfos = m_streamWriter2.ReadLine();
            if (leftinfos == null) leftinfos = "100";
            double leftinfo = Double.Parse(leftinfos);

            string topinfos = m_streamWriter2.ReadLine();
            if (topinfos == null) topinfos = "100";
            double topinfo = Double.Parse(topinfos);

            string cinfo = m_streamWriter2.ReadLine();
            if (cinfo == null) cinfo = "#FF1EDCCB";
            Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)cinfo));//string转brush
            Color color = (Color)ColorConverter.ConvertFromString(brush.ToString());                  //brush转color

            string cinfo2 = m_streamWriter2.ReadLine();
            if (cinfo2 == null) cinfo2 = "#FF000000";
            Brush brush2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)cinfo2));//string转brush

            string fixedTop = m_streamWriter2.ReadLine();
            if (fixedTop == null) fixedTop = "False";
            this.Topmost = Boolean.Parse(fixedTop);

            b_grid.Background = new SolidColorBrush(color);
            texxt.Foreground = brush2;
            texxt.CaretBrush = brush2;
            m_streamWriter2.Close();
            panl.Left = leftinfo;
            panl.Top = topinfo;
        }

        //读取文本
        void Readtext()
        {
            //内容的操作
            FileStream fs = new FileStream(txtPath, FileMode.OpenOrCreate, FileAccess.Read);//打开或创建文件
            StreamReader MyStreamReader = new StreamReader(fs);
            //  从数据流中读取每一行，直到文件的最后一行，并在MyTextBox中显示出内容
            texxt.Text = "";
            string strLine = MyStreamReader.ReadLine();
            while (strLine != null)
            {
                texxt.Text += strLine + "\n";
                strLine = MyStreamReader.ReadLine();
            }
            //关闭此StreamReader对象
            MyStreamReader.Close();
        }

        //更改背景颜色
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //int ikey = 0;
            //Color c1 = Color.FromArgb(255, 255, 255, 255);
            //Color c2 = Color.FromArgb(255, 30, 220, 203);
            //Color c3 = Color.FromArgb(255, 142, 228, 117);
            //Color c4 = Color.FromArgb(255, 204, 209, 114);
            //Color c5 = Color.FromArgb(255, 56, 154, 211);
            //Color c6 = Color.FromArgb(255, 139, 108, 195);
            //Color c7 = Color.FromArgb(255, 190, 52, 197);
            //Color c8 = Color.FromArgb(255, 228, 116, 131);
            //Color c9 = Color.FromArgb(001, 228, 116, 131);
            //Color[] c = new Color[9] { c1, c2, c3, c4, c5, c6, c7, c8, c9 };

            //Random r = new Random();
            //ikey = r.Next(0, 9);
            //Color isc = c[ikey];
            //b_grid.Background = new SolidColorBrush(isc);

            //writeinfo();
        }

        //写配置信息
        void Writeinfo()
        {
            FileStream fs = new FileStream(settingPath, FileMode.Truncate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.Flush();
            //  使用StreamWriter来往文件中写入内容
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            //  把richTextBox1中的内容写入文件
            m_streamWriter.Write(panl.Left + "\r\n" + 
                panl.Top + "\r\n" + 
                ((Brush)b_grid.Background).ToString() + "\r\n" + 
                ((Brush)texxt.Foreground).ToString() + "\r\n" +
                this.Topmost
            );
            //关闭此文件
            m_streamWriter.Flush();
            m_streamWriter.Close();

            /*
             using System.Windows.Media;
            1、String转换成Color
            Color color = (Color)ColorConverter.ConvertFromString(string);
            2、String转换成Brush
            BrushConverter brushConverter = new BrushConverter();
            Brush brush = (Brush)brushConverter.ConvertFromString(string);
            3、Color转换成Brush
            Brush brush = new SolidColorBrush(color));
            4、Brush转换成Color有两种方法：
                （1）先将Brush转成string，再转成Color。
                 Color color= (Color)ColorConverter.ConvertFromString(brush.ToString());
                （2）将Brush转成SolidColorBrush，再取Color。
                 Color color= ((SolidColorBrush)CadColor.Background).Color;
            5、string转brush
            Brush color = newSolidColorBrush((Color)ColorConverter.ConvertFromString((string)str)); 
            6、Brush转string  
            ((Brush)value).ToString();
            7、string转byte[] 
            System.Text.UnicodeEncoding converter = newSystem.Text.UnicodeEncoding(); 
            byte[] stringBytes = converter.GetBytes(inputString);   
            8、byte[]转string   
            System.Text.UnicodeEncoding converter = newSystem.Text.UnicodeEncoding(); 
            stringoutputString = converter.GetString(stringByte);   
             */
        }

        //判断是不是以管理员身份运行的
        public static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        //更改背景颜色
        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ColorWindow cw = new ColorWindow();
            cw.ColorChangeBetweenForm += new ColorWindow.ColorChangeBetweenFormHandler(FrmChild_BackgroundColorChangeBetweenForm);
            cw.Show();
            cw.Activate();
        }

        //更改文字颜色
        private void Labe2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ColorWindow cw = new ColorWindow();
            cw.ColorChangeBetweenForm += new ColorWindow.ColorChangeBetweenFormHandler(FrmChild_TextColorChangeBetweenForm);
            cw.Show();
            cw.Activate();
        }

        // 更换背景颜色返回事件
        private void FrmChild_BackgroundColorChangeBetweenForm(object sender, ColorChangeEventArgs e)
        {
            b_grid.Background = new SolidColorBrush(e.Color);
            Writeinfo();
        }

        // 更换文字颜色返回事件
        private void FrmChild_TextColorChangeBetweenForm(object sender, ColorChangeEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(e.Color);
            texxt.Foreground = brush;
            texxt.CaretBrush = brush;
            Writeinfo();
        }

        //判断注册表键值对是否存在
        private bool IsExistKey(string keyName)
        {
            try
            {
                bool _exist = false;
                RegistryKey local = Registry.LocalMachine;
                RegistryKey runs = local.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (runs == null)
                {
                    RegistryKey key2 = local.CreateSubKey("SOFTWARE");
                    RegistryKey key3 = key2.CreateSubKey("Microsoft");
                    RegistryKey key4 = key3.CreateSubKey("Windows");
                    RegistryKey key5 = key4.CreateSubKey("CurrentVersion");
                    RegistryKey key6 = key5.CreateSubKey("Run");
                    runs = key6;
                }
                string[] runsName = runs.GetValueNames();
                foreach (string strName in runsName)
                {
                    if (strName.ToUpper() == keyName.ToUpper())
                    {
                        _exist = true;
                        return _exist;
                    }
                }
                return _exist;

            }
            catch
            {
                return false;
            }
        }

        //写入/删除注册表键值对
        ///isStart--是否开机自启动
        ///exeName--应用程序名
        ///path--应用程序路径
        private static bool SelfRunning(bool isStart, string exeName, string path)
        {
            try
            {
                RegistryKey local = Registry.LocalMachine;
                RegistryKey key = local.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (key == null)
                {
                    local.CreateSubKey("SOFTWARE//Microsoft//Windows//CurrentVersion//Run");
                }
                if (isStart)//若开机自启动则添加键值对
                {
                    key.SetValue(exeName, path);
                    key.Close();
                }
                else//否则删除键值对
                {
                    string[] keyNames = key.GetValueNames();
                    foreach (string keyName in keyNames)
                    {
                        if (keyName.ToUpper() == exeName.ToUpper())
                        {
                            key.DeleteValue(exeName);
                            key.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
                //throw;
            }

            return true;
        }

        // 固定顶层/取消固定
        private void Label_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            this.Topmost = !this.Topmost;
            Writeinfo();
        }
    }
}
