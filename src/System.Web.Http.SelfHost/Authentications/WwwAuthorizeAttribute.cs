using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace System.Web.Http.SelfHost.Authentications
{



    /// <summary>
    /// 授权
    /// </summary>
    public class WwwAuthorizeAttribute : AuthorizeAttribute
    {
        public WwwAuthorizeAttribute()
        {




        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }


    }
}
