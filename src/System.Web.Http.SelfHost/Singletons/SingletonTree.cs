using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Models;
using System.Web.Http.SelfHost.Singletons;
using Wintellect.PowerCollections;

namespace System.Web.Http.SelfHost.Singletons
{
    public class SingletonTree<T> where T : ObjectIdentity
    {
        private static readonly Lazy<RedBlackTree<T>> _singleTree = new Lazy<RedBlackTree<T>>(Init, true);

        private SingletonTree()
        {
            Trace.WriteLine(string.Format("HashCode:{0}", this.GetHashCode()));
        }

        private static RedBlackTree<T> Init()
        {
            RedBlackTree<T> tree = new RedBlackTree<T>(Comparer<T>.Create((x, y) => { return (int)(x.Id - y.Id); }));
            return tree;
        }


        public static RedBlackTree<T> RedBlackTree
        {
            get { return _singleTree.Value; }
        }


    }
}
