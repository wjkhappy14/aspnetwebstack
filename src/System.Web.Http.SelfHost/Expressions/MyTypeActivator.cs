using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;


namespace System.Web.Http.SelfHost.Expressions
{



    public class MyTypeActivator
    {

        public static Func<TBase> Create<TBase>(Type type) where TBase : class
        {
            NewExpression instance = Expression.New(type);
            return Expression.Lambda<Func<TBase>>(instance).Compile();
        }

        /// <summary>
        /// 创建泛型的Func 
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <returns></returns>
        public static Func<TInstance> Create<TInstance>() where TInstance : class
        {
            return Create<TInstance>(typeof(TInstance));
        }


        /// <summary>
        /// 创建Oject的实例
        /// </summary>
        /// <returns></returns>
        public Func<object> Create()
        {
            return Create<object>(typeof(System.Object));
        }

    }
}
