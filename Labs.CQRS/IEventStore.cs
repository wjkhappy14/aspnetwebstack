using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS
{
    public interface IEventStore
    {
        void SaveEvents(ItemIdentity aggregateId, IEnumerable<Event> events, int expectedVersion);
        List<Event> GetEventsForAggregate(ItemIdentity aggregateId);
    }
}
