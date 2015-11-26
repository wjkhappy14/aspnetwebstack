using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace System.Web.Http.SelfHost.Controllers
{
    public class AlgorithmsController : ApiController
    {



        private double[] data = new double[100];

        public AlgorithmsController()
        {
            for (int i=0;i<data.Length;i++)
            {
                data.SetValue(Math.E*Math.Log10(i+1),i);
            }
        }


        /// <summary>
        /// 计算两个自然数的最大公约数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GCD(int x, int y)
        {
            int gcd = Algorithms.TailRecursionCalcGCD(x, y);
            return Json(new { x = x, y = y, GCD = gcd });
        }

        /// <summary>
        /// 交换两个数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Swap(int x, int y)
        {
            Algorithms.Swap<int>(ref x, ref y);
            return Json(new { x = x, y = y });
        }
        [HttpGet]
        public IHttpActionResult Reverse()
        {

            Algorithms.Reverse<double>(data);
            return Json(data);


        }



        public IHttpActionResult BinarySearch()
        {
            var list = Enumerable.Range(0,1000).ToList();
            int index = 0;
            var result = Algorithms.BinarySearch<int>(list, 233, out index);


            return Json("");
        }


        public IHttpActionResult FindIndicesWhere()
        {
            var list = Enumerable.Range(0, 1000).ToList();

            var indices = Algorithms.FindIndicesWhere<int>(list, x => x % 2 == 3);
            return Json("");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IHttpActionResult Rotate()
        {
         var result= Algorithms.Rotate(data, data.Length);
            return Json(result);
        }

    }
}
