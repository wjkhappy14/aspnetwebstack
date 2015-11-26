using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Controllers
{


    /// <summary>
    /// 
    /// </summary>
    public class BlogController : ApiController
    {


        public BlogController()
        {
            
        }

        [HttpGet]
        public IHttpActionResult Config()
        {
            return Json(this.Configuration);
        }

        [HttpGet]
        public IHttpActionResult This()
        {

            return Json(this);
        }


        [HttpGet]
        public IHttpActionResult ActionContextInfo()
        {

            return Json(base.ActionContext);

        }

        [HttpGet]
        public IHttpActionResult RequestUri()
        {
            return Json(this.Request.RequestUri);
        }
    }
}
