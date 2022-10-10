using Play.Accounts.Domain.Entities;
using Play.Domain.Events;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

public record UserRegistrationCreatedDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UserRegistrationCreatedDomainEvent));

    #endregion

    #region Instance Values

    public readonly UserRegistrationId UserRegistrationId;
    public readonly string StreetAddress;
    public readonly string ApartmentNumber;
    public readonly string Zipcode;
    public readonly string State;
    public readonly string City;
    public readonly string LastFourOfSocialSecurityNumber;
    public readonly string FirstName;
    public readonly string LastName;
    public readonly string Phone;
    public readonly string Email;
    public readonly DateTimeUtc DateOfBirth;
    public readonly DateTimeUtc RegisteredDate;

    #endregion

    #region Constructor

    public UserRegistrationCreatedDomainEvent(
        UserRegistrationId userRegistrationId, Address address, ContactInfo contactInfo, string lastFourOfSocialSecurityNumber, DateTimeUtc dateOfBirth,
        DateTimeUtc registeredDate) : base(DomainEventTypeId)
    {
        UserRegistrationId = userRegistrationId;
        StreetAddress = address.StreetAddress;
        ApartmentNumber = address.ApartmentNumber;
        Zipcode = address.Zipcode.Value;
        State = address.State;
        City = address.City;

        LastFourOfSocialSecurityNumber = lastFourOfSocialSecurityNumber;

        FirstName = contactInfo.FirstName.Value;
        LastName = contactInfo.LastName.Value;
        Phone = contactInfo.Phone.Value;
        Email = contactInfo.Email.Value;
        DateOfBirth = dateOfBirth;
        RegisteredDate = registeredDate;
    }

    #endregion
}