using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS
{
    public abstract class ItemIdentity
    {
        public static ItemIdentity Default_Id = ItemIdentity.Create();
        public static ItemIdentity Create()
        {
            return new GuidIdentity();
        }

        private class GuidIdentity : ItemIdentity
        {
            public GuidIdentity()
            {
            }
            public Guid Guid { get; } = Guid.NewGuid();
        }
    }
}
