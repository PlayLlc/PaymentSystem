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
    public readonly Name Name;
    public readonly Address Address;
    public readonly BusinessTypes BusinessType;
    public readonly MerchantCategoryCode MerchantCategoryCode;
    public readonly DateTimeUtc RegisteredDate;
    public RegistrationStatuses Status;

    #endregion

    #region Constructor

    public MerchantRegistrationCreated(
        string id, Name name, Address address, BusinessTypes businessType, MerchantCategoryCode merchantCategoryCode, DateTimeUtc registeredDate,
        RegistrationStatuses status) : base($"The {nameof(Merchant)}: [{name}] has begun the registration process")
    {
        Id = id;
        Name = name;
        Address = address;
        BusinessType = businessType;
        MerchantCategoryCode = merchantCategoryCode;
        RegisteredDate = registeredDate;
        Status = status;
    }

    #endregion
}