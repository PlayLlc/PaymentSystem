using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates.Rules;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Entitiesd;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Aggregates;

public partial class LoyaltyProgram : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly RewardsProgram _RewardsProgram;
    private readonly DiscountsProgram _DiscountsProgram;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    internal LoyaltyProgram(string id, string merchantId, RewardsProgram rewardsProgram, DiscountsProgram discountsProgram)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _RewardsProgram = rewardsProgram;
        _DiscountsProgram = discountsProgram;
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
        _DiscountsProgram = new DiscountsProgram(dto.DiscountsProgram);
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
            RewardsProgram._DefaultPointsRequired, false);
        DiscountsProgram discountsProgram = new DiscountsProgram(GenerateSimpleStringId(), false, Array.Empty<Discount>());

        LoyaltyProgram loyaltyProgram = new LoyaltyProgram(GenerateSimpleStringId(), command.MerchantId, rewardsProgram, discountsProgram);
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantRetriever.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

        loyaltyProgram.Enforce(new UserMustBeActiveToUpdateAggregate<Member>(user));
        loyaltyProgram.Enforce(new AggregateMustBeUpdatedByKnownUser<Member>(command.MerchantId, user));
        loyaltyProgram.Enforce(new MerchantMustBeActiveToCreateAggregate<Member>(merchant));

        loyaltyProgram.Publish(new LoyaltyProgramHasBeenCreated(loyaltyProgram, command.MerchantId));

        return loyaltyProgram;
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task Remove(IRetrieveUsers userRetriever, RemoveLoyaltyProgram command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Member>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Member>(command.MerchantId, user));

        Publish(new LoyaltyProgramHasBeenRemoved(this, _MerchantId));
    }

    public override SimpleStringId GetId() => Id;

    public override LoyaltyProgramDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            DiscountsProgram = _DiscountsProgram.AsDto(),
            RewardsProgram = _RewardsProgram.AsDto()
        };

    #endregion
}