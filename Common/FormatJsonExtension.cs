using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace BlossomWeb.Common
{
    public static class FormatJsonExtension
    {
        public static FormatJsonResult JsonFormat(this Controller c, object data)
        {
            var result = new FormatJsonResult();
            result.NotLigerUIFriendlySerialize = true;
            result.Data = data;
            return result;
        }

    }

    /// <summary>
    ///     JsonResult格式化扩展
    /// </summary>
    public class FormatJsonResult : ActionResult
    {
        /// <summary>
        ///     是否产生错误
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        ///     错误信息，或者成功信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     成功可能时返回的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        ///     正常序列化方式(为True则不进行UI友好的序列化)
        /// </summary>
        public bool NotLigerUIFriendlySerialize { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";

            var sw = new StringWriter();
            JsonSerializer serializer =
                JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, !NotLigerUIFriendlySerialize ? this : Data);
            }

            response.Write(sw.ToString());
        }
    }

    public class EasyUIGrid
    {
        /// <summary>
        ///     返回的记录
        /// </summary>
        public object rows { get; set; }

        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        ///     Gets or sets the jobnums.
        /// </summary>
        public object jobnums { get; set; }

        /// <summary>
        ///     Gets or sets the wattypes.
        /// </summary>
        public object wattypes { get; set; }

        /// <summary>
        ///     总个数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(
                this, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, });
        }
    }
}
