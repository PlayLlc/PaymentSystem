using Play.Domain.Entities;

namespace Play.Merchants.Onboarding.Domain.Common;

public record ContactInfo : IEntity<string>
{
    #region Instance Values

    public readonly string FirstName;
    public readonly string LastName;
    public readonly string Phone;
    public readonly string Email;

    public EntityId<string> Id { get; }

    #endregion

    #region Constructor

    public ContactInfo(EntityId<string> id, string firstName, string lastName, string phone, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
    }

    #endregion
}