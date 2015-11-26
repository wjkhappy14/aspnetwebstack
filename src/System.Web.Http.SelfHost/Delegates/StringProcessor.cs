using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Delegates
{


    /// <summary>
    /// 字符串处理委托
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public delegate string StringProcessorDelegate(string content);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    public delegate void StringHandlerDelegate(string content);


}
