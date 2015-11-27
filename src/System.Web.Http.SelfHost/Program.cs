using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using System.Web.Http.Owin;
using System.Web.Http.Routing;

namespace System.Web.Http.SelfHost
{
    class Program
    {

        static void Main(string[] args)
        {

            var baseAddress = "http://localhost:8080";
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(baseAddress);
            config.MessageHandlers.Add(new ProgressMessageHandler() { });


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
            var handlers = new DelegatingHandler[] { new PassiveAuthenticationMessageHandler(), new HttpServer() };
            var routeHandlers = HttpClientFactory.CreatePipeline(new HttpControllerDispatcher(config), handlers);
            config.Routes.MapHttpRoute(
                   name: "CustomerRouter",
                  routeTemplate: "MyAPI/{Controller}/{Action}/Id",
                  defaults: new { Id = RouteParameter.Optional },
                  constraints: null,
                  handler: routeHandlers
                );
            HttpSelfHostServer server = new HttpSelfHostServer(config);
            server.OpenAsync();
            Console.WriteLine("Server  http://localhost:8080   Open now ....at {0}..", server.Configuration.VirtualPathRoot);
            config.EnsureInitialized();
            foreach (var route in config.Routes)
            {
                System.Diagnostics.Debug.WriteLine(route);
            }

            Console.ReadLine();

        }
    }
}
