using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wintellect.PowerCollections;
using System.Threading.Tasks;

namespace System.Web.Mvc.Controllers
{
    public class RedBlackTreeController : Controller
    {
        private readonly RedBlackTree<OrderedDictionary<int, BigList<double>>> tree;
        public RedBlackTreeController()
        {
            tree = SingleRedBlackTree<OrderedDictionary<int, BigList<double>>>.Instance;


        }
        public JsonResult Insert()
        {
            OrderedDictionary<int, BigList<double>> orderedDic = new OrderedDictionary<int, BigList<double>>();
            BigList<double> bigList = new BigList<double>();
            Parallel.For(1, 10000, x =>
            {
               lock(this)
                {
                    bigList.Add(x * Math.PI * new Random().NextDouble());
                }
            });
            orderedDic.Add(Environment.CurrentManagedThreadId, bigList);
            tree.Insert(orderedDic, DuplicatePolicy.InsertFirst, out orderedDic);
            return Json(new { OrderedDictionary = orderedDic }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Tree()
        {
            return Json(new { RedBlackTree = tree }, JsonRequestBehavior.AllowGet);


        }

    }
}