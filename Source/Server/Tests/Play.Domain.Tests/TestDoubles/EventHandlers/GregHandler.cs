using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian.DomainEvents;
using Play.Domain.Tests.TestDoubles.Aggregates.Greg;
using Play.Domain.Tests.TestDoubles.Aggregates.Greg.DomainEvents;
using Play.Domain.Tests.TestDoubles.Aggregates.Shared.DomainEvents;

namespace Play.Domain.Tests.TestDoubles.EventHandlers;

public class GregHandler : DomainEventHandler, IHandleDomainEvents<NameWasNotGreg>, IHandleDomainEvents<FirstCharacterWasNotCapitalized<Greg>>
{
    #region Instance Values

    public bool WasNameWasNotGregCalled = false;
    public bool WasFirstCharacterWasNotCapitalized = false;

    #endregion

    #region Constructor

    public GregHandler(ILogger<GregHandler> logger) : base(logger)
    {
        Subscribe((IHandleDomainEvents<FirstCharacterWasNotCapitalized<Greg>>) this);
        Subscribe((IHandleDomainEvents<NameWasNotGreg>) this);
    }

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