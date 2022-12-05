using NServiceBus;

using Play.Domain.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Contracts.NetworkEvents;
using Play.Payroll.Domain.Services;
using Play.Core;
using Play.Core.Extensions.IEnumerable;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Application.Handlers.NetworkEvents;

public class PaychecksHandler : IHandleMessages<EmployeePaychecksHaveBeenCreatedEvent>
{
    #region Instance Values

    private readonly IISendAchTransfers _AchClient;
    private readonly ILogger<PaychecksHandler> _Logger;

    #endregion

    #region Constructor

    public PaychecksHandler(IISendAchTransfers achClient, ILogger<PaychecksHandler> logger)
    {
        _AchClient = achClient;
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    public async Task Handle(EmployeePaychecksHaveBeenCreatedEvent message, IMessageHandlerContext context)
    {
        await message.Employer.DistributeUndeliveredChecks(_AchClient).ConfigureAwait(false);
    }

    #endregion
}