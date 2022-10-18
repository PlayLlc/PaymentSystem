using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates;
using Play.Core;

namespace Play.Accounts.Domain.Services
{
    public interface IVerifyEmailAccounts
    {
        #region Instance Members

        public Task<Result> SendVerificationCode(uint code, string email);

        #endregion
    }
}