using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Domain.Services;

public interface IEnsureUniquePasswordHistory
{
    #region Instance Members

    public Task<bool> AreLastFourPasswordsUnique(string userId, string hashedPassword);

    #endregion
}