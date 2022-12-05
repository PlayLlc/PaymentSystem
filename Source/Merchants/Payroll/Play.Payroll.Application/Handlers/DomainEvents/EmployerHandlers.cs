using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Payroll.Domain.Repositories;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Application.Handlers.DomainEvents;

public partial class EmployerHandlers : DomainEventHandler
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IEmployerRepository _EmployerRepository;

    #endregion

    #region Constructor

    public EmployerHandlers(ILogger logger, IMessageHandlerContext messageHandlerContext, IEmployerRepository employerRepository) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _EmployerRepository = employerRepository;
    }

    #endregion
}