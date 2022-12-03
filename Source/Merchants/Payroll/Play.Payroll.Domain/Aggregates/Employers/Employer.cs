using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Domain.Aggregates.Employers;

public class Employer : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    // PaySchedule
    private readonly IEnumerable<Employee> _Employees;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    internal Employer(string id, string merchantId) 
    {
        Id = new(id);
        _MerchantId = new(merchantId);
    }

    // Constructor for Entity Framework
    private Employer()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Employer(EmployerDto dto)
    {
        Id = new(dto.Id);
        _MerchantId = new(dto.MerchantId);
    }

    #endregion

    #region Instance Members

    public async Task CutPaychecks(IISendAchTransfers achClient, CutChecks commands)
    {
        PayPeriod payPeriod = new(commands.PayPeriod);
          
        foreach (var employee in _Employees)
        { 
            employee.AddPaycheck(CutPaycheck(payPeriod, employee));
            await employee.TryDispursingUndeliveredChecks(achClient).ConfigureAwait(false);
        }   
    }

    private Paycheck CutPaycheck(PayPeriod payPeriod, Employee employee)
    {   
        TimeSheet timeSheet = TimeSheet.Create(GenerateSimpleStringId(), employee.Id, payPeriod, employee.GetTimeEntries(payPeriod)); 
        Money earnedWage = employee.CalculatePaycheckEarnings(timeSheet);
        return Paycheck.Create(GenerateSimpleStringId(), employee.Id, earnedWage, timeSheet, payPeriod);
    }


    public Paycheck GeneratePaycheck(TimeSheet timeSheet, PayPeriod payPeriod)
    {
        Paycheck.Create()
    }
    //public async Task GeneratePaychecks()
    //{
    //    PaydaySchedule paySchedule = null;
    //    foreach (var employee in _Employees)
    //        employee.AddPaycheck(() => new SimpleStringId(GenerateSimpleStringId()), null);
    //}



    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static async Task<Employer> Create(CreateEmployer command)
    {
        //Money rewardAmount = new Money(RewardProgram.DefaultRewardAmount, command.NumericCurrencyCode);
        //RewardProgram rewardProgram = new RewardProgram(GenerateSimpleStringId(), rewardAmount, RewardProgram.DefaultPointsPerDollar,
        //    RewardProgram.DefaultPointsRequired, false);
        //DiscountProgram discountProgram = new DiscountProgram(GenerateSimpleStringId(), false, Array.Empty<Discount>());

        //Employer employer = new Employer(GenerateSimpleStringId(), command.MerchantId, rewardProgram, discountProgram);
        //User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        //Merchant merchant = await merchantRetriever.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

        //employer.Enforce(new UserMustBeActiveToUpdateAggregate<Member>(user));
        //employer.Enforce(new AggregateMustBeUpdatedByKnownUser<Member>(command.MerchantId, user));
        //employer.Enforce(new MerchantMustBeActiveToCreateAggregate<Member>(merchant));

        //employer.Publish(new LoyaltyProgramHasBeenCreated(employer, command.MerchantId));

        //return employer;
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