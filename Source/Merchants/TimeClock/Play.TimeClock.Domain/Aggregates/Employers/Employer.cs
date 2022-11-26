using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.TimeClock.Domain.Entities;

namespace Play.TimeClock.Domain.Aggregates;

public class Employer : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;
    private readonly HashSet<Employee> _Employees;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Employer()
    { }

    #endregion

    #region Instance Members

    //public static async Task<Employer> Create()
    //{
    //    Money rewardAmount = new Money(RewardProgram.DefaultRewardAmount, command.NumericCurrencyCode);
    //    RewardProgram rewardProgram = new RewardProgram(GenerateSimpleStringId(), rewardAmount, RewardProgram.DefaultPointsPerDollar,
    //        RewardProgram.DefaultPointsRequired, false);
    //    DiscountProgram discountProgram = new DiscountProgram(GenerateSimpleStringId(), false, Array.Empty<Discount>());

    //    Employer employer = new Employer(GenerateSimpleStringId(), command.MerchantId, rewardProgram, discountProgram);
    //    User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
    //    Merchant merchant = await merchantRetriever.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

    //    employer.Enforce(new UserMustBeActiveToUpdateAggregate<Member>(user));
    //    employer.Enforce(new AggregateMustBeUpdatedByKnownUser<Member>(command.MerchantId, user));
    //    employer.Enforce(new MerchantMustBeActiveToCreateAggregate<Member>(merchant));

    //    employer.Publish(new LoyaltyProgramHasBeenCreated(employer, command.MerchantId));

    //    return employer;
    //}

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