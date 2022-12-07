using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Entities;

public class User : Entity<SimpleStringId>
{
    #region Instance Values

    public readonly SimpleStringId MerchantId;
    public readonly bool IsActive;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private User()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public User(UserDto dto)
    {
        Id = new(dto.Id);
        MerchantId = new(dto.MerchantId);
        IsActive = dto.IsActive;
    }

    /// <exception cref="ValueObjectException"></exception>
    public User(string id, string merchantId, bool isActive)
    {
        Id = new(id);
        MerchantId = new(merchantId);
        IsActive = isActive;
    }

    #endregion

    #region Instance Members

    public bool DoesUserBelongToMerchant(string merchantId) => MerchantId == merchantId;

    public override SimpleStringId GetId() => Id;

    public override UserDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = MerchantId
        };

    #endregion
}