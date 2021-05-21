using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
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

        public ListWindow()
        {
            InitializeComponent();
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
            mainWindow.Show();
        }

        private void OnActivited(object sender, EventArgs e)
        {
            foreach (Trip trip in listView.Items)
            {
                trip.ReLoad();
            }
        }
    }
}
