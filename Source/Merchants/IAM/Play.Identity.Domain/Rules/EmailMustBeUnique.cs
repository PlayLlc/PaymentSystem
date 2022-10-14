using Play.Codecs;
using Play.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Services;
using Play.Domain.Events;

namespace Play.Identity.Domain.Rules
{
    public record EmailWasNotUnique : DomainEvent
    { }

    internal class EmailMustBeUnique : IBusinessRule
    {
        #region Instance Values

        private readonly bool _AlphabeticCharacterExists;
        public string Message => "New users must choose a new email. The email provided is already in use";

        #endregion

        #region Constructor

        public EmailMustBeUnique(IEnsureUniqueEmails uniqueEmailChecker, string email)
        {
            if (!uniqueEmailChecker.IsUnique(email))
            {
                _AlphabeticCharacterExists = false;
                DomainEventBus.Publish(new object());
            }

            _AlphabeticCharacterExists = true;
        }

        #endregion

        #region Instance Members

        public bool IsBroken()
        {
            return _AlphabeticCharacterExists;
        }

        #endregion
    }
}