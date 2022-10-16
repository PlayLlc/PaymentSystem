using Play.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Play.Accounts.Application.Handlers.Domain
{
    public class MerchantHandler : DomainEventHandler
    {
        #region Constructor

        public MerchantHandler(ILogger<MerchantHandler> logger) : base(logger)
        { }

        #endregion
    }
}