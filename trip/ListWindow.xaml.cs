﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using ToolsCommon;
using trip.bean;
using trip.dialog;
using trip.util;

namespace trip
{
    /// <summary>
    /// ListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ListWindow : Window
    {
        public static ListWindow Instance { get; private set; }

        private List<MainWindow> mainWindows = new List<MainWindow>();
        private NotifyIcon _notifyIcon = null;

        public ListWindow()
        {
            InitializeComponent();
            Instance = this;

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

            InitialTray(); //一启动就最小化至托盘
        }

        private void MenuItem_Click_Show(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            Trip item = (Trip)listView.Items[index];
            ShowItemWindow(item);
        }

        // 显示某一个贴士的窗口
        private void ShowItemWindow(Trip item) 
        {
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

        private void MenuItem_Click_SetTitle(object sender, RoutedEventArgs e)
        {
            Trip item = (Trip)listView.SelectedItem;

            InputDialog dialog = new InputDialog(item.Title);
            var r = dialog.ShowDialog();
            //得到弹窗返回结果，如果是true
            if (r.HasValue && r.Value == true)
            {
                IniFile.GetInstance().IniWriteValue(item.CreateTime, "Title", dialog.textBox.Text);
                item.ReLoad();
                RefreshContextMenu();
            }
        }

        private void MenuItem_Click_History(object sender, RoutedEventArgs e)
        {
            Trip item = (Trip)listView.SelectedItem;
            System.Diagnostics.Process.Start(item.HistoryFilePath);
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
            _notifyIcon = new NotifyIcon
            {
                BalloonTipText = "贴士已最小化至托盘图标运行",//托盘气泡显示内容
                Text = "贴士",
                Visible = true,//托盘按钮是否可见
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath)//托盘中显示的图标
            };
            RefreshContextMenu();

            _notifyIcon.ShowBalloonTip(2000);//托盘气泡显示时间
            //_notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            _notifyIcon.MouseClick += NotifyIcon_MouseClick;
            //窗体状态改变时触发
            this.StateChanged += MainWindow_StateChanged;
        }
        #endregion

        #region 窗口状态改变
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal; // 取消最小化的操作
                this.Visibility = Visibility.Hidden; // 隐藏窗口
                RefreshContextMenu();// 添加右键菜单
            }
        }
        #endregion

        #region 托盘图标鼠标单击事件
        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
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
            else if (e.Button == MouseButtons.Right) 
            {
            }
        }
        #endregion

        // 刷新右键菜单项
        public void RefreshContextMenu()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            MenuItem add = new MenuItem("新增");
            add.Click += new EventHandler(OnClickContextMentItemAdd);
            menuItems.Add(add);
            for (int i= 0;i< listView.Items.Count;i++)
            {
                Trip trip = (Trip)listView.Items[i];
                string text = trip.Title;
                if (text == null || text.Length < 1) 
                {
                    text = trip.Content.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                    if (text.Length > 20)
                    {
                        text = text.Substring(0, 20);
                    }
                }
                MenuItem menuItem = new MenuItem(text)
                {
                    Tag = trip
                };
                menuItem.Click += new EventHandler(OnClickContextMentItem);
                menuItems.Add(menuItem);
            }
            _notifyIcon.ContextMenu = new ContextMenu(menuItems.ToArray());
        }

        private void OnClickContextMentItemAdd(object sender, EventArgs e)
        {
            OnClickAdd(null,null);
        }
        // 点击了右键菜单中的某一项
        private void OnClickContextMentItem(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Trip trip = (Trip)menuItem.Tag;
            ShowItemWindow(trip);
        }
    }
}