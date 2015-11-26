using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintellect.PowerCollections
{



    /// <summary>
    /// https://msdn.microsoft.com/zh-cn/library/dd557750(v=vs.100).aspx
    ///  https://msdn.microsoft.com/zh-cn/library/dd460693(v=vs.100).aspx
    /// http://blogs.msdn.com/b/pfxteam/archive/2008/01/31/7357135.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeTraverse
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="action"></param>
        // By using tasks explcitly.
        public static void TaskTraverse<T>(Node<T> node, Action<T> action)
        {
            if (node == null) return;
            var left = Task.Factory.StartNew(() => TaskTraverse(node.Left, action));
            var right = Task.Factory.StartNew(() => TaskTraverse(node.Right, action));
            action(node.Item);

            try
            {
                Task.WaitAll(left, right);
            }
            catch (AggregateException ex)
            {

                var msg = ex.Message;

                //handle exceptions here
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="action"></param>
        // By using Parallel.Invoke
        public static void ParallelTraverse<T>(Node<T> node, Action<T> action)
        {
            if (node == null) return;
            Parallel.Invoke(
                () => ParallelTraverse(node.Left, action),
                () => ParallelTraverse(node.Right, action),
                () => action(node.Item)
            );
        }


    }

  
}
