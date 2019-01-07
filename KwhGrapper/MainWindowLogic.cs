using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KwhGrapper.core
{
    public class MyCrawler
    {
        private MyCrawler()
        {
            s_crawler = new SimpleCrawler();
            s_crawler.OnCompleted += S_crawler_OnCompleted;
            s_crawler.OnError += S_crawler_OnError;
            s_crawler.OnStart += S_crawler_OnStart;
        }

        static MyCrawler() { }

        public static MyCrawler Instance = new MyCrawler();

        private void S_crawler_OnStart(object sender, OnStartEventArgs e)
        {

        }

        private void S_crawler_OnError(object sender, OnErrorEventArgs e)
        {
            CrawlerDoneHandler?.Invoke(e, null);
        }

        #region 常量

        /// <summary>
        /// 邮件发送源
        /// </summary>
        private const string MAIL_FROM = "359050096@qq.com";

        /// <summary>
        /// 邮件发送到
        /// </summary>
        private const string MAIL_TO = "health@kwh.org.mo";

        /// <summary>
        /// 邮件标题
        /// </summary>
        private const string MAIL_TITLE = "加衛苗注射预约";

        /// <summary>
        /// 关键词
        /// </summary>
        private const string TAG_KEY_WORD = "加衛苗";

        /// <summary>
        /// 爬虫网址
        /// </summary>
        private const string CRAW_URL = "http://www.kwh.org.mo/";

        #endregion



        #region 事件

        /// <summary>
        /// 爬虫每次处理完成之后回调
        /// </summary>
        public event EventHandler CrawlerDoneHandler = null;

        /// <summary>
        /// 每个邮件发送完成之后回调
        /// </summary>
        public event EventHandler MailSentCallBackHandler = null;


        /// <summary>
        /// 爬虫停止回调 
        /// </summary>
        public event EventHandler CrawlerStopHandler = null;

        #endregion


        /// <summary>
        /// 设置发送邮件内容
        /// </summary>
        public string MailBody { get; set; } = "姓名：曾舒婷\n性别：女\n通行证号码：C07933346\n身份证：44068319930406921\n邮箱：359050096@qq.com\n电话：0086-18566050406\n注射日期：4号、5号、6号";

        /// <summary>
        /// 邮件内容文件路径
        /// </summary>
        public string MailBodyFilePath { get; set; } = $"{Directory.GetCurrentDirectory()}/mails/";


        /// <summary>
        /// 发送完成的邮件内容会备份到这里(按时间分文件夹)
        /// </summary>
        public string MailBackupPathPrefix { get; set; } = $"{Directory.GetCurrentDirectory()}/mails/backup";

        /// <summary>
        /// 处理邮件
        /// </summary>
        /// <param name="path"></param>
        public void HandleMails()
        {
            if (string.IsNullOrEmpty(MailBodyFilePath))
            {
                // error
                throw new Exception("邮件文件路径不能为空！");
            }

            if (!Directory.Exists(MailBodyFilePath))
            {
                Directory.CreateDirectory(MailBodyFilePath);
                m_isFinish = true;
                return;
            }

            var files = Directory.GetFiles(MailBodyFilePath);

            if (files.Length == 0)
            {
                // handle over
                m_isFinish = true;
                return;
            }

            foreach (var eFile in files)
            {
                var fName = Path.GetFileName(eFile);
                var extName = Path.GetExtension(eFile);
                if (extName.IndexOf("mail") >= 0)
                {
                    using (var fs = new FileStream(eFile, FileMode.Open))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            var mailBody = sr.ReadToEnd();

                            MailSentCallBackHandler?.Invoke(mailBody, null);

                            if (MyMailHelper.Instance.Send(MAIL_FROM, MAIL_TO, MAIL_TITLE, mailBody))
                            {
                                // 邮件发送完成之后，将该邮件迁移到备份目录，防止多次发送，且自动备份曾经请求内容以便核对
                                var backUpDir = $"{MailBackupPathPrefix}/{DateTime.Now.ToString("yyyyMMdd")}";
                                if (!Directory.Exists(MailBackupPathPrefix))
                                {
                                    Directory.CreateDirectory(backUpDir);
                                }
                                File.Move(eFile, $"{backUpDir}/{fName}.bak");
                            }
                        }
                    }
                }
            }
        }


        public void AnaliseHtml(string strHtml)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(strHtml);
            var nodes = doc.DocumentNode.SelectNodes("//td[@class=\"line_td1\"]");

            foreach (var eNode in nodes)
            {
                var eText = eNode.InnerText;
                if (string.IsNullOrEmpty(eText)) continue;
                if (eText.IndexOf(TAG_KEY_WORD) >= 0)
                {
                    // 找到目标了
                    if (eText.IndexOf("已有") >= 0 || eText.IndexOf("少量") >= 0 || true)
                    {
                        //MyMailHelper.Instance.Send(MAIL_FROM,MAIL_TO, MAIL_TITLE, MailBody);
                        HandleMails();

                        m_isFinish = true;
                    }
                    break;
                }
            }
        }


        private void S_crawler_OnCompleted(object sender, OnCompletedEventArgs e)
        {
            var strHtml = e.PageSource;

            CrawlerDoneHandler?.Invoke(e, null);

            AnaliseHtml(strHtml);
        }

        /// <summary>
        /// 爬虫实例
        /// </summary>
        private SimpleCrawler s_crawler = null;

        /// <summary>
        /// 是否已经完成所有任务，停止运行标志
        /// </summary>
        private bool m_isFinish = false;

        /// <summary>
        /// 开启任务，默认一分钟查一次
        /// </summary>
        /// <param name="inter"></param>
        public void StartInLoop(int inter = 60 * 1000)
        {
            var uri = new Uri(CRAW_URL);

            while (!m_isFinish)
            {
                s_crawler.Start(uri);

                Thread.Sleep(inter);
            }
        }


        /// <summary>
        /// 停止任务
        /// </summary>
        public void Stop()
        {
            m_isFinish = true;
        }
    }
}
