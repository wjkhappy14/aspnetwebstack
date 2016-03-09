using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS.Events
{
    public class ItemDeletedEvent : Event
    {
        public readonly ItemIdentity Id;
        public ItemDeletedEvent(ItemIdentity id)
        {
            Id = id;
        }
    }
}
