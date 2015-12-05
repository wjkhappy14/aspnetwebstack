using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Events
{
    /// <summary>
    /// 异步的执行结果
    /// </summary>
   public class CommandResult : IAsyncResult
    {


        public CommandResult()
        {


        }

        public object AsyncState
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsCompleted
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
