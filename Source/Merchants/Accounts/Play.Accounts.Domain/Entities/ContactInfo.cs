using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Entities;

namespace Play.Accounts.Domain.Entities;

public class ContactInfo : Entity<string>
{
    #region Instance Values

    public readonly Name FirstName;
    public readonly Name LastName;
    public readonly Phone Phone;
    public readonly Email Email;

    public ContactInfoId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    public ContactInfo(ContactInfoId id, string firstName, string lastName, string phone, string email)
    {
        Id = id;
        Email = new Email(email);
        Phone = new Phone(phone);
        FirstName = new Name(firstName);
        LastName = new Name(lastName);
    }

    #endregion

    #region Instance Members

    public override ContactInfoId GetId()
    {
        return Id;
    }

    public override ContactInfoDto AsDto()
    {
        return new ContactInfoDto()
        {
            Email = Email.Value,
            FirstName = FirstName.Value,
            LastName = LastName.Value,
            Phone = Phone.Value
        };
    }

    #endregion
}