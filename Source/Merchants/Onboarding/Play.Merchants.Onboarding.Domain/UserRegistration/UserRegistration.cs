using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Entities;
using Play.Domain.Events;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.UserRegistration.Events;
using Play.Merchants.Onboarding.Domain.UserRegistration.Rules;
using Play.Merchants.Onboarding.Domain.Users;
using Play.Merchants.Onboarding.Domain.Users.Events;
using Play.Merchants.Onboarding.Domain.Users.Rules;

namespace Play.Merchants.Onboarding.Domain.UserRegistration
{
    public class UserRegistration : Aggregate<string>
    {
        #region Instance Values

        private readonly Address _Address;
        private readonly ContactInfo _ContactInfo;
        private readonly string _LastFourOfSocialSecurityNumber;
        private readonly DateTimeUtc _DateOfBirth;
        private readonly DateTimeUtc _RegisteredDate;
        private UserRegistrationStatus _Status;
        private DateTimeUtc _ConfirmedDate;

        #endregion

        #region Constructor

        private UserRegistration()
        { }

        public UserRegistration(
            UserRegistrationId id, Address address, ContactInfo contactInfo, string lastFourOfSocialSecurityNumber, DateTimeUtc dateOfBirth) : base(id)
        {
            _Address = address;
            _ContactInfo = contactInfo;

            _LastFourOfSocialSecurityNumber = lastFourOfSocialSecurityNumber;

            _DateOfBirth = dateOfBirth;
            _RegisteredDate = DateTimeUtc.Now;
            _Status = UserRegistrationStatus.WaitingForConfirmation;
        }

        /// <exception cref="BusinessRuleValidationException"></exception>
        private UserRegistration(
            Address address, ContactInfo contactInfo, string lastFourOfSocialSecurityNumber, DateTimeUtc dateOfBirth, IEnsureUniqueEmails uniqueEmailChecker) :
            base(new UserRegistrationId($"{Guid.NewGuid()}-{DateTimeUtc.Now}"))
        {
            CheckRule(new UserEmailMustBeUnique(uniqueEmailChecker, contactInfo.Email));

            _Address = address;
            _ContactInfo = contactInfo;

            _LastFourOfSocialSecurityNumber = lastFourOfSocialSecurityNumber;

            _DateOfBirth = dateOfBirth;
            _RegisteredDate = DateTimeUtc.Now;
            _Status = UserRegistrationStatus.WaitingForConfirmation;

            Raise(new UserRegistrationCreated(address, contactInfo, lastFourOfSocialSecurityNumber, dateOfBirth, _RegisteredDate));
        }

        #endregion

        #region Instance Members

        /// <exception cref="BusinessRuleValidationException"></exception>
        public static UserRegistration CreateNewUserRegistration(
            Address address, ContactInfo contactInfo, string lastFourOfSocialSecurityNumber, DateTimeUtc dateOfBirth, IEnsureUniqueEmails uniqueEmailChecker)
        {
            return new UserRegistration(address, contactInfo, lastFourOfSocialSecurityNumber, dateOfBirth, uniqueEmailChecker);
        }

        /// <exception cref="BusinessRuleValidationException"></exception>
        public User CreateUser()
        {
            CheckRule(new UserCannotBeCreatedWhenRegistrationHasExpired(_RegisteredDate));
            CheckRule(new UserCannotBeCreatedWhenRegistrationIsNotConfirmed(_Status));

            return User.CreateFromUserRegistration(Id!, _Address, _ContactInfo, _LastFourOfSocialSecurityNumber, _DateOfBirth);
        }

        /// <exception cref="BusinessRuleValidationException"></exception>
        public void Confirm()
        {
            CheckRule(new UserRegistrationCanNotBeConfirmedMoreThanOnce(_Status));
            CheckRule(new UserRegistrationCanNotBeConfirmedAfterItHasExpired(_Status));

            _Status = UserRegistrationStatus.Confirmed;
            _ConfirmedDate = DateTimeUtc.Now;

            Raise(new UserRegistrationHasBeenConfirmed(Id!));
        }

        /// <exception cref="BusinessRuleValidationException"></exception>
        public void Expire()
        {
            CheckRule(new UserRegistrationCanNotExpireMoreThanOnce(_Status));

            _Status = UserRegistrationStatus.Expired;

            Raise(new UserRegistrationHasExpired(Id!));
        }

        #endregion
    }
}