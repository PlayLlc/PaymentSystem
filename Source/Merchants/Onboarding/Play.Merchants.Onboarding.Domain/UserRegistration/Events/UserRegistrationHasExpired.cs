using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Merchants.Onboarding.Domain.UserRegistration.Events
{
    public record UserRegistrationHasExpired : DomainEvent
    {
        #region Static Metadata

        public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRegistrationHasBeenConfirmed));

        #endregion

        #region Instance Values

        public EntityId<string> UserRegistrationId;

        #endregion

        #region Constructor

        public UserRegistrationHasExpired(EntityId<string> userRegistrationId) : base(DomainEventTypeId)
        {
            UserRegistrationId = userRegistrationId;
        }

        #endregion
    }
}