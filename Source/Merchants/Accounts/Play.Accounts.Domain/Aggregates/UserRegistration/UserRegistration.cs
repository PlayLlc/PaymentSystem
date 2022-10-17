using Play.Accounts.Contracts.Commands.UserRegistration;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

using Address = Play.Accounts.Domain.Entities.Address;
using ContactInfo = Play.Accounts.Domain.Entities.ContactInfo;

namespace Play.Accounts.Domain.Aggregates;





public class UserRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly string Username;
    private readonly string Password; 

    private readonly Address? _Address;
    private readonly ContactInfo? _ContactInfo;  
    private readonly PersonalInfo? _PersonalInfo;


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


    public UserRegistration(Address address, ContactInfo contactInfo, PersonalInfo personalInfo)
    {
        _Id = contactInfo.Email.Value;
        _Address = address;
        _ContactInfo = contactInfo;
        _PersonalInfo = personalInfo;
        _RegisteredDate = DateTimeUtc.Now;
        _ConfirmedDate = null;
        _Status = RegistrationStatuses.WaitingForConfirmation;
    }

    #endregion

    #region Instance Members

    public static UserRegistration CreateNewUserRegistration()






    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(RegisterUserRequest registerUserRequest, IEnsureUniqueEmails uniqueEmailChecker)
    {
        registerUserRequest.AddressDto.Id = GenerateSimpleStringId();
        registerUserRequest.ContactInfoDto.Id = GenerateSimpleStringId();
        registerUserRequest.PersonalInfoDto.Id = GenerateSimpleStringId();

        

        // Create the entities needed for this Aggregate Object. Entities are responsible for ensuring they are instantiated correctly
        Address address = new(registerUserRequest.AddressDto);
        ContactInfo contactInfo = new(registerUserRequest.ContactInfoDto);
        PersonalInfo personalInfo = new(registerUserRequest.PersonalInfoDto);

        UserRegistration userRegistration = new UserRegistration(address, contactInfo, );

        // Validate the business rules for this Aggregate Object
        userRegistration.Enforce(new UsernameMustBeUnique(uniqueEmailChecker, contactInfo.Email));

        // Publish a domain event when a business process has taken place
        userRegistration.Publish(new UserRegistrationCreatedDomainEvent(userRegistration._Id, address, contactInfo,
            registerUserRequest.PersonalInfoDto.LastFourOfSocial, registerUserRequest.PersonalInfoDto.DateOfBirth, userRegistration._RegisteredDate));

        return userRegistration;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public User CreateUser()
    {
        Enforce(new UserCannotBeCreatedWhenRegistrationHasExpired(_RegisteredDate!.Value));
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