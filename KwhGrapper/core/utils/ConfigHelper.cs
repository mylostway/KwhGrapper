using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KwhGrapper.core
{
    /// <summary>
    /// 配置助手（目前只支持json）
    /// </summary>
    public class ConfigHelper
    {
        private const String DEFAULT_CFG_FILE_NAME = "appsettings.json";

        /// <summary>
        /// 配置缓存
        /// </summary>
        private static Dictionary<string, IConfiguration> ConfigurationDic
            = new Dictionary<string, IConfiguration>();

        /// <summary>
        /// 配置内容缓存
        /// </summary>
        private static Dictionary<string, string> ConfigurationContentDic = new Dictionary<string, string>();

        /// <summary>
        /// 初始化配置（如果没读入缓存，则读入，否则不做任何事情）
        /// </summary>
        /// <param name="cfgFileName"></param>
        /// <returns>最终读取的配置文件路径</returns>
        private static string InitCfg(string cfgFileName)
        {
            PathHelper.MustBeNotUpPath(cfgFileName);

            var basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            // 只能传入配置文件子路径，不能传入非法路径
            string cfgFilePath = string.Format("{0}/{1}", basePath, cfgFileName);

            if (!ConfigurationDic.ContainsKey(cfgFileName))
            {
                SAssert.MustTrue(File.Exists(cfgFilePath), string.Format("配置文件:{0}不存在", cfgFilePath));

                var configuartion = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile(cfgFileName).Build();

                ConfigurationDic.Add(cfgFileName, configuartion);

                var cfgContent = FileHelper.ReadFileContent(cfgFilePath);

                ConfigurationContentDic.Add(cfgFileName, cfgContent);
            }

            return cfgFilePath;
        }

        /// <summary>
        /// 转换配置文件到json实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cfgFileName"></param>
        /// <returns></returns>
        public static T Get<T>(string cfgFileName = "")
            where T : class
        {
            if (string.IsNullOrEmpty(cfgFileName))
            {
                var type = typeof(T);

                var attributes = type.GetCustomAttributes(typeof(ConfigAttribute), true);

                if (null == attributes || 0 == attributes.Length)
                {
                    cfgFileName = string.Format("{0}.json", type.Name);
                }
                else
                {
                    var attr = attributes[0] as ConfigAttribute;

                    if (string.IsNullOrEmpty(attr.FileRelativePath)) cfgFileName = string.Format("{0}.json", type.Name);
                    else cfgFileName = attr.FileRelativePath;
                }
            }

            var cfgPath = InitCfg(cfgFileName);

            var cfgContent = ConfigurationContentDic[cfgFileName];

            SAssert.MustTrue(!string.IsNullOrEmpty(cfgContent), string.Format("配置{0}内容为空！", cfgPath));

            var retObj = JsonHelper.DeserializeTo<T>(cfgContent);

            return retObj;
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetOnKey(string key, string cfgFileName = DEFAULT_CFG_FILE_NAME)
        {
            var cfgPath = InitCfg(cfgFileName);

            var cfg = ConfigurationDic[cfgFileName];

            return cfg[key];
        }
    }
}
