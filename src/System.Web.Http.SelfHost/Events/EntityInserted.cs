
using System.Web.Http.SelfHost.Models;

namespace Nop.Core.Events
{
    /// <summary>
    /// A container for entities that have been inserted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityInserted<T> where T : ObjectIdentity
    {
        public EntityInserted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
