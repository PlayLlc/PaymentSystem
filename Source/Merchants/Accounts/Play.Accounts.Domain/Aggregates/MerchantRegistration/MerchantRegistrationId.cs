using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationId : EntityId<string>
{
    #region Constructor

    public MerchantRegistrationId(string id) : base(id)
    { }

    #endregion

    #region Instance Members

    public static MerchantRegistrationId New()
    {
        return new MerchantRegistrationId(GenerateStringId());
    }

    #endregion
}