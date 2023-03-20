using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Domain.Tests.TestDoubles.Aggregates.Greg;
using Play.Domain.Tests.TestDoubles.Aggregates.Shared.DomainEvents;

namespace Play.Domain.Tests.TestDoubles.EventHandlers;

public class GregGenericHandler : DomainEventHandler, IHandleDomainEvents<FirstCharacterWasNotCapitalized<Greg>>
{
    #region Instance Values

    public bool WasNameWasNotGregCalled = false;
    public bool WasFirstCharacterWasNotCapitalized = false;

    #endregion

    #region Constructor

    public GregGenericHandler(ILogger<BrianHandler> logger) : base(logger)
    {
        Subscribe((IHandleDomainEvents<FirstCharacterWasNotCapitalized<Greg>>) this);
    }

    #endregion

    #region Instance Members

    public Task Handle(FirstCharacterWasNotCapitalized<Greg> domainEvent)
    {
        WasFirstCharacterWasNotCapitalized = true;

        return Task.CompletedTask;
    }

    #endregion
}