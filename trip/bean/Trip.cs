using System;
using System.Diagnostics;
using System.IO;
using ToolsCommon;

namespace trip.bean
{
    public class Trip
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
        public double Width { get; private set; }
        // 高
        public double Height { get; private set; }

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
        public string createTime { get; private set; }

        public Trip()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            createTime = ((long)ts.TotalMilliseconds).ToString();

            Init();
        }
        public Trip(string time)
        {
            createTime = time;
            Init();
        }

        private void Init()
        {
            string appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            CongigFilePath = appPath + @"\congig-" + createTime + ".ini";
            ContentFilePath = appPath + @"\content-" + createTime + ".txt"; ;
            HistoryFilePath = appPath + @"\history-" + createTime + ".txt"; ;

            IniFile = new IniFile(CongigFilePath);
            ReadConfig();
            ReadContent();
        }

        // 读取配置文件
        private void ReadConfig() 
        {
            string leftinfos = IniFile.IniReadValue(createTime, "Left");
            if (leftinfos == null || leftinfos.Length < 1) leftinfos = "100";
            Left = Double.Parse(leftinfos);

            string topinfos = IniFile.IniReadValue(createTime, "Top");
            if (topinfos == null || topinfos.Length < 1) topinfos = "100";
            Top = Double.Parse(topinfos);

            string widthinfos = IniFile.IniReadValue(createTime, "Width");
            if (widthinfos == null || widthinfos.Length < 1) widthinfos = "300";
            Width = Double.Parse(widthinfos);

            string heightinfos = IniFile.IniReadValue(createTime, "Height");
            if (heightinfos == null || heightinfos.Length < 1) heightinfos = "300";
            Width = Double.Parse(heightinfos);

            string cinfo = IniFile.IniReadValue(createTime, "TBackgroundColorop");
            if (cinfo == null || cinfo.Length < 1) cinfo = "#FF1EDCCB";
            BackgroundColor = cinfo;

            string cinfo2 = IniFile.IniReadValue(createTime, "ForegroundColor");
            if (cinfo2 == null || cinfo2.Length < 1) cinfo2 = "#FF000000";
            ForegroundColor = cinfo2;

            string fixedTop = IniFile.IniReadValue(createTime, "Topmost");
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
    }
}
