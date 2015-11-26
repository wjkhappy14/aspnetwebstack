using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Delegates;
using Wintellect.PowerCollections.BasicAlgorithms;

namespace System.Web.Http.SelfHost.Controllers
{




    /*
    NET Framework 中的事件模型基于具有事件委托，该委托将事件与事件处理程序连接起来。引发事件需要两个元素：
•引用向事件提供响应的方法的委托。
•保存事件数据的类。
委托是一个定义签名的类型，即方法的返回值类型和参数列表类型。可以使用委托类型来声明一个变量，该变量可以引用与委托签名相同的所有方法。
事件处理程序委托的标准签名定义一个没有返回值的方法，其第一个参数的类型为 Object，它引用引发事件的实例，第二个参数从 EventArgs 类型派生，它保存事件数据。如果事件不生成事件数据，则第二个参数只是 EventArgs 的一个实例。否则，第二个参数为从 EventArgs 派生的自定义类型，提供保存事件数据所需的全部字段或属性。
EventHandler<TEventArgs> 是一种预定义委托，表示事件的事件处理程序方法，它与事件是否生成事件数据无关。如果事件不生成事件数据，则用 EventArgs 替代泛型类型参数；否则，提供自己的自定义事件数据类型并用该类型替代泛型类型参数。
使用 EventHandler<TEventArgs> 的优点在于，如果事件生成事件数据，则无需编写自己的自定义委托代码。此外，.NET Framework 只需一个实现就能支持 EventHandler<TEventArgs>，这与替代泛型类型参数的事件数据类型无关。
若要将事件与处理事件的方法关联，请向事件添加委托的实例。除非移除了该委托，否则每当发生该事件时都会调用事件处理程序。
        */

    /// <summary>
    /// 2,000 Things You Should Know About C#   http://csharp.2000things.com/
    /// </summary>
    public class DelegateController : ApiController
    {
        StringProcessorDelegate x = (content) =>
        {
            int begin = 0;
            int end = content.Length;
            while (begin != end)
            {
                //  Common.Swap(ref content[begin],ref content[end]);
            }
            return content.Reverse().ToString();
        };


        StringHandlerDelegate handler = (content) =>
        {

        };


        public DelegateController()
        {
            x.Invoke("Hello Angkor");
        }






        /// <summary>
        /// 返回委托的调用列表
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public Delegate[] InvocationList()
        {
            Delegate.Combine(x, handler);
            x.Equals(handler);
            return x.GetInvocationList();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult EventHandlerList()
        {
            EventHandlerList eventHandlers = new EventHandlerList();
            eventHandlers.AddHandler("StringProcessorDelegate", x);
            return Json(eventHandlers);
        }
    }
}
