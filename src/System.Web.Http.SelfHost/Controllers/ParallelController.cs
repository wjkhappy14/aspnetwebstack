using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Controllers
{
    public class ParallelController : ApiController
    {

        double[] data = new double[10000];


        public ParallelController()
        {



        }




        /// <summary>
        /// 并行 Invoke 多个方法，是否每个方法开始执行的时间相同？
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Invoke()
        {
            Parallel.Invoke(new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },
                new Action(delegate ()
            {
                data[0] = DateTime.Now.Ticks;
            }),
            () =>
            {
                data[1] = DateTime.Now.Ticks;
            }, () =>
            {
                data[2] = DateTime.Now.Ticks;
            });
            return Json(data);
        }
        [HttpGet]
        public IHttpActionResult For()
        {
            ParallelLoopResult pResult = Parallel.For<double>(
                0,
                10000,
                new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },
                () => { return 0; },
                (long a, ParallelLoopState state, double b) =>
                {
                    if (state.IsStopped)
                    {

                    }
                    return a + b;

                },
                (x) =>
                {
                    System.Diagnostics.Debug.WriteLine(x);

                });
            return Json(pResult);
        }

    }
}
