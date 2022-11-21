using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entities;

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
        Id = new SimpleStringId(dto.Id);
        MerchantId = new SimpleStringId(dto.MerchantId);
        IsActive = dto.IsActive;
    }

    /// <exception cref="ValueObjectException"></exception>
    public User(string id, string merchantId, bool isActive)
    {
        Id = new SimpleStringId(id);
        MerchantId = new SimpleStringId(merchantId);
        IsActive = isActive;
    }

    #endregion

    #region Instance Members

    public bool DoesUserBelongToMerchant(string merchantId)
    {
        return MerchantId == merchantId;
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override UserDto AsDto()
    {
        return new UserDto()
        {
            Id = Id,
            MerchantId = MerchantId
        };
    }

    #endregion
}