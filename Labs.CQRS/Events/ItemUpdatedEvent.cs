using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS.Events
{
    public class ItemUpdatedEvent : Event
    {
        public readonly ItemIdentity Id;
        public ItemUpdatedEvent()
        {
            Id = ItemIdentity.Default_Id;
        }
        public ItemUpdatedEvent(ItemIdentity id)
        {
            Id = id;
        }
    }
}
