using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace Labs.CQRS
{
    public class ServiceBus : ICommandSender, IEventPublisher
    {

        private readonly ConcurrentDictionary<Type, List<Action<Message>>> _routues = new ConcurrentDictionary<Type, List<Action<Message>>>();
        public void RegisterHandler<T>(Action<T> handler) where T : Message
        {
            System.Diagnostics.Debug.Write(typeof(T));
            List<Action<Message>> handlers;
            if (!_routues.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<Message>>();
                _routues.TryAdd(typeof(T), handlers);
            }
            handlers.Add(x => handler((T)x));
        }


        /// <summary>
        /// 读取当前的命令路由信息
        /// </summary>
        public ConcurrentDictionary<Type, List<Action<Message>>> Routes
        {
            get { return _routues; }
        }

        public void Publish<T>(T @event) where T : Event
        {
            System.Diagnostics.Debug.Write(@event.GetType().FullName);
            System.Diagnostics.Debug.Write(_routues.Keys);

            List<Action<Message>> handlers;
            if (!_routues.TryGetValue(@event.GetType(), out handlers))
                return;
            foreach (var handler in handlers)
            {
                //Add to  ThreadPool
                ThreadPool.QueueUserWorkItem(x => handler(@event));
            }
        }

        public void Send<T>(T command) where T : Command
        {
            List<Action<Message>> handlers;
            if (_routues.TryGetValue(typeof(T), out handlers))
            {
                if (handlers.Count != 1)
                    throw new InvalidOperationException("Can not send to more than one handler!");
                handlers[0](command);
            }
            else
            {
                throw new InvalidOperationException("No handler registered for this  command!");
            }
        }
    }
}
