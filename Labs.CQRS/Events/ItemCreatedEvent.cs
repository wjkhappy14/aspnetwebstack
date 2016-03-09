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
    public  abstract class ItemCreatedEvent : Event
    {
        public readonly ItemIdentity Id;

        public ItemCreatedEvent(ItemIdentity id)
        {
            Id = id;
        }
    }
}
