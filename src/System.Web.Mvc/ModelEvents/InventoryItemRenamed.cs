using Labs.CQRS;
using Labs.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc.ModelEvents
{
    public class InventoryItemRenamed : ItemUpdatedEvent
    {
        public InventoryItemRenamed(ItemIdentity id,string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}