using Play.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Merchants.Onboarding.Contracts.Common;

namespace Play.Merchants.Onboarding.Contracts.Dto;

internal class MerchantDto : Dto<int>
{
    #region Instance Values

    public override int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    #endregion
}