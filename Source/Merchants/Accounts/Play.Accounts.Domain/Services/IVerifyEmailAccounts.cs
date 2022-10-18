using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates;

namespace Play.Accounts.Domain.Services
{
    public interface IVerifyEmailAccounts
    {
        #region Instance Members

        public Task<uint> SendVerificationCode(string email);

        #endregion
    }
}