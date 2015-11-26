using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace System.Web.Http.SelfHost.Controllers
{


    /// <summary>
    /// 
    /// </summary>
    public class OrderedSetController : ApiController
    {
        private OrderedSet<double> orderedSet = new OrderedSet<double>();
        public OrderedSetController()
        {
            Parallel.For(0,1000,(x)=>{
                orderedSet.Add(Math.E * Math.PI * DateTime.Now.Ticks);
            });
        }



        [HttpGet]
        public IHttpActionResult OrderedSet()
        {
            return Json(orderedSet);
        }


        [HttpGet]
        public IHttpActionResult Delete()
        {

            bool isOK = orderedSet.Remove(Math.E * Math.PI);
            return Json(new { isOK = isOK });
        }
    }
}
