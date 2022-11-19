using Play.Domain.Common.Dtos;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;

namespace Play.Domain.Common.Entities;

public class Contact : Entity<SimpleStringId>
{
    #region Instance Values

    public readonly Name FirstName;
    public readonly Name LastName;
    public readonly Phone Phone;
    public readonly Email Email;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Contact()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Contact(string id, string firstName, string lastName, string phone, string email)
    {
        Id = new SimpleStringId(id);
        Email = new Email(email);
        Phone = new Phone(phone);
        FirstName = new Name(firstName);
        LastName = new Name(lastName);
    }

    /// <exception cref=" ValueObjectException"></exception>
    public Contact(ContactDto contact)
    {
        Id = new SimpleStringId(contact.Id!);
        Email = new Email(contact.Email);
        Phone = new Phone(contact.Phone);
        FirstName = new Name(contact.FirstName);
        LastName = new Name(contact.LastName);
    }

    #endregion

    #region Instance Members

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override ContactDto AsDto()
    {
        return new ContactDto()
        {
            Id = Id,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,
            Phone = Phone
        };
    }

    #endregion
}