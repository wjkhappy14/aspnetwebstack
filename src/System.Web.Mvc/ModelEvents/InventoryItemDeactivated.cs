using Labs.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc.ModelEvents
{
    public class InventoryItemDeactivated : ItemCreatedEvent
    {
        public InventoryItemDeactivated() : base(ItemIdentity.Default_Id)
        {
        }

        public InventoryItemDeactivated(ItemIdentity id) : base(id)
        {
        }
    }
}