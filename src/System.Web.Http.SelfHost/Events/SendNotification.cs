using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Events
{
    /// <summary>
    /// 传入一个异步的执行命令结果
    /// </summary>
    public class SendNotification
    {
        public SendNotification(CommandResult cmdResult)
        {
            Result = cmdResult;
            Notify += SendNotification_Notify;
        }



        private void DoNotify()
        {
            //Action<NotificationEventArgs> action
            if (Result.IsCompleted)
            {
               
            }
            Notify += (sender, args) =>
            {
                SendNotification_Notify(this, args);

            };

        }

        public CommandResult Result { get; private set; }
        /// <summary>
        ///   通知事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SendNotification_Notify(object sender, NotificationEventArgs e)
        {

            

        }


        public event EventHandler<NotificationEventArgs> Notify { add {  } remove { } }
    }
}
