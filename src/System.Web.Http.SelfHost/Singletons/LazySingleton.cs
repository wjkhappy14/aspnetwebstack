using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Singletons
{
    /* <summary>
    /// In .NET Framework 4.0, the Lazy<T> class was introduced,
    ///  which internally uses double-checked locking by default (ExecutionAndPublication mode) to store either the exception that was thrown during construction,
    ///  or the result of the function that was passed to Lazy
    */
    public class LazySingleton<T> where T : class
    {
        private static readonly Lazy<LazySingleton<T>> _lazySingleton = new Lazy<LazySingleton<T>>(() => new LazySingleton<T>(), true);
        private LazySingleton()
        {
            Trace.WriteLine(string.Format("HashCode:{0}", this.GetHashCode()));

        }
        public static LazySingleton<T> Instance
        {
            get
            {
                return _lazySingleton.Value;
            }

        }
    }
}
