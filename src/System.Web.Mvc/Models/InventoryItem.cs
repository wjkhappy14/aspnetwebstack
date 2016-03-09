using Labs.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.ModelEvents;

namespace System.Web.Mvc.Models
{

    
    public class InventoryItem : AggregateRoot<InventoryItem>
    {
        private bool _activated;
        private ItemIdentity _id;
        public DateTime Now { get; } = DateTime.Now;

        private void Apply(InventoryItemCreated e)
        {
            _id = e.Id;
            _activated = true;
        }

        private void Apply(InventoryItemDeactivated e)
        {
            _activated = false;
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new InventoryItemRenamed(_id, newName));
        }

        public void Remove(int count)
        {
            if (count <= 0) throw new InvalidOperationException("cant remove negative count from inventory");
            ApplyChange(new ItemsDeletedFromInventory(_id, count));
        }


        public void CheckIn(int count)
        {
            if (count <= 0) throw new InvalidOperationException("must have a count greater than 0 to add to inventory");
            ApplyChange(new ItemsCheckedInToInventory(_id, count));
        }

        public void Deactivate()
        {
            if (!_activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new InventoryItemDeactivated(_id));
        }

        public ItemIdentity Id
        {
            get { return _id; }
        }

        public InventoryItem(ItemIdentity id) : base(id)
        {
           base.ApplyChange(new InventoryItemCreated(id,"Hello"));

        }

        public InventoryItem() : base(ItemIdentity.Default_Id)
        {
            var now = DateTime.Now;
            base.ApplyChange(new InventoryItemCreated(base.Id, "Hello"));

        }
    }

}