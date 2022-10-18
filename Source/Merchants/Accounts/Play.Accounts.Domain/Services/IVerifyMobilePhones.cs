using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Domain.Services
{
    public interface IVerifyMobilePhones
    {
        #region Instance Members

        public Task<ushort> SendVerificationCode(string mobile);

        #endregion
    }
}