using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace System.Web.Http.SelfHost.Controllers
{

    [CLSCompliant(false)]//指示所指示的程序元素是否符合 CLS
    public class HubController : HubControllerBase
    {
        public HubController()
        {



        }




        [HttpGet]
        public IHttpActionResult Push()
        {
            ConnectionManager.GetHubContext("Jobs");
            return Json("");
        }

        protected override IHubContext HubContext
        {
            get
            {

                throw new NotImplementedException();
            }
        }
    }
}
