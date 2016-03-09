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
    public class EventStore : IEventStore
    {
        public readonly IEventPublisher _publisher;
        private readonly Dictionary<ItemIdentity, List<EventDescriptor>> _current = new Dictionary<ItemIdentity, List<EventDescriptor>>();
        public EventStore(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        private struct EventDescriptor
        {
            public readonly Event EventData;
            public readonly ItemIdentity Id;
            public readonly int Version;

            public EventDescriptor(ItemIdentity id, Event eventData, int version)
            {
                Id = id;
                EventData = eventData;  
                Version = version;
            }
        }



        List<Event> IEventStore.GetEventsForAggregate(ItemIdentity aggregateId)
        {
            List<EventDescriptor> eventDescriptors;

            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                throw new AggregateNotFoundException();
            }

            return eventDescriptors.Select(desc => desc.EventData).ToList();

        }

        void IEventStore.SaveEvents(ItemIdentity aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            List<EventDescriptor> eventDescriptors;

            // try to get event descriptors list for given aggregate id
            // otherwise -> create empty dictionary
            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                eventDescriptors = new List<EventDescriptor>();
                _current.Add(aggregateId, eventDescriptors);
            }
            // check whether latest event version matches current aggregate version
            // otherwise -> throw exception
            else if (eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1)
            {
                throw new ConcurrencyException();
            }
            var i = expectedVersion;

            // iterate through current aggregate events increasing version with each processed event
            foreach (var @event in events)
            {
                i++;
                @event.Version = i;

                // push event to the event descriptors list for current aggregate
                eventDescriptors.Add(new EventDescriptor(aggregateId, @event, i));

                // publish current event to the bus for further processing by subscribers
                _publisher.Publish(@event);
            }
        }
    }
}
