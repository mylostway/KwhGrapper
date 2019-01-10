using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwhGrapper.core
{
    public class PathHelper
    {
        /// <summary>
        /// 路径往上一级的标记
        /// </summary>
        const string UP_PATH_TAG = "../";

        /// <summary>
        /// 传入必须为文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileName(string path)
        {
            return (!string.IsNullOrWhiteSpace(path) && Path.GetFileName(path) == path);
        }


        /// <summary>
        /// 文件路径不能超越当前级别（非空字符串）
        /// </summary>
        /// <returns></returns>
        public static bool IsNotUpPath(string path)
        {
            return (!string.IsNullOrWhiteSpace(path) && path.IndexOf(UP_PATH_TAG) < 0);
        }

        /// <summary>
        /// 文件路径不能超越当前级别（非空字符串）
        /// </summary>
        /// <param name="path"></param>
        public static void MustBeNotUpPath(string path)
        {
            SAssert.MustTrue(IsNotUpPath(path), string.Format("非法文件路径:{0}", path));
        }
    }
}
