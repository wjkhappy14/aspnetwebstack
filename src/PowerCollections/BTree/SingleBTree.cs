using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintellect.PowerCollections.BTree
{



    /// <summary>
    ///   ExecutionAndPublication Thread safe  
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TP"></typeparam>
    public class SingleBTree<TK, TP> where TK : IComparable<TK>
    {

        private const int Degree = 2;
        static Lazy<BTree<TK, TP>> lazyTree = new Lazy<BTree<TK, TP>>(Init, true);
        private SingleBTree()
        {

        }


        /// <summary>
        /// get BTree  instance
        /// </summary>
        public static BTree<TK, TP> Instance
        {
            get
            {
                return lazyTree.Value;
            }
        }

        /// <summary>
        /// init  BTree
        /// </summary>
        /// <returns></returns>
        private static BTree<TK, TP> Init()
        {
            BTree<TK, TP> temp = new BTree<TK, TP>(Degree);
            return temp;
        }

    }
}
