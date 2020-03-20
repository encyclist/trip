using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace trip
{
    /// <summary>
    /// GetColor.xaml 的交互逻辑
    /// </summary>
    public partial class GetColor : Window
    {
        //添加一个委托
        public delegate void ColorChangeBetweenFormHandler(object sender, ColorChangeEventArgs e);
        //添加一个ColorChangeBetweenFormHandler类型的事件
        public event ColorChangeBetweenFormHandler ColorChangeBetweenForm;

        private struct POINT
        {
            public uint X;
            public uint Y;
        }
        [DllImport("gdi32")]
        private static extern int GetPixel(int hdc, int nXPos, int nYPos);
        [DllImport("user32")]
        private static extern int GetWindowDC(int hwnd);
        [DllImport("user32")]
        private static extern int GetCursorPos(out POINT lpPoint);
        [DllImport("user32")]
        private static extern int ReleaseDC(int hWnd, int hDC);
        private static Color GetPixelColor(Point point)
        {
            int lDC = GetWindowDC(0);
            int intColor = GetPixel(lDC, (int)point.X, (int)point.Y);
            // Release the DC after getting the Color.
            ReleaseDC(0, lDC);
            //byte a = (byte)( ( intColor >> 0x18 ) & 0xffL );
            byte b = (byte)((intColor >> 0x10) & 0xffL);
            byte g = (byte)((intColor >> 8) & 0xffL);
            byte r = (byte)(intColor & 0xffL);
            Color color = Color.FromRgb(r, g, b);
            return color;
        }

        public GetColor()
        {
            InitializeComponent();

            this.Topmost = true;
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight-1;
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            POINT point;
            GetCursorPos(out point);
            Color color = GetPixelColor(new Point(point.X, point.Y));

            ColorChangeEventArgs ccea = new ColorChangeEventArgs(color);
            ColorChangeBetweenForm(this, ccea);
        }
    }
}
