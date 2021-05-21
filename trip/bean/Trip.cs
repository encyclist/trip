using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using ToolsCommon;

namespace trip.bean
{
    public class Trip: INotifyPropertyChanged
    {
        // 背景颜色
        public string BackgroundColor { get; private set; }
        // 文字颜色
        public string ForegroundColor { get; private set; }
        // 位置-左
        public double Left { get; private set; }
        // 位置-上
        public double Top { get; private set; }
        // 是否置顶
        public bool Topmost { get; private set; }
        // 宽
        private double Width;
        // 高
        private double Height;

        // 文字内容
        public string Content { get; private set; }

        // 配置文件路径
        public string CongigFilePath { get; private set; }
        // 内容文件路径
        public string ContentFilePath { get; private set; }
        // 历史记录文件路径
        public string HistoryFilePath { get; private set; }

        // 配置文件操作工具
        public IniFile IniFile { get; private set; }

        // 创建时间
        public string CreateTime { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Trip()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            CreateTime = ((long)ts.TotalMilliseconds).ToString();

            Init();
        }
        public Trip(string time)
        {
            CreateTime = time;
            Init();
        }

        private void Init()
        {
            string appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            CongigFilePath = appPath + @"\congig-" + CreateTime + ".ini";
            ContentFilePath = appPath + @"\content-" + CreateTime + ".txt"; ;
            HistoryFilePath = appPath + @"\history-" + CreateTime + ".txt"; ;

            IniFile = new IniFile(CongigFilePath);
            ReadConfig();
            ReadContent();
        }

        public void ReLoad()
        {
            ReadConfig();
            ReadContent();
            OnPropertyChanged(new PropertyChangedEventArgs(""));
        }

        // 读取配置文件
        private void ReadConfig() 
        {
            string leftinfos = IniFile.IniReadValue(CreateTime, "Left");
            if (leftinfos == null || leftinfos.Length < 1) leftinfos = "100";
            Left = Double.Parse(leftinfos);

            string topinfos = IniFile.IniReadValue(CreateTime, "Top");
            if (topinfos == null || topinfos.Length < 1) topinfos = "100";
            Top = Double.Parse(topinfos);

            string widthinfos = IniFile.IniReadValue(CreateTime, "Width");
            if (widthinfos == null || widthinfos.Length < 1) widthinfos = "300";
            Width = Double.Parse(widthinfos);

            string heightinfos = IniFile.IniReadValue(CreateTime, "Height");
            if (heightinfos == null || heightinfos.Length < 1) heightinfos = "300";
            Width = Double.Parse(heightinfos);

            string cinfo = IniFile.IniReadValue(CreateTime, "TBackgroundColorop");
            if (cinfo == null || cinfo.Length < 1) cinfo = "#FF1EDCCB";
            BackgroundColor = cinfo;

            string cinfo2 = IniFile.IniReadValue(CreateTime, "ForegroundColor");
            if (cinfo2 == null || cinfo2.Length < 1) cinfo2 = "#FF000000";
            ForegroundColor = cinfo2;

            string fixedTop = IniFile.IniReadValue(CreateTime, "Topmost");
            if (fixedTop == null || fixedTop.Length < 1) fixedTop = "False";
            Topmost = Boolean.Parse(fixedTop);
        }

        // 读取内容
        private void ReadContent()
        {
            FileStream fs = new FileStream(ContentFilePath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            Content = sr.ReadToEnd();
            if (Content == null || Content.Length < 1) 
            {
                Content = "trip";
            }

            sr.Close();
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
