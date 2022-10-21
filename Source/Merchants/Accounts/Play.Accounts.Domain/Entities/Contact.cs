using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.Entities;

public class Contact : Entity<string>
{
    #region Instance Values

    public readonly Name FirstName;
    public readonly Name LastName;
    public readonly Phone Phone;
    public readonly Email Email;

    public string Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Contact(string id, string firstName, string lastName, string phone, string email)
    {
        Id = id;
        Email = new Email(email);
        Phone = new Phone(phone);
        FirstName = new Name(firstName);
        LastName = new Name(lastName);
    }

    /// <exception cref=" ValueObjectException"></exception>
    public Contact(ContactDto contact)
    {
        Id = contact.Id!;
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

    public override string GetId()
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