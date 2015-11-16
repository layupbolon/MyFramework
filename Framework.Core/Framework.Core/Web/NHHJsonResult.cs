using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Framework.Core.Web
{

    /// <summary>
    ///  JSON 格式内容响应对象
    /// </summary>
    public class NJsonResult : JsonResult
    {

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            //if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
            //    String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new InvalidOperationException(MvcResources.JsonRequest_GetNotAllowed);
            //}

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var rlt = JsonConvert.SerializeObject(this.Data, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        DateFormatString = "yyyy-MM-dd HH:mm:ss",
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling= DefaultValueHandling.Ignore
                    });
                response.Write(rlt);
            }
        }
    }

}
