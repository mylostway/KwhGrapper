using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwhGrapper.core
{
    /// <summary>
    /// 我的qq邮箱授权邮件客户端
    /// </summary>
    public class MyMailHelper : MailHelper
    {
        static MyMailHelper() { }

        private MyMailHelper()
            :base(ACCOUNT, AUTH_KEY, HOST)
        {

        }

        private const string ACCOUNT = "359050096@qq.com";

        private const string AUTH_KEY = "mqhxwxqowzzfbghi";

        private const string HOST = "smtp.qq.com";

        public static MyMailHelper Instance = new MyMailHelper();
    }
}
