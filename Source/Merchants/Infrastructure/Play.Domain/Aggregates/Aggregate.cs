using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Domain.Aggregates
{
    public abstract class Aggregate<_TId> : IAggregate<_TId>
    {
        #region Instance Values

        public EntityId<_TId>? Id { get; protected set; } = null;

        #endregion

        #region Constructor

        protected Aggregate()
        { }

        protected Aggregate(EntityId<_TId> id)
        {
            Id = id;
        }

        #endregion

        #region Instance Members

        protected void Raise(DomainEvent domainEvent)
        {
            // stuff?
            DomainEventBus.Publish(domainEvent);
        }

        public override string ToString()
        {
            return GetValueDetails();
        }

        private static string GetValueDetails()
        {
            throw new NotImplementedException();
        }

        /// <exception cref="BusinessRuleValidationException"></exception>
        protected void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
                throw new BusinessRuleValidationException(rule);
        }

        #endregion
    }
}