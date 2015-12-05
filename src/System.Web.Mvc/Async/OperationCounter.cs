// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Threading;

namespace System.Web.Mvc.Async
{



    /// <summary>
    /// 操作计数器
    /// </summary>
    public sealed class OperationCounter
    {
        private int _count;



        /// <summary>
        /// 定义完成事件
        /// </summary>
        public event EventHandler Completed;

        public int Count
        {
            get { return Thread.VolatileRead(ref _count); }
        }

        private int AddAndExecuteCallbackIfCompleted(int value)
        {
            int newCount = Interlocked.Add(ref _count, value);
            if (newCount == 0)
            {
                OnCompleted();
            }

            return newCount;
        }

        public int Decrement()
        {
            return AddAndExecuteCallbackIfCompleted(-1);
        }

        public int Decrement(int value)
        {
            return AddAndExecuteCallbackIfCompleted(-value);
        }

        public int Increment()
        {
            return AddAndExecuteCallbackIfCompleted(1);
        }

        public int Increment(int value)
        {
            return AddAndExecuteCallbackIfCompleted(value);
        }





        /// <summary>
        /// 在完成的时候调用该事件
        /// </summary>

        private void OnCompleted()
        {
            System.Diagnostics.Debug.WriteLine("OperationCounter ->OnCompleted:"+this.Count);
            EventHandler handler = Completed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
