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
    public class TaskController : ApiController
    {

        private double[] data = new double[10000];


        public TaskController()
        {
            this.Factory();
        }
        private Task Factory()
        {
            return Task.Factory.StartNew(() =>
            {
                Enumerable.Range(0, 10000).AsParallel().ForAll((item)=> {

                    
                });

            }, TaskCreationOptions.LongRunning
           );


        }

        public IHttpActionResult Yield()
        {
            var t = Task.Yield();
            return Json(t);
        }
    }
}
