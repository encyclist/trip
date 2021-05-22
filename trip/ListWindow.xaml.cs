using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using ToolsCommon;
using trip.bean;
using trip.util;

namespace trip
{
    /// <summary>
    /// ListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ListWindow : Window
    {
        private List<MainWindow> mainWindows = new List<MainWindow>();
        private NotifyIcon _notifyIcon = null;

        public ListWindow()
        {
            InitializeComponent();
            InitialTray(); //一启动就最小化至托盘

            if (AdminUtil.IsRunAsAdmin())
                RegUtil.SelfRunning();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            string appPath = Directory.GetCurrentDirectory();
            FileInfo[] files = new DirectoryInfo(appPath).GetFiles();
            foreach (FileInfo file in files)
            {
                string fileName = file.Name.ToLower();
                if (fileName.StartsWith("content-") && fileName.EndsWith(".txt"))
                {
                    string time = fileName.Substring(8, 13);
                    Trip trip = new Trip(time);
                    MainWindow mainWindow = new MainWindow(trip);
                    mainWindows.Add(mainWindow);
                    listView.Items.Add(trip);
                    mainWindow.Show();
                }
            }
        }

        private void MenuItem_Click_Show(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            Trip item = (Trip)listView.Items[index];

            MainWindow mainWindow = null;
            foreach (MainWindow mainWindow1 in mainWindows)
            {
                if (mainWindow1.IsSame(item))
                {
                    mainWindow = mainWindow1;
                    mainWindow.Activate();
                    break;
                }
            }
            if (mainWindow == null)
            {
                mainWindow = new MainWindow(item);
                mainWindows.Add(mainWindow);
                mainWindow.Show();
            }
        }

        private void MenuItem_Click_Hide(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = mainWindows[listView.SelectedIndex];
            mainWindow.Close();
            mainWindows.Remove(mainWindow);
        }

        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {
            Trip item = (Trip)listView.SelectedItem;
            MainWindow mainWindow = null;

            listView.Items.Remove(item);
            foreach (MainWindow mainWindow1 in mainWindows)
            {
                if (mainWindow1.IsSame(item))
                {
                    mainWindow = mainWindow1;
                    break;
                }
            }
            mainWindows.Remove(mainWindow);
            mainWindow.Close();

            File.Delete(item.ContentFilePath);
            File.Delete(item.HistoryFilePath);
            IniFile.GetInstance().ClearSection(item.CreateTime);
        }

        // 列表页面关闭时关闭所有窗口
        private void OnClosedWindow(object sender, EventArgs e)
        {
            foreach (MainWindow mainWindow1 in mainWindows)
            {
                mainWindow1.Close();
            }
        }

        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            Trip trip = new Trip();
            MainWindow mainWindow = new MainWindow(trip);
            mainWindows.Add(mainWindow);
            listView.Items.Add(trip);
            mainWindow.Show();
        }

        private void OnClickHideAll(object sender, RoutedEventArgs e)
        {
            foreach (MainWindow mainWindow in mainWindows)
            {
                mainWindow.Close();
            }
            mainWindows.Clear();
        }

        private void OnActivited(object sender, EventArgs e)
        {
            foreach (Trip trip in listView.Items)
            {
                trip.ReLoad();
            }
        }

        #region 最小化系统托盘
        private void InitialTray()
        {
            //隐藏主窗体
            this.Visibility = Visibility.Hidden;
            //设置托盘的各个属性
            _notifyIcon = new NotifyIcon();
            _notifyIcon.BalloonTipText = "贴士已最小化至托盘图标运行";//托盘气泡显示内容
            _notifyIcon.Text = "贴士";
            _notifyIcon.Visible = true;//托盘按钮是否可见
            _notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);//托盘中显示的图标
            _notifyIcon.ShowBalloonTip(2000);//托盘气泡显示时间
            _notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            //窗体状态改变时触发
            this.StateChanged += MainWindow_StateChanged;
        }
        #endregion

        #region 窗口状态改变
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region 托盘图标鼠标单击事件
        private void notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }
        }
        #endregion
    }
}
