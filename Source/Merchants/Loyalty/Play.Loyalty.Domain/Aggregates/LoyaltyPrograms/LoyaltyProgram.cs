using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Aggregates;

public partial class LoyaltyProgram : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly RewardsProgram _RewardsProgram;
    private readonly HashSet<Discount> _Discounts;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyProgram(string id, string merchantId, RewardsProgram rewardsProgram, IEnumerable<Discount> discounts)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _RewardsProgram = rewardsProgram;
        _Discounts = discounts.ToHashSet();
    }

    // Constructor for Entity Framework
    private LoyaltyProgram()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyProgram(LoyaltyProgramDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _MerchantId = new SimpleStringId(dto.MerchantId);
        _RewardsProgram = new RewardsProgram(dto.RewardsProgram);
        _Discounts = new HashSet<Discount>(dto.Discounts.Select(a => new Discount(a)));
    }

    #endregion

    #region Instance Members

    internal bool IsRewardProgramActive() => _RewardsProgram.IsActive();

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static async Task<LoyaltyProgram> Create(IRetrieveMerchants merchantRetriever, IRetrieveUsers userRetriever, CreateLoyaltyProgram command)
    {
        Money rewardAmount = new Money(RewardsProgram._DefaultRewardAmount, command.NumericCurrencyCode);
        RewardsProgram rewardsProgram = new RewardsProgram(GenerateSimpleStringId(), rewardAmount, RewardsProgram._DefaultPointsPerDollar,
            RewardsProgram._DefaultPointsRequired);
        LoyaltyProgram loyaltyProgram = new LoyaltyProgram(GenerateSimpleStringId(), command.MerchantId, rewardsProgram, Array.Empty<Discount>());
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantRetriever.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

        loyaltyProgram.Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyMember>(user));
        loyaltyProgram.Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyMember>(command.MerchantId, user));
        loyaltyProgram.Enforce(new MerchantMustBeActiveToCreateAggregate<LoyaltyMember>(merchant));

        loyaltyProgram.Publish(new LoyaltyProgramHasBeenCreated(loyaltyProgram, command.MerchantId));

        return loyaltyProgram;
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task Remove(IRetrieveUsers userRetriever, RemoveLoyaltyProgram command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<LoyaltyMember>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<LoyaltyMember>(command.MerchantId, user));

        Publish(new LoyaltyProgramHasBeenRemoved(this, _MerchantId));
    }

    public override SimpleStringId GetId() => Id;

    public override LoyaltyProgramDto AsDto()
    {
        return new LoyaltyProgramDto
        {
            Id = Id,
            MerchantId = _MerchantId,
            Discounts = _Discounts.Select(a => a.AsDto()),
            RewardsProgram = _RewardsProgram.AsDto()
        };
    }

    #endregion
}