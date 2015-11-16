using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.IO
{
    /// <summary>
    /// 分布式文件存储目录辅助类
    /// </summary>
    public class DFISHelper
    {
        /// <summary>
        /// 根据文件名称计算分布式文件存储目录
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static string GetDFISFolder(string fileName)
        {
            string SavePath = @"{0}\{1}";
            fileName = fileName.ToLower();
            return string.Format(SavePath, GetSingleCharacter(fileName), GetCoupleInteger(fileName));
        }
        /// <summary>
        /// 根据文件名称计算分布式文件存储目录（包括文件名）
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static string GetDFISFileName(string fileName)
        {
            string SavePath = @"{0}\{1}\{2}";
            fileName = fileName.ToLower();
            return string.Format(SavePath, GetSingleCharacter(fileName), GetCoupleInteger(fileName),fileName);
        }

        private static string GetSingleCharacter(string key)
        {
            //Format to A-Z
            return ((char)('A' + GetCharToIntSum(key, false) % 26)).ToString();
        }

        private static string GetCoupleInteger(string key)
        {
            //Format to 00-99
            return (GetCharToIntSum(key, true) % 100).ToString("00");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="isOdd">是否只取奇数</param>
        /// <returns></returns>
        private static int GetCharToIntSum(string chars, bool isOdd)
        {
            int sumAsciiValue = 0;
            if (!string.IsNullOrEmpty(chars))
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    char fileNameChar = chars[i];
                    if (isOdd)
                    {
                        if (i % 2 != 0) sumAsciiValue += fileNameChar * i;
                    }
                    else
                    {
                        sumAsciiValue += fileNameChar;
                    }
                }
            }

            return sumAsciiValue;

        }
    }
}
