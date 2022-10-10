using Microsoft.AspNetCore.Identity;

using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Accounts.Domain.Aggregates.Users;
using Play.Accounts.Domain.Entities;
using Play.Globalization.Time;

namespace Play.Accounts.Api.Data
{
    public class ApplicationUser : IdentityUser
    {
        #region Instance Values

        private readonly UserId _Id;
        private readonly MerchantId _MerchantId;
        private readonly HashSet<UserRole> _Roles;

        private readonly Address _Address;
        private readonly ContactInfo _ContactInfo;
        private readonly DateTimeUtc _DateOfBirth;
        private readonly string _LastFourOfSsn;
        private readonly bool _IsActive;

        #endregion
    }
}