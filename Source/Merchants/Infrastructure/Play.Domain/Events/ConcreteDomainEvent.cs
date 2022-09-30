using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Domain.Events;

internal record ConcreteDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(ConcreteDomainEvent));

    #endregion

    #region Constructor

    public ConcreteDomainEvent() : base(DomainEventTypeId)
    { }

    #endregion
}