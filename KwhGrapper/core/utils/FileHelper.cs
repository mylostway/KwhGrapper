using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwhGrapper.core
{
    public class FileHelper
    {
        /// <summary>
        /// 简单读取文件内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string ReadFileContent(string path)
        {
            return ReadFileContent(path, Encoding.UTF8);
        }

        /// <summary>
        /// 简单读取文件内容·
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">指定编码</param>
        /// <returns></returns>
        public static string ReadFileContent(string path, Encoding encoding)
        {
            SAssert.MustTrue(!string.IsNullOrEmpty(path), string.Format("要读取的文件路径不能为空"));

            PathHelper.MustBeNotUpPath(path);

            SAssert.MustTrue(File.Exists(path), string.Format("文件路径{0}不存在!", path));

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
