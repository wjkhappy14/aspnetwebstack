using ParallelMergeSort;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Models;

namespace System.Web.Http.SelfHost.Controllers
{




    /// <summary>
    /// 并行方式排序
    /// </summary>
    public class ParallelSortController : ApiController
    {
        private Stopwatch _stopwatch;
        private readonly ConcurrentBag<double> bag = new ConcurrentBag<double>();
        private double[] data;
        public ParallelSortController()
        {
            this.Initialize();
        }
        private void Initialize()
        {
            Parallel.For(1, 10000, (index, state) =>
            {
                bag.Add(index * Math.Log(index));
                bag.Add(index * Math.PI);
            });

            data = bag.ToArray<double>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Sort()
        {
            _stopwatch = Stopwatch.StartNew();
            ParallelSort.Sort<double>(data);
            _stopwatch.Stop();
            SortInfo<double> info = new SortInfo<double>();
            info.Name = "Sort";
            info.Count = data.Length;
            info.Elapsed = _stopwatch.Elapsed;
            info.Array = data;
            return Json(info);
        }
    }
}
