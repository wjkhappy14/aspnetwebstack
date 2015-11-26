using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace System.Web.Http.SelfHost.Controllers
{
    public class RedBlackTreeController : ApiController
    {

        RedBlackTree<double> redblackTree = new RedBlackTree<double>(Comparer<double>.Default);

        public RedBlackTreeController()
        {
            double previous = 0;

            for (int i = 0; i < 1000; i++)
            {
                redblackTree.Insert(i * Math.PI, DuplicatePolicy.InsertFirst, out previous);
            }
        }



        [HttpGet]
        public IHttpActionResult Clone()
        {
            return Json(redblackTree.Clone());
        }
        [HttpGet]
        public IHttpActionResult Sum()
        {
            return Json(new { Sum = redblackTree.Sum() });
        }
        [HttpGet]
        public IHttpActionResult TaskTraverse()
        {

            int counter = 0;
            TreeTraverse.TaskTraverse<double>(redblackTree.Root, (node) =>
            {
                counter++;
                System.Diagnostics.Debug.WriteLine(node.ToString());
            });
            return Json(new { Counter = counter });

        }

        [HttpGet]
        public IHttpActionResult ParallelTraverse()
        {

            int counter = 0;
            TreeTraverse.ParallelTraverse<double>(redblackTree.Root, (node) =>
            {
                counter++;
                System.Diagnostics.Debug.WriteLine(node.ToString());
            });
            return Json(new { Counter = counter });

        }
        [HttpGet]
        public Node<double> Nodes()
        {
            return redblackTree.Root;
        }
    }
}
