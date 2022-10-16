using Play.Accounts.Domain.Aggregates.MerchantRegistration;
using Play.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates.Merchants;

namespace Play.Accounts.Domain.Repositories
{
    public interface IMerchantRepository : IRepository<Merchant, string>
    { }
}