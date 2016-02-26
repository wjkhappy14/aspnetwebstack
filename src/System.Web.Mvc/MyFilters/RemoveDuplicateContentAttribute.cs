using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace System.Web.Mvc.MyFilters
{


    /// <summary>
    /// 一个地址对应一个唯一独立的内容
    /// http://www.cnblogs.com/TomXu/archive/2011/12/23/2299368.html
    /// </summary>
    public class RemoveDuplicateContentAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routes = RouteTable.Routes;
            var requestContext = filterContext.RequestContext;

            var routeData = requestContext.RouteData;
            var dataTokens = routeData.DataTokens;
            //判断如果当前没有使用Area的话就为DataToken添加一个空值，这一点非常重要
            if (dataTokens["area"] == null)
            {
                dataTokens.Add("area", "");
            }
            var vpd = routes.GetVirtualPathForArea(requestContext, routeData.Values);
            if (vpd != null)
            {
                var virtualPath = vpd.VirtualPath.ToLower();
                var request = requestContext.HttpContext.Request;
                if (!string.Equals(virtualPath, request.Path))
                {
                    filterContext.Result = new RedirectResult(virtualPath + request.Url.Query, true);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}