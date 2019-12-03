using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using Microsoft.Win32;

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
        private Dictionary<int, string> mConclutionDictionary;
        private Dictionary<int, string> mSubTypeDictionary;

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
            mClient = new Baidu.Aip.ContentCensor.ImageCensor(API_KEY, SECRET_KEY);
            mClient.Timeout = 60000; // 修改超时时间
        }

        void initDict()
        {
            mConclutionDictionary = new Dictionary<int, string>();
            mConclutionDictionary.Add(0, "百度官方违禁图库");
            mConclutionDictionary.Add(1, "色情识别");
            mConclutionDictionary.Add(2, "暴恐识别");
            mConclutionDictionary.Add(3, "恶心图识别");
            mConclutionDictionary.Add(4, "广告监测");
            mConclutionDictionary.Add(5, "政治敏感识别");
            mConclutionDictionary.Add(6, "图像质量检测");
            mConclutionDictionary.Add(7, "用户图像黑名单");
            mConclutionDictionary.Add(8, "用户图像白名单");
            mConclutionDictionary.Add(9, "图文审核");
            mConclutionDictionary.Add(11, "百度官方违禁词库");
            mConclutionDictionary.Add(12, "文本反作弊");

            mSubTypeDictionary = new Dictionary<int, string>();
            mSubTypeDictionary.Add(3,"爆炸火灾");

        }

        private void button_file_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "选择图像识别文件";
                //fdlg.InitialDirectory = @"c:\";
                fdlg.Filter = "图片文件(*.jpg,*.jpeg,*.png,*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

                fdlg.FilterIndex = 0;
                fdlg.RestoreDirectory = false;
                if (fdlg.ShowDialog() == true)
                {
                    m_imgPath = fdlg.FileName;
                }

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(m_imgPath);
                bi.EndInit();
                image.Source = bi;
                image.InvalidateVisual();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "错误");
            }
        }

         private void authPic()
        {
            this.Dispatcher.BeginInvoke(new Action((() =>
            {
                try
                {
                    var image = File.ReadAllBytes(m_imgPath);
                    var result = mClient.UserDefined(image);
                    var resList = new List<string>();
                    var resConclusion = (string)result["conclusion"];
                    resList.Add(resConclusion);
                    resList.Add("-------------------------------------");
                    if (resConclusion == "不合规")
                    {
                        foreach (var eachHit in result["data"])
                        {
                            var resType = (int)eachHit["type"];
                            var TypeStr = mConclutionDictionary[resType];
                            var subTypeStr = (string)eachHit["msg"];
                            resList.Add(string.Format("{0}不合规", subTypeStr));
                            resList.Add(string.Format("命中模型：{0}", TypeStr));
                            var stars = eachHit["stars"];
                            if (stars != null)
                            {
                                foreach (var star in stars)
                                {
                                    resList.Add(string.Format("敏感人物：{0}",star["name"]));
                                }
                            }
                            resList.Add("\n");
                        }
                    }
                    textBox_Auth.Text = string.Join("\n",resList);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "错误");
                }
                


            })));
        }

        private void button_Auth_Click(object sender, RoutedEventArgs e)
        {
            var task = new Thread(authPic);
            task.Start();
        }
    }
}
