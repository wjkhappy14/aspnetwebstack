using System;
using System.Globalization;
using System.Web;
using System.Threading;
using System.Web.Routing;
using System.Diagnostics;

namespace System.Web.Mvc.MyHttpModules
{
    /// <summary>
    /// 实现多语言的切换， 设计参考  http://www.cnblogs.com/artech/archive/2012/05/04/localization-via-url-routing.html
    /// Created By: Angkor.wu
    /// Created On：2016-02-29
    /// </summary>
    public class MultiCultureAwareHttpModule : IHttpModule//ApplicationStartModuleBase
    {
        private static Stopwatch stopwatch = Stopwatch.StartNew();
        private CultureInfo currentCulture;
        private CultureInfo currentUICulture;
        /// <summary>
        /// 您将需要在网站的 Web.config 文件中配置此模块
        /// 并向 IIS 注册它，然后才能使用它。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //此处放置清除代码。
        }

        public void Init(HttpApplication context)
        {
            // 下面是如何处理 LogRequest 事件并为其 
            // 提供自定义日志记录实现的示例

            context.BeginRequest += SetCurrentCulture;
            context.EndRequest += RecoverCulture; ;
            //context.LogRequest += new EventHandler(OnLogRequest);
        }



        private void SetCurrentCulture(object sender, EventArgs args)
        {

            currentCulture = Thread.CurrentThread.CurrentCulture;
            currentUICulture = Thread.CurrentThread.CurrentUICulture;


            HttpContextBase contextWrapper = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = RouteTable.Routes.GetRouteData(contextWrapper);
            object culture;
            if (routeData.Values.TryGetValue("culture", out culture))
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(culture.ToString());
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                }
                catch (Exception ex)
                {
                    Debug.Write(ex);
                }
                finally
                {


                }
            }
            else
            {

                // 如果没有指定，设置默认区域文化
                Thread.CurrentThread.CurrentCulture = currentCulture;
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
        }

        private void RecoverCulture(object sender, EventArgs args)
        {
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentUICulture;

        }

     //  public override void OnInit(HttpApplication context)
     //  {
     //      base.OnInit(context);
     //      Debug.Write("OnInit:ApplicationStartModuleBase   ");
     //  }
     //
     //  public override void OnStart(HttpApplication context)
     //  {
     //      Debug.Write("OnStart:ApplicationStartModuleBase   ");
     //      base.OnStart(context);
     //  }
        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //自定义日志记录逻辑
            Debug.Write(stopwatch.Elapsed);
            stopwatch.Stop();
        }
    }
}
