using Labs.CQRS;
using Labs.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc.ModelCommands
{
    public class CreateInventoryItem : CreateItemCommand
    {
        public CreateInventoryItem() : base(ItemIdentity.Default_Id)
        {
            
        }
    }
}