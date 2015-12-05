
using System.Web.Http.SelfHost.Events;
using System.Web.Http.SelfHost.Models;

namespace Nop.Core.Events
{
    /// <summary>
    /// A container for entities that are updated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityUpdated<T> where T : ObjectIdentity
    {
        public EntityUpdated(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }


    }
}
