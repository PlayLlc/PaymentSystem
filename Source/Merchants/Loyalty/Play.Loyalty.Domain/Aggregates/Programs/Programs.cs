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

public partial class Programs : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly RewardProgram _RewardProgram;
    private readonly DiscountProgram _DiscountProgram;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    internal Programs(string id, string merchantId, RewardProgram rewardProgram, DiscountProgram discountProgram)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _RewardProgram = rewardProgram;
        _DiscountProgram = discountProgram;
    }

    // Constructor for Entity Framework
    private Programs()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Programs(LoyaltyProgramDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _MerchantId = new SimpleStringId(dto.MerchantId);
        _RewardProgram = new RewardProgram(dto.RewardsProgram);
        _DiscountProgram = new DiscountProgram(dto.DiscountsProgram);
    }

    #endregion

    #region Instance Members

    internal bool IsRewardProgramActive() => _RewardProgram.IsActive();

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static async Task<Programs> Create(IRetrieveMerchants merchantRetriever, IRetrieveUsers userRetriever, CreateLoyaltyProgram command)
    {
        Money rewardAmount = new Money(RewardProgram._DefaultRewardAmount, command.NumericCurrencyCode);
        RewardProgram rewardProgram = new RewardProgram(GenerateSimpleStringId(), rewardAmount, RewardProgram._DefaultPointsPerDollar,
            RewardProgram._DefaultPointsRequired, false);
        DiscountProgram discountProgram = new DiscountProgram(GenerateSimpleStringId(), false, Array.Empty<Discount>());

        Programs programs = new Programs(GenerateSimpleStringId(), command.MerchantId, rewardProgram, discountProgram);
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantRetriever.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

        programs.Enforce(new UserMustBeActiveToUpdateAggregate<Member>(user));
        programs.Enforce(new AggregateMustBeUpdatedByKnownUser<Member>(command.MerchantId, user));
        programs.Enforce(new MerchantMustBeActiveToCreateAggregate<Member>(merchant));

        programs.Publish(new LoyaltyProgramHasBeenCreated(programs, command.MerchantId));

        return programs;
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
            DiscountsProgram = _DiscountProgram.AsDto(),
            RewardsProgram = _RewardProgram.AsDto()
        };

    #endregion
}