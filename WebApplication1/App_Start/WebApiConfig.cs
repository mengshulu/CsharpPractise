using System;
using System.Web;
using System.Web.Http;
using WebApplication1.Helpers;
using System.Web.Http.Cors;

namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 啟用 CORS，允許所有來源請求
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            
            // 啟用屬性路由
            config.MapHttpAttributeRoutes();
            
            // 預設路由：api/{controller}/{id}
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // 啟用 JSON 回應格式
            // var formatter = config.Formatters.JsonFormatter;
            // // config.Services.Replace(typeof(System.Net.Http.Formatting.IContentNegotiator), new JsonContentNegotiator(formatter));
            // var isSwaggerRequest = HttpContext.Current?.Request.Url.AbsolutePath.Contains("/swagger") ?? false;
            //
            // if (!isSwaggerRequest)
            // {
            //     config.Services.Replace(typeof(System.Net.Http.Formatting.IContentNegotiator), new JsonContentNegotiator(formatter));
            // }

            config.EnsureInitialized();
        }
    }
}

