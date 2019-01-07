using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KwhGrapper
{
    public class MailHelper
    {
        protected MailHelper(string account,string authKey,string host,int port = -1)
        {
            m_account = account;
            m_authKey = authKey;
            m_host = host;
            m_port = port;
        }

        private string m_account = "";

        private string m_authKey = "";

        private string m_host = "";

        private int m_port = -1;

        public bool Send(string from,string to,string subject,string content)
        {
            try
            {
                var client = new SmtpClient();
                client.Host = m_host;
                if (m_port > 0) client.Port = m_port;
                //使用安全加密连接。
                client.EnableSsl = true;
                // 不和请求一块发送。
                client.UseDefaultCredentials = false;
                //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
                client.Credentials = new NetworkCredential(m_account, m_authKey);

                var mailMessage = new MailMessage(from, to, subject, content);
                client.Send(mailMessage);
            }
            catch(Exception ex)
            {
                return false;
            }            
            return true;
        }
    }
}
