using Play.Domain.Entities;

namespace Play.Identity.Domain;

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

    #endregion

    #region Instance Members

    public override string GetId()
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