using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Aggregates;
using Play.Domain.Entities;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.UserRegistration;

namespace Play.Merchants.Onboarding.Domain.Users
{
    public class User : Aggregate<string>
    {
        #region Instance Values

        private Address _Address;
        private ContactInfo _ContactInfo;
        private DateTimeUtc _DateOfBirth;
        private string _LastFourOfSsn;
        private List<UserRole> _Role;
        private bool _IsActive;

        #endregion

        #region Constructor

        private User()
        {
            // Entity Framework only
        }

        private User(
            UserId id, Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth, bool isActive,
            params UserRole[] roles) : base(id)
        {
            _Address = address;
            _ContactInfo = contactInfo;
            _LastFourOfSsn = lastFourOfSsn;
            _DateOfBirth = dateOfBirth;
            _IsActive = isActive;
            _Role = roles.ToList();
        }

        #endregion

        #region Instance Members

        public static User CreateFromUserRegistration(
            UserRegistrationId userRegistrationId, Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth)
        {
            return new User(new UserId(userRegistrationId.Id), address, contactInfo, lastFourOfSsn, dateOfBirth, true, UserRole.Member);
        }

        #endregion
    }
}