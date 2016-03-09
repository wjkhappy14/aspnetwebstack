using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS
{
    public interface IRepository<T> where T : AggregateRoot<T>
    {
        void Save(AggregateRoot<T> aggregate, int expectedVersion);
        T GetId(ItemIdentity identity);

    }
}
