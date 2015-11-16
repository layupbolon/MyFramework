using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Configuration;

namespace Framework.Core.Utility
{
    public class ParamManager
    {
        /// <summary>
        /// 获取指定名称的String参数值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        public static string GetStringValue(string name,string def="")
        {

            if (string.IsNullOrEmpty(name))
            {
                return def;
            }

            var param = ConfigManager.GetConfig<ParamConfig>();
            if (param == null
                || param.ParamItems == null)
            {
                return def;
            }

            string value = def;
            var item = param.ParamItems.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
         
            return item==null?def:item.Content;
        }

        /// <summary>
        /// 获取指定名称的Boolean参数值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static bool GetBoolValue(string name, bool def = false)
        {
            var val = GetStringValue(name);
            return string.IsNullOrEmpty(val) ? def : val.Equals("TRUE", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取指定名称的Integer参数值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static int GetIntValue(string name, int def = 0)
        {
            int i;
            var val = GetStringValue(name);
            return int.TryParse(val,out i) ? i : def;
        }
    }
}
