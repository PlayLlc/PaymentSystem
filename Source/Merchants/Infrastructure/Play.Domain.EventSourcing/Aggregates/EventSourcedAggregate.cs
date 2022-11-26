using Play.Domain.Aggregates;
using Play.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Domain.EventSourcing.Aggregates
{
    public abstract class EventSourcedAggregate<_TId> : Aggregate<_TId> where _TId : IEquatable<_TId>
    {
        private const long _NewAggregateVersion = -1;
        private long _Version = _NewAggregateVersion;
        private readonly ICollection<DomainEvent> _UncommittedEvents = new LinkedList<DomainEvent>();




        protected abstract void Apply(dynamic @event); 
         

        protected void ApplyEvent(DomainEvent domainEvent, long version)
        { 
            if (!_UncommittedEvents.Any(x => x.GetEventId() == domainEvent.GetEventId()))
            {
                this.Apply((dynamic)domainEvent);
                _Version = version;
            }
        }



        protected  IEnumerable<DomainEvent> GetUncommittedEvents()
        {
            return _UncommittedEvents;
        }
        private void ClearUncommittedEvents()
        {
            _UncommittedEvents.Clear();
        }
    }
}
