using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost
{

    /// <summary>
    /// 对于可空类型的自定义设置默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyValue<T> where T : struct
    {
        private Nullable<T> value;
        private Func<T> getValue;
        public LazyValue(Func<T> func, Nullable<T> val)
        {
            getValue = func;
            value = val;
        }
        public T Value
        {
            get
            {
                if (value == null)
                {
                    return getValue();//通过外部函数来设置默认值
                }
                return (T)value;
            }
        }

    }
}
