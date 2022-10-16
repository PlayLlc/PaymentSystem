using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates.MerchantRegistration;
using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Domain.Repositories;

namespace Play.Accounts.Domain.Repositories
{
    public interface IMerchantRegistrationRepository : IRepository<MerchantRegistration, string>
    { }
}