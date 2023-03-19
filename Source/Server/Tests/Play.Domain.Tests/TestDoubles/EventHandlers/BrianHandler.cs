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
using Play.Domain.Tests.TestDoubles.Aggregates.Greg;
using Play.Domain.Tests.TestDoubles.Aggregates.Greg.DomainEvents;
using Play.Domain.Tests.TestDoubles.Aggregates.Shared.DomainEvents;

namespace Play.Domain.Tests.TestDoubles.EventHandlers;

public class BrianHandler : DomainEventHandler, IHandleDomainEvents<NameWasNotBrian>
{
    #region Instance Values

    public bool WasNameWasNotBrianCalled = false;

    #endregion

    #region Constructor

    public BrianHandler(ILogger<BrianHandler> logger) : base(logger)
    { }

    #endregion

    #region Instance Members

    public virtual Task Handle(NameWasNotBrian domainEvent)
    {
        WasNameWasNotBrianCalled = true;

        return Task.CompletedTask;
    }

    #endregion
}

public class GregHandler : DomainEventHandler, IHandleDomainEvents<NameWasNotGreg>, IHandleDomainEvents<FirstCharacterWasNotCapitalized<Greg>>
{
    #region Instance Values

    public bool WasNameWasNotGregCalled = false;
    public bool WasFirstCharacterWasNotCapitalized = false;

    #endregion

    #region Constructor

    public GregHandler(ILogger<BrianHandler> logger) : base(logger)
    { }

    #endregion

    #region Instance Members

    public Task Handle(FirstCharacterWasNotCapitalized<Greg> domainEvent)
    {
        WasFirstCharacterWasNotCapitalized = true;

        return Task.CompletedTask;
    }

    public Task Handle(NameWasNotGreg domainEvent)
    {
        WasNameWasNotGregCalled = true;

        return Task.CompletedTask;
    }

    #endregion
}

public class GregHandler2 : DomainEventHandler, IHandleDomainEvents<FirstCharacterWasNotCapitalized<Greg>>
{
    #region Instance Values

    public bool WasNameWasNotGregCalled = false;
    public bool WasFirstCharacterWasNotCapitalized = false;

    #endregion

    #region Constructor

    public GregHandler2(ILogger<BrianHandler> logger) : base(logger)
    { }

    #endregion

    #region Instance Members

    public Task Handle(FirstCharacterWasNotCapitalized<Greg> domainEvent)
    {
        WasFirstCharacterWasNotCapitalized = true;

        return Task.CompletedTask;
    }

    #endregion
}