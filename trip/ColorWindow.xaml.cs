using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace trip
{
    /// <summary>
    /// ColorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ColorWindow : Window
    {
        // string settingPath = Directory.GetCurrentDirectory() + @"\tripinfo.ini";

        //添加一个委托
        public delegate void ColorChangeBetweenFormHandler(object sender, ColorChangeEventArgs e);
        //添加一个ColorChangeBetweenFormHandler类型的事件
        public event ColorChangeBetweenFormHandler ColorChangeBetweenForm;

        public ColorWindow()
        {
            InitializeComponent();
        }

        // 拖动窗口
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        // 选中了某个预设颜色
        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var b = sender as Label;
            Color color = (Color)ColorConverter.ConvertFromString(b.Background.ToString());
            ColorChangeEventArgs ccea = new ColorChangeEventArgs(color);
            ColorChangeBetweenForm(this, ccea);
            this.Close();
        }

        // 使用自定义颜色
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int a = int.Parse(tb_a.Text);
                int r = int.Parse(tb_r.Text);
                int g = int.Parse(tb_g.Text);
                int b = int.Parse(tb_b.Text);

                if (a <= 0 || a > 255 || r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
                    ;
                else
                {
                    string color = "#" + Pres(a) + Pres(r) + Pres(g) + Pres(b);
                    ColorChangeEventArgs ccea = new ColorChangeEventArgs((Color)ColorConverter.ConvertFromString(color));
                    ColorChangeBetweenForm(this, ccea);
                    this.Close();
                }
            }
            catch
            { }
        }

        private String Pres(int num)
        {
            String str = Convert.ToString(num, 16);
            if (str.Length == 1)
            {
                return "0" + str;
            }
            else
            {
                return str;
            }
        }

        // 取消
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GetColor_Click(object sender, RoutedEventArgs e)
        {
            GetColor gc = new GetColor();
            gc.ColorChangeBetweenForm += new GetColor.ColorChangeBetweenFormHandler(FrmChild_GetColorBetweenForm);
            gc.Show();
            gc.Activate();
        }

        // 取色返回事件
        private void FrmChild_GetColorBetweenForm(object sender, ColorChangeEventArgs e)
        {
            tb_a.Text = ConvertString(e.Color.ToString().Substring(1, 2), 16, 10);
            tb_r.Text = ConvertString(e.Color.ToString().Substring(3, 2), 16, 10);
            tb_g.Text = ConvertString(e.Color.ToString().Substring(5, 2), 16, 10);
            tb_b.Text = ConvertString(e.Color.ToString().Substring(7, 2), 16, 10);
            GetColor.Background = new SolidColorBrush(e.Color);
            //ColorChangeBetweenForm(this, e);
        }

        //进行转换
        private string ConvertString(string value, int frombase, int tobase)
        {
            string s;
            int intvalue;
            try
            {
                intvalue = Convert.ToInt32(value, frombase);
                s = Convert.ToString(intvalue, tobase);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (OverflowException)
            {
                return null;
            }
            return s;
        }
    }
}
