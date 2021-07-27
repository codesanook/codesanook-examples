using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Codesanook.Examples.DotNet.Orm.NHExamples
{
    // https://ayende.com/blog/3987/nhibernate-ipreupdateeventlistener-ipreinserteventlistener
    // http://fgheysels.blogspot.com/2008/07/nhibernate-iinterceptor.html
    // http://gnomealone.co.uk/2019/02/16/nhibernate-auditing/
    // NHibernate: difference between Interceptor and Listener https://stackoverflow.com/a/867356/1872200
    // Register as 
    // config.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new AuditEventListener() }; 
    // config.EventListeners.PreUpdateEventListeners = new IPreUpateEventListener[] { new AuditEventListener() }; 
    // EF similar example https://www.ryansouthgate.com/2019/03/18/ef-core-databse-auditing/
    public class AuditEventListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        // Called before inserting an item in the data store.
        // Return true to cancel an insert operation.
        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (!(@event.Entity is IAuditable audit))
                return false;

            var utcNow = DateTime.UtcNow;
            SetState(@event.Persister, @event.State, nameof(IAuditable.CreatedUtc), utcNow);
            audit.CreatedUtc = utcNow;
            return false;
        }

        public async Task<bool> OnPreInsertAsync(
            PreInsertEvent @event,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(OnPreInsert(@event));
        }

        // Called before update an item in the data store.
        // Return true to cancel an update operation.
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (!(@event.Entity is IAuditable audit))
                return false;

            var utcNow = DateTime.UtcNow;
            // Update entity state which is an array that contains the parameters that we will push into the ADO.Net Command.
            SetState(@event.Persister, @event.State, nameof(IAuditable.UpdatedUtc), utcNow);

            // Any change that we make to the entity state would not be reflected in the entity itself.
            // This makes a database row and the entity instance to get in sync
            audit.UpdatedUtc = utcNow;
            return false;
        }

        public async Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(OnPreUpdate(@event));
        }

        private void SetState(
            IEntityPersister persister,
            object[] state,
            string propertyName,
            object value
        )
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;
            state[index] = value;
        }
    }
}
