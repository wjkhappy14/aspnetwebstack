using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Models;

namespace System.Web.Http.SelfHost.Events
{


    /// <summary>
    /// 定义通知事件参数
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(ObjectIdentity identity)
        {
            Identity = identity;
        }
        public ObjectIdentity Identity { get; private set; }

    }
}
