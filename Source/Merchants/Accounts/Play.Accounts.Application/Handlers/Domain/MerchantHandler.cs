using Play.Domain.Events;

using Microsoft.Extensions.Logging;

namespace Play.Accounts.Application.Handlers.Domain;

public class MerchantHandler : DomainEventHandler
{
    #region Constructor

    public MerchantHandler(ILogger<MerchantHandler> logger) : base(logger)
    { }

    #endregion
}