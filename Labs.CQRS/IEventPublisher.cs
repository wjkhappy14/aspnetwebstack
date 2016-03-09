using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS
{
    /// <summary>
    /// 
    /// </summary>
   public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
    }
}
