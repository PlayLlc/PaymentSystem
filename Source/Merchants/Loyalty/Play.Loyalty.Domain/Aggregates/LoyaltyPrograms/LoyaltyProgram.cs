using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.ValueObjects;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Entitiesddd;

namespace Play.Loyalty.Domain.Aggregates;

public partial class LoyaltyProgram : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly RewardProgram _RewardProgram;
    private readonly HashSet<Discount> _Discounts;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private LoyaltyProgram()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyProgram(LoyaltyProgramDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _MerchantId = new SimpleStringId(dto.MerchantId);
        _RewardProgram = new RewardProgram(dto.RewardProgram);
        _Discounts = new HashSet<Discount>(dto.Discounts.Select(a => new Discount(a)));
    }

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyProgram(string id, string merchantId, RewardProgram rewardProgram, IEnumerable<Discount> discounts)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _RewardProgram = rewardProgram;
        _Discounts = discounts.ToHashSet();
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override LoyaltyProgramDto AsDto()
    {
        return new LoyaltyProgramDto
        {
            Id = Id,
            MerchantId = _MerchantId,
            Discounts = _Discounts.Select(a => a.AsDto()),
            RewardProgram = _RewardProgram.AsDto()
        };
    }

    #endregion
}