using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using trip.bean;

namespace trip
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Trip trip;

        public MainWindow(Trip trip)
        {
            this.trip = trip;
            InitializeComponent();

            Readtext();
            Readinfo();
        }

        //自动保存文本
        private void Text_KeyUp(object sender, KeyEventArgs e)
        {
            //创建一个文件流，用以写入或者创建一个StreamWriter
            FileStream fs = new FileStream(trip.ContentFilePath, FileMode.Truncate, FileAccess.Write);
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
            FileStream fs = new FileStream(trip.HistoryFilePath, FileMode.OpenOrCreate, FileAccess.Write);
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

        //读取配置信息
        void Readinfo()
        {
            panl.Left = trip.Left;

            panl.Top = trip.Top;

            Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(trip.BackgroundColor));
            b_grid.Background = brush;

            Brush brush2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(trip.ForegroundColor));
            texxt.Foreground = brush2;
            texxt.CaretBrush = brush2;

            this.Topmost = trip.Topmost;
        }

        //读取文本
        void Readtext()
        {
            //内容的操作
            FileStream fs = new FileStream(trip.ContentFilePath, FileMode.OpenOrCreate, FileAccess.Read);//打开或创建文件
            StreamReader MyStreamReader = new StreamReader(fs);
            //  从数据流中读取每一行，直到文件的最后一行，并在MyTextBox中显示出内容
            texxt.Text = MyStreamReader.ReadToEnd();
            //关闭此StreamReader对象
            MyStreamReader.Close();
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

        //写配置信息
        void Writeinfo()
        {
            trip.IniFile.IniWriteValue(trip.createTime, "Left", "" + panl.Left);
            trip.IniFile.IniWriteValue(trip.createTime, "Top", "" + panl.Top);
            trip.IniFile.IniWriteValue(trip.createTime, "Width", "" + panl.Width);
            trip.IniFile.IniWriteValue(trip.createTime, "Height", "" + panl.Height);
            trip.IniFile.IniWriteValue(trip.createTime, "Topmost", "" + this.Topmost);
            trip.IniFile.IniWriteValue(trip.createTime, "TBackgroundColorop", ((Brush)b_grid.Background).ToString());
            trip.IniFile.IniWriteValue(trip.createTime, "ForegroundColor", ((Brush)texxt.Foreground).ToString());
        }

        // 固定顶层/取消固定
        private void Label_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            this.Topmost = !this.Topmost;
            Writeinfo();
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

        // 两个实例是否指同一窗口
        public bool IsSame(MainWindow mainWindow)
        {
            return IsSame(mainWindow.trip);
        }
        public bool IsSame(Trip trip)
        {
            return ReferenceEquals(this.trip, trip);
        }
    }
}
