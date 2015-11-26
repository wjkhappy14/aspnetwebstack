using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost
{
    class Program
    {

        static void Main(string[] args)
        {

            var baseAddress = "http://localhost:8080";
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(baseAddress);

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //全局允许CROS
            // config.EnableCors();//启用跨域


            config.Routes.MapHttpRoute
           (
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );

            config.MaxConcurrentRequests = 1000;
          
            HttpSelfHostServer server = new HttpSelfHostServer(config);
            server.OpenAsync();



            Console.WriteLine("Server  http://localhost:8080   Open now ......");
           


            config.EnsureInitialized();
            Console.ReadLine();

        }
    }
}
