using Labs.CQRS;
using Labs.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc.ModelEvents
{
    public class ItemsCheckedInToInventory : ItemUpdatedEvent
    {
        public int Count { get; }
        public ItemsCheckedInToInventory(ItemIdentity id, int count)
        {
            Count = count;
        }
    }
}