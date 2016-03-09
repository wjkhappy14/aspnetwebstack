using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS
{
    public class AggregateRoot<T>
    {
        private readonly List<Event> _changes = new List<Event>();
        public AggregateRoot(ItemIdentity id)
        {
            Id = id;
        }
        public ItemIdentity Id { get; set; }
        public int Version { get; internal set; }


        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }
        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }
        public void LoadsFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }
        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }
        private void ApplyChange(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }
}
