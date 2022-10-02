using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;

namespace Play.Merchants.Onboarding.Domain.UserRegistration
{
    public record UserRegistrationId : EntityId<string>
    {
        #region Constructor

        public UserRegistrationId(string id) : base(id)
        { }

        #endregion
    }
}