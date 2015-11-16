using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Exceptions
{
    /// <summary>
    /// 业务异常
    /// 用户服务端抛出前端展示
    /// </summary>
    [Serializable]
    public class BusinessException : NHHException
    {
        public BusinessException()
            : base("业务执行异常")
        {
        }

        public BusinessException(string message)
            : base(message)
        {
        }
    }
}
