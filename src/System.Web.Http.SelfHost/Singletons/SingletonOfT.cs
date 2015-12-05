using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Singletons
{

    /// <summary>
    ///  泛型的单例 模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonOfT<T> where T : class
    {
        public static object s_lock = new object();
        private static SingletonOfT<T> s_value = new SingletonOfT<T>();

        public Missing Missing { get; set; }
        private SingletonOfT()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("ctor{0}", typeof(SingletonOfT<T>).FullName));
        }
        public  static SingletonOfT<T> GetInstanc()
        {
            if (s_value != null) return s_value;
            Monitor.Enter(s_lock);
            if (s_value == null)
            {
                SingletonOfT<T> temp = new SingletonOfT<T>();
                Interlocked.Exchange(ref s_value, temp);
            }
            Monitor.Exit(s_value);
            return s_value;
        }
    }
}
