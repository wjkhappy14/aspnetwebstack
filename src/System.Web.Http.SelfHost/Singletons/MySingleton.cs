using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
namespace System.Web.Http.SelfHost.Singletons
{
    /// <summary>
    /// double checked locking  双检锁
    ///  this is java(double checked locking)   maybe  help you http://www.infoq.com/cn/articles/double-checked-locking-with-delay-initialization
    /// https://en.wikipedia.org/wiki/Double-checked_locking#Usage_in_Microsoft_.NET_.28Visual_Basic.2C_C.23.29
    /// </summary>
    public class MySingleton
    {
        private static Object s_lock = new object();
        private static MySingleton s_value = new MySingleton();
        /// <summary>
        /// 私有构造函数，禁止外部创建实例
        /// </summary>
        private MySingleton()
        {
           Trace.WriteLine(string.Format("HashCode:{0}", this.GetHashCode()));
        }
        /// <summary>
        ///  两个If  进行两次安全检查   同样的做法还有Syste.DBNull,System.Reflection.Missing 设计
        ///  以 公开的静态的方法返回 对象
        /// </summary>
        /// <returns></returns>
        public static MySingleton GetSingleton()
        {

          Debug.WriteIf(s_value == null, "  s_value is  null");
            if (s_value != null) return s_value;
            Monitor.Enter(s_lock);
            if (s_value == null)
            {
                //如果仍旧没有创建实例，则创建一个
                MySingleton temp = new MySingleton();
                //讲引用保存到s_value 中
                Interlocked.Exchange(ref s_value, temp);
            }
            Monitor.Exit(s_lock);
            return s_value;
        }
    }
}
