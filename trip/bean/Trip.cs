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

        // 文字内容
        public string Content { get; private set; }

        // 内容文件路径
        public string ContentFilePath { get; private set; }
        // 历史记录文件路径
        public string HistoryFilePath { get; private set; }

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

            ContentFilePath = appPath + @"\content-" + CreateTime + ".txt"; ;
            HistoryFilePath = appPath + @"\history-" + CreateTime + ".txt"; ;

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
            string leftinfos = IniFile.GetInstance().IniReadValue(CreateTime, "Left");
            if (leftinfos == null || leftinfos.Length < 1) leftinfos = "100";
            Left = Double.Parse(leftinfos);

            string topinfos = IniFile.GetInstance().IniReadValue(CreateTime, "Top");
            if (topinfos == null || topinfos.Length < 1) topinfos = "100";
            Top = Double.Parse(topinfos);

            string cinfo = IniFile.GetInstance().IniReadValue(CreateTime, "TBackgroundColorop");
            if (cinfo == null || cinfo.Length < 1) cinfo = "#FF1EDCCB";
            BackgroundColor = cinfo;

            string cinfo2 = IniFile.GetInstance().IniReadValue(CreateTime, "ForegroundColor");
            if (cinfo2 == null || cinfo2.Length < 1) cinfo2 = "#FF000000";
            ForegroundColor = cinfo2;

            string fixedTop = IniFile.GetInstance().IniReadValue(CreateTime, "Topmost");
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
