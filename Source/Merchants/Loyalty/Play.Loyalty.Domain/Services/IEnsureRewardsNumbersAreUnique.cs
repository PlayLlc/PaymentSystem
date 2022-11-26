using Play.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Loyalty.Domain.Services;

public interface IEnsureRewardsNumbersAreUnique
{
    #region Instance Members

    public bool IsRewardsNumberUnique(SimpleStringId merchantId, string rewardsNumber);

    #endregion
}