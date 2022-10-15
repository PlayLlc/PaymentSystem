using Play.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Domain.Aggregates.UserRegistration.Events
{
    public record UserNameWasNotAValidEmailAddress : DomainEvent
    {
        #region Static Metadata

        public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserNameWasNotAValidEmailAddress));

        #endregion

        #region Instance Values

        public string Username;

        #endregion

        #region Constructor

        public UserNameWasNotAValidEmailAddress(string username) : base(DomainEventTypeId)
        {
            Username = username;
        }

        #endregion
    }
}