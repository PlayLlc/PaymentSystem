using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;

namespace Play.Merchants.Onboarding.Domain.Entities
{
    public record AddressId : EntityId<string>
    {
        #region Constructor

        public AddressId(string id) : base(id)
        { }

        #endregion

        #region Instance Members

        public static AddressId New()
        {
            return new AddressId(GenerateStringId());
        }

        #endregion
    }
}