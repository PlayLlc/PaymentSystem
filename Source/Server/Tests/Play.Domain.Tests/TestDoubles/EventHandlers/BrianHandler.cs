using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Domain.Tests.Events;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian.DomainEvents;

namespace Play.Domain.Tests.TestDoubles.EventHandlers;

public class BrianHandler : DomainEventHandler, IHandleDomainEvents<NameWasNotBrian>
{
    #region Instance Values

    public bool WasNameWasNotBrianCalled = false;

    #endregion

    #region Constructor

    public BrianHandler(ILogger<BrianHandler> logger) : base(logger)
    {
        Subscribe((IHandleDomainEvents<NameWasNotBrian>) this);
    }

    #endregion

    #region Instance Members

    public Task Handle(NameWasNotBrian domainEvent)
    {
        WasNameWasNotBrianCalled = true;

        return Task.CompletedTask;
    }

    #endregion
}