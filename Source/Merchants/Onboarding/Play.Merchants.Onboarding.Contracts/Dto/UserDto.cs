using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Merchants.Onboarding.Contracts.Common;

namespace Play.Merchants.Onboarding.Contracts.Dto;

public class UserDto : Dto<int>
{
    #region Instance Values

    public override int Id { get; set; }
    public Address Address { get; set; } = new();
    public ContactInfo ContactInfo { get; set; } = new();

    #endregion
}