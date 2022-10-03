using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Merchants.Onboarding.Contracts.Common;

namespace Play.Merchants.Onboarding.Contracts.Dto;

internal class StoreDto : Dto<int>
{
    #region Instance Values

    public override int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new();

    #endregion
}