using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS.Commands
{
   public abstract class CreateItemCommand: Command
    {
        public ItemIdentity Id { get; set; }
        public CreateItemCommand(ItemIdentity itemId)
        {
            Id = itemId;
        }
    }
}
