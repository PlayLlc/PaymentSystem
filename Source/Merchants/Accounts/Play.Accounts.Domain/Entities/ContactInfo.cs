using Play.Accounts.Contracts.Dtos;
using PlPlay.Accounts.Domain.ValueObjects
using PlPlay.Domain.Entities
using PlPlay.Domain.ValueObjects

namespace Play.Accounts.Domain.Entities;

public class ContactInfo : Entity<string>
{
    #region Instance Values

    public readonly Name FirstName;
    public readonly Name LastName;
    public readonly Phone Phone;
    public readonly Email Email;

    public string Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    public ContactInfo(string id, string firstName, string lastName, string phone, string email)
    {
        Id = id;
        Email = new Email(email);
        Phone = new Phone(phone);
        FirstName = new Name(firstName);
        LastName = new Name(lastName);
    }

    /// <exception cref=" ValueObjectException"></exception>
    public ContactInfo(ContactInfoDto contactInfo)
    {
        Id = contactInfo.Id!;
        Email = new Email(contactInfo.Email);
        Phone = new Phone(contactInfo.Phone);
        FirstName = new Name(contactInfo.FirstName);
        LastName = new Name(contactInfo.LastName);
    }

    #endregion

    #region Instance Members

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    public override string GetId()
    {
        return Id;
    }

    public override ContactInfoDto AsDto()
    {
        return new ContactInfoDto()

        

            ail = Email.Value,

            rstName = FirstName.Value,

            stName = LastName.Value,

            one = Phone.Value

        
    }

    #endregion
}