using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.Entities;
using Play.Merchants.Onboarding.Domain.Enums;
using Play.Merchants.Onboarding.Domain.Services;
using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public class UserRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly UserRegistrationId _Id;
    private readonly Address _Address;
    private readonly ContactInfo _ContactInfo;
    private readonly string _LastFourOfSocialSecurityNumber;
    private readonly DateTimeUtc _DateOfBirth;
    private readonly DateTimeUtc _RegisteredDate;
    private DateTimeUtc? _ConfirmedDate;
    private RegistrationStatuses _Status;

    #endregion

    #region Constructor

    private UserRegistration()
    { }

    public UserRegistration(UserRegistrationId id, Address address, ContactInfo contactInfo, string lastFourOfSocialSecurityNumber, DateTimeUtc dateOfBirth)
    {
        _Id = id;
        _Address = address;
        _ContactInfo = contactInfo;

        _LastFourOfSocialSecurityNumber = lastFourOfSocialSecurityNumber;

        _DateOfBirth = dateOfBirth;
        _RegisteredDate = DateTimeUtc.Now;
        _ConfirmedDate = null;
        _Status = RegistrationStatuses.WaitingForConfirmation;
    }

    #endregion

    #region Instance Members

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(
        string name, string streetAddress, string apartmentNumber, string zipcode, StateAbbreviations state, string city, string firstName, string lastName,
        string phone, string email, string lastFourOfSocialSecurityNumber, DateTimeUtc dateOfBirth, IEnsureUniqueEmails uniqueEmailChecker)
    {
        UserRegistrationId id = UserRegistrationId.New();

        // Create the entities needed for this Aggregate Object. Entities are responsible for ensuring they are instantiated correctly
        Address address = new(AddressId.New(), streetAddress, apartmentNumber, zipcode, state, city);
        ContactInfo contactInfo = new(ContactInfoId.New(), firstName, lastName, phone, email);
        UserRegistration userRegistration = new UserRegistration(id, address, contactInfo, lastFourOfSocialSecurityNumber, dateOfBirth);

        // Validate the business rules for this Aggregate Object
        userRegistration.Enforce(new UserEmailMustBeUnique(uniqueEmailChecker, contactInfo.Email));

        // Publish a domain event when a business process has taken place
        userRegistration.Raise(new UserRegistrationCreated(address, contactInfo, lastFourOfSocialSecurityNumber, dateOfBirth,
            userRegistration._RegisteredDate));

        return userRegistration;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public User CreateUser()
    {
        Enforce(new UserCannotBeCreatedWhenRegistrationHasExpired(_RegisteredDate));
        Enforce(new UserCannotBeCreatedWhenRegistrationIsNotConfirmed(_Status));

        User user = User.CreateFromUserRegistration((UserRegistrationId) _Id!, _Address, _ContactInfo, _LastFourOfSocialSecurityNumber, _DateOfBirth);

        return user;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void Confirm()
    {
        Enforce(new UserRegistrationCanNotBeConfirmedMoreThanOnce(_Status));
        Enforce(new UserRegistrationCanNotBeConfirmedAfterItHasExpired(_Status));

        _Status = RegistrationStatuses.Confirmed;
        _ConfirmedDate = DateTimeUtc.Now;

        Raise(new UserRegistrationHasBeenConfirmed(_Id!));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void Expire()
    {
        Enforce(new UserRegistrationCanNotExpireMoreThanOnce(_Status));

        _Status = RegistrationStatuses.Expired;

        Raise(new UserRegistrationHasExpired(_Id!));
    }

    public override UserRegistrationId GetId()
    {
        return (UserRegistrationId) _Id;
    }

    #endregion
}