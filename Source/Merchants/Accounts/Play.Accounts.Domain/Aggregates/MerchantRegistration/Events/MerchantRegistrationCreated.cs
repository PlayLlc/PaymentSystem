using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantRegistrationCreated : DomainEvent
{
    #region Instance Values

    public readonly string Id;

    #endregion

    #region Constructor

    public MerchantRegistrationCreated(string id) : base(
        $"The {nameof(MerchantRegistration)} process has begun for the {nameof(MerchantRegistration)} with the Id: [{id}];")
    {
        Id = id;
    }

    #endregion
}