using Labs.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS
{
    public  class Repository<T> : IRepository<T> where T : AggregateRoot<T>, new()
    {
        private readonly IEventStore _storage;

        public Repository(IEventStore storage)
        {
            _storage = storage;
        }

        public T GetId(ItemIdentity identity)
        {
            var obj = new T();//lots of ways to do this
            var e = _storage.GetEventsForAggregate(identity);
            obj.LoadsFromHistory(e);
            return obj;

        }
        public virtual void Save(AggregateRoot<T> aggregate, int expectedVersion)
        {
            var uncommittedChanges = aggregate.GetUncommittedChanges();
            _storage.SaveEvents(aggregate.Id, uncommittedChanges, expectedVersion);

        }
    }
}
