using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AICourseCSharpL6
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IniFile mConfig;
        private string m_imgPath = "";
        private Baidu.Aip.ContentCensor.ImageCensor mClient;
        private Dictionary<int, string> mDictionary;

        public MainWindow()
        {
            InitializeComponent();
            init();
            initDict();
        }

        void init()
        {
            mConfig = new IniFile("./cfg.ini");
            var APP_ID = mConfig.IniReadValue("Baidu", "APP_ID").ToString();
            var API_KEY = mConfig.IniReadValue("Baidu", "API_KEY").ToString();
            ;
            var SECRET_KEY = mConfig.IniReadValue("Baidu", "SECRET_KEY").ToString();
            Console.WriteLine(API_KEY);
            Console.WriteLine(SECRET_KEY);
            mClient = new Baidu.Aip.ContentCensor.ImageCensor(API_KEY,SECRET_KEY);
            mClient.Timeout = 60000; // 修改超时时间
        }

        void initDict()
        {
            mDictionary = new Dictionary<int, string>();
            mDictionary.Add(0, "百度官方违禁图库");
            mDictionary.Add(1, "色情识别");
            mDictionary.Add(2, "暴恐识别");
            mDictionary.Add(3, "恶心图识别");
            mDictionary.Add(4, "广告监测");
            mDictionary.Add(5, "政治敏感识别");
            mDictionary.Add(6, "图像质量检测");
            mDictionary.Add(7, "用户图像黑名单");
            mDictionary.Add(8, "用户图像白名单");
            mDictionary.Add(9, "图文审核");
            mDictionary.Add(11, "百度官方违禁词库");
            mDictionary.Add(12,"文本反作弊");

        }





    }
}
