using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core;

namespace Play.Accounts.Domain.Services
{
    public interface IVerifyMobilePhones
    {
        #region Instance Members

        public Task<Result> SendVerificationCode(uint code, string mobile);

        #endregion
    }
}