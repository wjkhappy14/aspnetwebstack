using Labs.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc.ModelEvents
{
    public class InventoryItemCreated: ItemCreatedEvent
    {
        public InventoryItemCreated(ItemIdentity id,string name) :base(id)
        {

        }
    }
}