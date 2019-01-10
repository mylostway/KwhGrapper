using KwhGrapper.core;
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
using System.Windows.Threading;

namespace KwhGrapper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitLogic();
        }

        public void InitLogic()
        {
            MyCrawler.Instance.CrawlerDoneHandler += Instance_CrawlerDoneHandler;
            MyCrawler.Instance.MailSentCallBackHandler += Instance_MailSentCallBackHandler;
            MyCrawler.Instance.CrawlerStopHandler += Instance_CrawlerStopHandler;
            MyCrawler.Instance.CrawlerErrHandler += Instance_CrawlerErrHandler;

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            MyCrawler.Instance.Stop();
        }

        private void LogToUI(string msg)
        {
            this.tbx_log.AppendText($"{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")} | {msg}\n");
            this.tbx_log.ScrollToEnd();
        }


        private void Instance_CrawlerErrHandler(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new EventHandler((x, args) =>
            {
                var errArg = x as OnErrorEventArgs;
                if (null != errArg)
                {
                    // 爬虫出错
                    this.LogToUI($"爬虫出错：{errArg.Exception.Message}");

                    SLogger.Instance.LogErr(errArg.Exception.Message, errArg.Exception);
                }
            }), sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isStart"></param>
        private void SetProcRunState(bool isStart = true)
        {
            btn_start.IsEnabled = !isStart;
            btn_stop.IsEnabled = isStart;
            lb_state.Content = isStart ? "运行中" : "已停止";
        }

        private void Instance_CrawlerStopHandler(object sender, EventArgs e)
        {
            SetProcRunState(false);
            this.LogToUI($"爬虫已停止,{sender.ToString()}");
        }

        private void Instance_MailSentCallBackHandler(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new EventHandler((x, args) =>
            {
                var sndMailBody = x.ToString();

                this.LogToUI("申请邮件已发送...");

                SLogger.Instance.LogInfo(string.Format($"申请邮件已发送...\n{sndMailBody}\n"));
            }), sender, null);
        }

        static DateTime dtLast = DateTime.Now.AddMinutes(-10);
        static long doneCounter = 0;

        private void Instance_CrawlerDoneHandler(object sender, EventArgs e)
        {
            doneCounter++;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new EventHandler((x, args) =>
            {
                var complateArg = x as OnCompletedEventArgs;
                if (null != complateArg)
                {
                    if((DateTime.Now - dtLast).TotalMinutes >= 10)
                    {
                        // 爬虫成功
                        this.LogToUI($"服务运行正常，当前总抓取次数：{doneCounter}...");
                        dtLast = DateTime.Now;
                    }
                }
            }), sender, e);
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            SetProcRunState(true);
            //MyCrawler.Instance.StartInLoop();                       
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new EventHandler((x, args) => {
                MyCrawler.Instance.StartInLoop();
            }), null,null);
            
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            SetProcRunState(false);
        }
    }
}
