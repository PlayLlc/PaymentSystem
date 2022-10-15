using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.UserRegistration.Rules;
using Play.Accounts.Domain.Aggregates.Users;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

using Address = Play.Accounts.Domain.Entities.Address;
using ContactInfo = Play.Accounts.Domain.Entities.ContactInfo;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

 



public class UserRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly string Username;
    private readonly string Password;


    private readonly Address? _Address;
    private readonly ContactInfo? _ContactInfo;
    private readonly string? _LastFourOfSsn;
    private readonly DateTimeUtc? _DateOfBirth;
    private readonly DateTimeUtc? _RegisteredDate;
    private DateTimeUtc? _ConfirmedDate;
    private RegistrationStatuses? _Status;

    #endregion

    #region Constructor

    public UserRegistration(string username, string password)
    {
        _Id = GenerateSimpleStringId();
      
        if(new UsernameMustBeAValidEmail(username).IsBroken())
            

    }


    public UserRegistration(Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth)
    {
        _Id = contactInfo.Email.Value;
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
        Address address = new(GenerateSimpleStringId(), registerUserRequest.AddressDto.StreetAddress, registerUserRequest.AddressDto.ApartmentNumber,
            registerUserRequest.AddressDto.Zipcode, StateAbbreviations.Empty.Get(registerUserRequest.AddressDto.StateAbbreviation),
            registerUserRequest.AddressDto.City);
        ContactInfo contactInfo = new(GenerateSimpleStringId(), registerUserRequest.ContactInfoDto.FirstName, registerUserRequest.ContactInfoDto.LastName,
            registerUserRequest.ContactInfoDto.Phone, registerUserRequest.ContactInfoDto.Email);
        UserRegistration userRegistration = new UserRegistration(address, contactInfo, registerUserRequest.PersonalInfoDto.LastFourOfSocial,
            registerUserRequest.PersonalInfoDto.DateOfBirth);

        // Validate the business rules for this Aggregate Object
        userRegistration.Enforce(new UserEmailMustBeUnique(uniqueEmailChecker, contactInfo.Email));

        // Publish a domain event when a business process has taken place
        userRegistration.Publish(new UserRegistrationCreatedDomainEvent(userRegistration._Id, address, contactInfo,
            registerUserRequest.PersonalInfoDto.LastFourOfSocial, registerUserRequest.PersonalInfoDto.DateOfBirth, userRegistration._RegisteredDate));

        return userRegistration;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public User CreateUser()
    {
        Enforce(new UserCannotBeCreatedWhenRegistrationHasExpired(_RegisteredDate));
        Enforce(new UserCannotBeCreatedWhenRegistrationIsNotConfirmed(_Status));

        User user = User.CreateFromUserRegistration(_Id!, _Address, _ContactInfo, _LastFourOfSsn, _DateOfBirth);

        return user;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void Confirm()
    {
        Enforce(new UserRegistrationCanNotBeConfirmedMoreThanOnce(_Status));
        Enforce(new UserRegistrationCanNotBeConfirmedAfterItHasExpired(_Status));

        _Status = RegistrationStatuses.Confirmed;
        _ConfirmedDate = DateTimeUtc.Now;

        Publish(new UserRegistrationHasBeenConfirmedDomainEvent(_Id!));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void Expire()
    {
        Enforce(new UserRegistrationCanNotExpireMoreThanOnce(_Status));

        _Status = RegistrationStatuses.Expired;

        Publish(new UserRegistrationHasExpiredDomainEvent(_Id!));
    }

    public override string GetId()
    {
        return _Id;
    }

    public override UserRegistrationDto AsDto()
    {
        return new UserRegistrationDto
        {
            Id = _Id,
            AddressDto = _Address.AsDto(),
            ContactInfoDto = _ContactInfo.AsDto(),
            DateOfBirth = _DateOfBirth,
            ConfirmedDate = _ConfirmedDate,
            LastFourOfSsn = _LastFourOfSsn,
            RegisteredDate = _RegisteredDate,
            RegistrationStatus = _Status
        };
    }

    #endregion
}