using System;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.XPath;
using WebActivatorEx;
using Swashbuckle.Application;
using WebApplication1;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApplication1
{
    public class SwaggerConfig
    {
        private static  string GetXmlCommentsPath()
        {
            return String.Format(
                @"{0}\App_Data\XmlDocument.xml",
                AppDomain.CurrentDomain.BaseDirectory);
        }
        
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1.0.1", "My API");

                        c.GroupActionsBy(apiDesc =>
                        {
                            var controllerName = apiDesc.ActionDescriptor.ControllerDescriptor.ControllerName;
                            var controllerType = apiDesc.ActionDescriptor.ControllerDescriptor.ControllerType.ToString();
                            var member = XDocument.Load(GetXmlCommentsPath()).Root?.XPathSelectElement($"/doc/members/member[@name=\"T:{controllerType}\"]");
                            return $"{controllerName} : {member?.XPathSelectElement("summary")?.Value}";
                        });
                        c.IncludeXmlComments(GetXmlCommentsPath());
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("My Swagger UI");
                        c.DocExpansion(DocExpansion.List);
                        // 預設展開 API
                    });
        }
    }
}
