using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Web.Http.SelfHost.Singletons;

namespace System.Web.Http.SelfHost.Controllers
{

    /// <summary>
    /// http://www.geeksforgeeks.org
    /// </summary>
    public class ConcurrentController : ApiController
    {

       
        private LazySingleton<Singleton> _lazySingleton = null;
        private readonly ConcurrentBag<double> bag = new ConcurrentBag<double>();
        public ConcurrentController()
        {
            this.Initialize();
        }
        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            Parallel.For(1, 10000, (index, state) =>
            {
                bag.Add(index * Math.Log(index));
              var   _singleton= Singleton.AllSingletons;
                _lazySingleton= LazySingleton<Singleton>.Instance;
                if (_singleton.GetHashCode() == _lazySingleton.GetHashCode())
                {

                }


            });
        }   
        [HttpGet]
        public IHttpActionResult Bag()
        {
            return Json(bag);
        }


        public IEnumerator<double> Enumerable()
        {
            return bag.GetEnumerator();
        }
    }
}
