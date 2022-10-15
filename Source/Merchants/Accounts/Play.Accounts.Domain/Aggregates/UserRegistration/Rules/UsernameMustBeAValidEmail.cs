using Play.Accounts.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Events;
using Play.Accounts.Domain.Aggregates.UserRegistration.Events;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates.UserRegistration.Rules
{
    public record UsernameWasNotAValidEmail : DomainEvent
    {
        #region Static Metadata

        public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UsernameWasNotAValidEmail));

        #endregion

        #region Instance Values

        public readonly string Username;

        #endregion

        #region Constructor

        public UsernameWasNotAValidEmail(string username) : base(DomainEventTypeId)
        { }

        #endregion
    }

    internal class UsernameMustBeAValidEmail : IBusinessRule
    {
        #region Instance Values

        private readonly bool _IsValid;

        public string Message => "Username must be a valid email address";

        #endregion

        #region Constructor

        internal UsernameMustBeAValidEmail(string username)
        {
            _IsValid = Zipcode.IsValid(username);
        }

        #endregion

        #region Instance Members

        public bool IsBroken()
        {
            return _IsValid;
        }

        #endregion
    }
}