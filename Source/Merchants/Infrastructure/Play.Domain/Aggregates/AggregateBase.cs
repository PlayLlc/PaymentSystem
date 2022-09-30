using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Domain.Aggregates
{
    public abstract class AggregateBase<_TId> : IAggregate<_TId>
    {
        #region Instance Values

        public EntityId<_TId> Id { get; }

        #endregion

        #region Constructor

        protected AggregateBase(EntityId<_TId> id)
        {
            Id = id;
        }

        #endregion

        #region Instance Members

        protected virtual void Raise(DomainEvent domainEvent)
        {
            // stuff?
            DomainEventBus.Publish(domainEvent);
        }

        public abstract override string ToString();

        #endregion
    }
}