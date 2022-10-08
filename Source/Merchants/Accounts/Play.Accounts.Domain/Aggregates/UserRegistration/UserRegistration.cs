using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.Entities;
using Play.Merchants.Onboarding.Domain.Enums;
using Play.Merchants.Onboarding.Domain.Services;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public class UserRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly UserRegistrationId _Id;
    private readonly Address _Address;
    private readonly ContactInfo _ContactInfo;
    private readonly string _LastFourOfSsn;
    private readonly DateTimeUtc _DateOfBirth;
    private readonly DateTimeUtc _RegisteredDate;
    private DateTimeUtc? _ConfirmedDate;
    private RegistrationStatuses _Status;

    #endregion

    #region Constructor

    public UserRegistration(Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth)
    {
        _Id = new UserRegistrationId(contactInfo.Email.Value);
        _Address = address;
        _ContactInfo = contactInfo;

        _LastFourOfSsn = lastFourOfSsn;

        _DateOfBirth = dateOfBirth;
        _RegisteredDate = DateTimeUtc.Now;
        _ConfirmedDate = null;
        _Status = RegistrationStatuses.WaitingForConfirmation;
    }

    #endregion

    #region Instance Members

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(RegisterUserRequest registerUserRequest, IEnsureUniqueEmails uniqueEmailChecker)
    {
        // Create the entities needed for this Aggregate Object. Entities are responsible for ensuring they are instantiated correctly
        Address address = new(AddressId.New(), registerUserRequest.Address.StreetAddress, registerUserRequest.Address.ApartmentNumber,
            registerUserRequest.Address.Zipcode, StateAbbreviations.Empty.Get(registerUserRequest.Address.StateAbbreviation), registerUserRequest.Address.City);
        ContactInfo contactInfo = new(ContactInfoId.New(), registerUserRequest.ContactInfo.FirstName, registerUserRequest.ContactInfo.LastName,
            registerUserRequest.ContactInfo.Phone, registerUserRequest.ContactInfo.Email);
        UserRegistration userRegistration = new UserRegistration(address, contactInfo, registerUserRequest.PersonalInfo.LastFourOfSocial,
            registerUserRequest.PersonalInfo.DateOfBirth);

        // Validate the business rules for this Aggregate Object
        userRegistration.Enforce(new UserEmailMustBeUnique(uniqueEmailChecker, contactInfo.Email));

        // Publish a domain event when a business process has taken place
        userRegistration.Raise(new UserRegistrationCreatedDomainEvent(userRegistration._Id, address, contactInfo,
            registerUserRequest.PersonalInfo.LastFourOfSocial, registerUserRequest.PersonalInfo.DateOfBirth, userRegistration._RegisteredDate));

        return userRegistration;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public User CreateUser()
    {
        Enforce(new UserCannotBeCreatedWhenRegistrationHasExpired(_RegisteredDate));
        Enforce(new UserCannotBeCreatedWhenRegistrationIsNotConfirmed(_Status));

        User user = User.CreateFromUserRegistration((UserRegistrationId) _Id!, _Address, _ContactInfo, _LastFourOfSsn, _DateOfBirth);

        return user;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void Confirm()
    {
        Enforce(new UserRegistrationCanNotBeConfirmedMoreThanOnce(_Status));
        Enforce(new UserRegistrationCanNotBeConfirmedAfterItHasExpired(_Status));

        _Status = RegistrationStatuses.Confirmed;
        _ConfirmedDate = DateTimeUtc.Now;

        Raise(new UserRegistrationHasBeenConfirmedDomainEvent(_Id!));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void Expire()
    {
        Enforce(new UserRegistrationCanNotExpireMoreThanOnce(_Status));

        _Status = RegistrationStatuses.Expired;

        Raise(new UserRegistrationHasExpiredDomainEvent(_Id!));
    }

    public override UserRegistrationId GetId()
    {
        return (UserRegistrationId) _Id;
    }

    public override UserRegistrationDto AsDto()
    {
        return new UserRegistrationDto
        {
            Id = _Id.Id, Address = _Address.AsDto(), ContactInfo = _ContactInfo.AsDto(), DateOfBirth = _DateOfBirth, ConfirmedDate = _ConfirmedDate,
            LastFourOfSsn = _LastFourOfSsn, RegisteredDate = _RegisteredDate, RegistrationStatus = _Status
        };
    }

    #endregion
}