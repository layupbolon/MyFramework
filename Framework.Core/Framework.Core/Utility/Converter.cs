using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework.Core.Utility
{
    public class Converter
    {
        //public static T To<T>(string str, T def)
        //{
        //    return def;
        //}

        public static string ToString(object obj, string def = null)
        {
            if (obj == null)
            {
                return def;
            }
            return obj.ToString();
        }

        public static List<string> ToStringList(string str, string separ = ",")
        {
            if (str == null)
            {
                return new List<string>();
            }
            return str.Split(new string[] { separ }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static int ToInt(object obj, int def = 0)
        {
            return ToInt(obj == null ? null : obj.ToString(), def);
        }
        public static int ToInt(string str, int def = 0)
        {
            int res;
            if (string.IsNullOrEmpty(str)
                || !int.TryParse(str, out res))
            {
                res = def;
            }
            return res;
        }

        public static List<int> ToIntList(string str, string separ = ",")
        {
            if (str == null)
            {
                return new List<int>();
            }
            var sl = ToStringList(str, separ);

            return sl.Select(x => ToInt(x)).ToList();
        }

        public static bool ToBoolean(string str, bool def = false)
        {
            bool res;
            if (string.IsNullOrEmpty(str)
                || !bool.TryParse(str, out res))
            {
                res = def;
            }
            return res;
        }

        public static bool ToBoolean(object obj, bool def = false)
        {
            if (obj == null)
            {
                return false;
            }
            return ToBoolean(obj.ToString(), def);
        }

        public static decimal ToDecimal(string str, int def = 0)
        {
            decimal res;
            if (string.IsNullOrEmpty(str)
                || !decimal.TryParse(str, out res))
            {
                res = def;
            }
            return res;
        }

        public static Nullable<DateTime> ToDateTime(string str, Nullable<DateTime> def = null)
        {
            DateTime res;
            if (string.IsNullOrEmpty(str)
                || !DateTime.TryParse(str, out res))
            {
                return def;
            }
            return res;
        }

        /// <summary>
        /// 获取单个汉字的首写拼音字母
        /// </summary>
        /// <param name="cn"></param>
        /// <returns></returns>
        public static string GetSpell(char cn)
        {
            var res = cn.ToString();
            if (Regex.IsMatch(res, @"[\u4E00-\u9FA5]"))
            {
                ChineseChar china = new ChineseChar(cn);
                res = china.Pinyins.First();
            }
            return res;
        }

        /// <summary>
        /// 获取汉字字符串的首写英文字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStringSpell(string str)
        {
            char[] chars = str.ToCharArray();

            Regex reg = new Regex(@"[\u4E00-\u9FA5]*[0-9]*[a-z]*[A-Z]*");
            var list = reg.Matches(str);

            var china = string.Empty;
            foreach (Match item in list)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    for (int i = 0; i < item.Value.Length; i++)
                    {
                        china += GetSpell(item.Value[i])[0];
                    }
                }
            }

            return china.ToUpper();
        }
    }
}
