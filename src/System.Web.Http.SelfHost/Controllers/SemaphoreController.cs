using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Controllers
{



    /// <summary>
    /// Semaphore 信号处理   限制可同时访问某一资源或资源池的线程数。
    /// </summary>
    public class SemaphoreController : ApiController
    {
        


        public SemaphoreController()
        {
            Mutex mutex = new Mutex();
        }


        public IHttpActionResult WaitOne()
        {
            Semaphore s = new Semaphore(1,1000);
            return Json("");

        }


    }
}
