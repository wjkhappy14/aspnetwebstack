
using System.Web.Http.SelfHost.Events;
using System.Web.Http.SelfHost.Models;

namespace Nop.Core.Events
{
    /// <summary>
    /// A container for passing entities that have been deleted.
    ///  This is not used for entities that are deleted logicaly via a bit column.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeleted<T> where T : ObjectIdentity
    {
        public EntityDeleted(T entity)
        {
            this.Entity = entity;

            CommandResult cmd = new CommandResult();
            SendNotification nofiy = new SendNotification(cmd);
            nofiy.Notify += (sender, args) =>
            {
                System.Diagnostics.Debug.Write(sender);

            };
        }

        public T Entity { get; private set; }
    }
}
