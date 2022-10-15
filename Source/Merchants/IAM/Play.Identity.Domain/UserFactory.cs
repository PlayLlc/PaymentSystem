using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;
using Play.Identity.Api.Identity.Services._Email_Sms_Clientz;
using Play.Identity.Application.Services.Registration.Users;

namespace Play.Identity.Domain;

public class UserFactory
{
    #region Instance Values

    private readonly IEnsureUniqueEmails _UniqueEmailChecker;
    private readonly IExcludeProhibitedUsers _UserExclusionChecker;
    private readonly IVerifyEmailAccount _EmailAccountVerifier;
    private readonly IVerifyMobilePhone _MobilePhoneVerifier;

    #endregion

    #region Instance Members

    public static User Create(string email, Address address, ContactInfo contactInfo) //....
    {
        if (!_UniqueEmailChecker.IsUnique(email))
            DomainEventBus.Publish(new object());

        // ...
    }

    #endregion
}