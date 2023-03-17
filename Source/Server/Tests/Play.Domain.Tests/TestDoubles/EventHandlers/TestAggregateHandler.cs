using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Domain.Tests.Events;

namespace Play.Domain.Tests.TestDoubles.EventHandlers;

public class TestAggregateHandler : DomainEventHandler, IHandleDomainEvents<NameWasNotBrian>
{
    #region Constructor

    public TestAggregateHandler(ILogger<TestAggregateHandler> logger) : base(logger)
    {
        //Subscribe((IHandleDomainEvents<NameWasNotBrian>) this);
    }

    #endregion

    #region Instance Members

    public Task Handle(NameWasNotBrian domainEvent) =>

        //    Log(domainEvent, LogLevel.Warning,
        //        "\n\n\n\nWARNING: There is likely an error in the client integration. The User is not associated with the specified Merchant");
        Task.CompletedTask;

    #endregion
}