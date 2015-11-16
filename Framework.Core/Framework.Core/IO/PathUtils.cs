using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework.Core.IO
{
    /// <summary>
    /// 目录辅助类
    /// </summary>
    public class PathUtils
    {
        #region IsFullPath
        // 磁盘目录模式 (格式：D:\\)
        private static readonly Regex PathDirRegex = new Regex(@"^[A-Z]:(\\{1,2}([^\\/:\*\?<>\|]*))*$", RegexOptions.IgnoreCase);
        // 磁盘共享目录模式(格式：\\MyComputer)
        private static readonly Regex PathSharedRegex = new Regex(@"^(\\{1,2}([^\\/:\*\?<>\|]*))*$", RegexOptions.IgnoreCase);
        /// <summary>
        /// 文件夹目录是否完整
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return PathDirRegex.IsMatch(path) || PathSharedRegex.IsMatch(path);
        }
        #endregion

        #region IsFileFullPath
        // 磁盘目录文件模式 (格式：D:\\Test.gif)
        private static readonly Regex FileDirRegex = new Regex(@"^[A-Z]:\\{1,2}(([^\\/:\*\?<>\|]+)\\{1,2})+([^\\/:\*\?<>\|]+)(\.[A-Z]+)$", RegexOptions.IgnoreCase);
        // 磁盘共享文件模式(格式：\\MyComputer\Test.gif)
        private static readonly Regex FileSharedRegex = new Regex(@"^\\{2}(([^\\/:\*\?<>\|]+)\\{1,2})+([^\\/:\*\?<>\|]+)(\.[A-Z]+)$", RegexOptions.IgnoreCase);
        /// <summary>
        /// 文件目录是否完整
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return FileDirRegex.IsMatch(path) || FileSharedRegex.IsMatch(path);
        }
        #endregion

        #region GetFullPath
        public static string GetFullPath(string fullPathOrRelativePath)
        {
            string path = fullPathOrRelativePath;

            if (!IsFullPath(path))
            {
                if (fullPathOrRelativePath.StartsWith("~"))
                    path = path.TrimStart('~');

                path = string.Format("{0}\\{1}",
                    AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/'),
                    path.TrimStart('\\', '/').Replace("/", "\\"));
            }

            return path;
        }
        #endregion

        private static readonly List<string> ImageTypes = new List<string>() { ".jpg", ".png", ".bmp", ".gif", ".jpeg", ".tif", ".ico", ".tif" };
        /// <summary>
        /// 判断指定文件名是否属于.jpg|.png|.bmp|.gif|.jpeg|.tif|.ico|.tif等格式的图片文件的一种
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns></returns>
        public static bool IsImageFile(string file)
        {
            if (string.IsNullOrEmpty(file))
                return false;

            return IsImageExt(Path.GetExtension(file));
        }

        /// <summary>
        /// 判断指定扩展是否属于.jpg|.png|.bmp|.gif|.jpeg|.tif|.ico|.tif等格式的图片文件的一种
        /// </summary>
        /// <param name="ext">扩展名</param>
        /// <returns></returns>
        public static bool IsImageExt(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return false;

            return ImageTypes.Contains(ext.ToLower());
        }

        private static readonly List<string> FileExts = new List<string>() { 
            ".jpg", ".png", ".bmp", ".gif", ".jpeg", ".tif", ".ico", ".tif",
            ".doc", ".xls", ".docx", ".xlsx", ".ppt", ".pptx", ".txt", ".zip", ".rar", ".tiff"
        };
        /// <summary>
        /// 判断指定文件名是否属于.jpg|.png|.bmp|.gif|.jpeg|.tif|.ico|.tif等格式的图片文件的一种
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns></returns>
        public static bool IsFile(string file)
        {
            if (string.IsNullOrEmpty(file))
                return false;

            return IsFileExt(Path.GetExtension(file));
        }

        /// <summary>
        /// 判断指定扩展是否属于.jpg|.png|.bmp|.gif|.jpeg|.tif|.ico|.tif等格式的图片文件的一种
        /// </summary>
        /// <param name="ext">扩展名</param>
        /// <returns></returns>
        public static bool IsFileExt(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return false;

            return FileExts.Contains(ext.ToLower());
        }

    }
}
