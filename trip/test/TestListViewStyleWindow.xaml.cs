using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// TestListViewStyleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestListViewStyleWindow : Window
    {
        public TestListViewStyleWindow()
        {
            InitializeComponent();
            // 获取数据源
            MelphiDataSource = MelphiDataService.GetDataSource();
        }

        private ObservableCollection<MelphiDataItem> melphiDataSource = new ObservableCollection<MelphiDataItem>();

        /// <summary>
        /// 数据源
        /// </summary>
        public ObservableCollection<MelphiDataItem> MelphiDataSource
        {
            get
            {
                return melphiDataSource;
            }
            set
            {
                melphiDataSource = value;
            }
        }

        /// <summary>
        /// 数据服务
        /// </summary>
        public class MelphiDataService
        {
            /// <summary>
            /// 模拟获取数据源
            /// </summary>
            /// <returns></returns>
            public static ObservableCollection<MelphiDataItem> GetDataSource()
            {
                ObservableCollection<MelphiDataItem> melphiDatas = new ObservableCollection<MelphiDataItem>();

                melphiDatas.Add(new MelphiDataItem()
                {
                    Name = "DataA",
                    IsEnabled = true,
                    SelectedValue = "Kea",
                    SelectionSource = new List<string>() { "Kea", "Lau", "Nuh" }
                });

                melphiDatas.Add(new MelphiDataItem()
                {
                    Name = "DataB",
                    IsEnabled = true,
                    SelectedValue = "Lau",
                    SelectionSource = new List<string>() { "Kea", "Lau", "Nuh" }
                });

                melphiDatas.Add(new MelphiDataItem()
                {
                    Name = "DataC",
                    IsEnabled = true,
                    SelectedValue = "invalid",
                    SelectionSource = new List<string>() { "invalid", "valid" }
                });

                melphiDatas.Add(new MelphiDataItem()
                {
                    Name = "DataD",
                    IsEnabled = true,
                    SelectedValue = "invalid",
                    SelectionSource = new List<string>() { "invalid", "valid" }
                });

                melphiDatas.Add(new MelphiDataItem()
                {
                    Name = "DataE",
                    IsEnabled = false,
                    SelectedValue = "0",
                    SelectionSource = new List<string>() { "0", "1", "2", "3", "4", "5", "6" }
                });

                var listsource = new List<string>();
                for (int i = 0; i <= 200; i += 20)
                {
                    listsource.Add(i.ToString());
                }

                melphiDatas.Add(new MelphiDataItem()
                {
                    Name = "DataF",
                    IsEnabled = true,
                    SelectedValue = "100",
                    SelectionSource = listsource
                });

                melphiDatas.Add(new MelphiDataItem()
                {
                    Name = "DataG",
                    IsEnabled = true,
                    SelectedValue = "3",
                    SelectionSource = new List<string>() { "0", "1", "2", "3", "4", "5", "6" }
                });

                return melphiDatas;
            }
        }
    }

    /// <summary>
    /// 数据项
    /// </summary>
    public class MelphiDataItem
    {
        /// <summary>
        /// 数据名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据标识
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 设定值
        /// </summary>
        public string SelectedValue { get; set; }

        /// <summary>
        /// 设定值选定项集合
        /// </summary>
        public List<string> SelectionSource { get; set; }

    }
}
