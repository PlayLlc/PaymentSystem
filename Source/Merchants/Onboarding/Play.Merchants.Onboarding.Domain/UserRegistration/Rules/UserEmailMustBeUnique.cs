using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;

namespace Play.Merchants.Onboarding.Domain.Users.Rules
{
    public interface IEnsureUniqueEmails
    {
        #region Instance Members

        public bool IsUnique(string email);

        #endregion
    }

    internal class UserEmailMustBeUnique : IBusinessRule
    {
        #region Instance Values

        private readonly IEnsureUniqueEmails _UniqueEmailChecker;
        private readonly string _Email;

        public string Message => "User Login must be unique";

        #endregion

        #region Constructor

        internal UserEmailMustBeUnique(IEnsureUniqueEmails uniqueEmailChecker, string email)
        {
            _UniqueEmailChecker = uniqueEmailChecker;
            _Email = email;
        }

        #endregion

        #region Instance Members

        public bool IsBroken()
        {
            if (!_UniqueEmailChecker.IsUnique(_Email))
                return true;

            return false;
        }

        #endregion
    }
}