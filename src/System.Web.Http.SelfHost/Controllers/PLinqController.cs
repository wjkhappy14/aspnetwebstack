using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Controllers
{
    /// <summary>
    /// PLINQ 查询会根据主计算机的能力按比例调整并发程度
    /// </summary>
    public class PLinqController : ApiController
    {
        private double[] data = new double[10000];

        public PLinqController()
        {
            Parallel.For(0, 10000, (x) =>
            {
                data[x] = x * Math.E;
            });
        }
        [HttpGet]
        public IHttpActionResult Show()
        {
            return Json(data);
        }


    }
}
