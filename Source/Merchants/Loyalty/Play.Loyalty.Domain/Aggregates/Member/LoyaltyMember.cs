using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Entitiesddd;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Domain.Aggregates;

public class LoyaltyMember : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;
    private readonly Name _Name;
    private readonly RewardsNumber _RewardsNumber;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private LoyaltyMember()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyMember(LoyaltyMemberDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _MerchantId = new SimpleStringId(dto.MerchantId);
        _Name = new Name(dto.Name);
        _RewardsNumber = new RewardsNumber(dto.RewardsNumber);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyMember(string id, string merchantId, string name, string rewardsNumber)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _Name = new Name(name);
        _RewardsNumber = new RewardsNumber(rewardsNumber);
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override LoyaltyMemberDto AsDto()
    {
        return new LoyaltyMemberDto
        {
            Id = Id,
            MerchantId = _MerchantId,
            Name = _Name,
            RewardsNumber = _RewardsNumber.Value
        };
    }

    /// <exception cref="ValueObjectException"></exception>
    public static LoyaltyMember Create(CreateLoyaltyMember command)
    {
        var loyaltyMember = new LoyaltyMember(GenerateSimpleStringId(), command.MerchantId, command.Name, command.RewardsNumber);

        // Enforce Rules
        // Publish Domain Event

        return loyaltyMember;
    }

    public static void Remove()
    {
        // Enforce Rules

        // Publish Domain Event
    }

    #endregion
}