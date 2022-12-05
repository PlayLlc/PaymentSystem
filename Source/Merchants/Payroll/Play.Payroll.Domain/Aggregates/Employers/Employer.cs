using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.Services;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Aggregates;

public partial class Employer : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;
    private readonly PaydaySchedule _PaydaySchedule;
    private readonly HashSet<Employee> _Employees;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    internal Employer(string id, string merchantId, PaydaySchedule paydaySchedule, IEnumerable<Employee> employees)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _PaydaySchedule = paydaySchedule;
        _Employees = employees.ToHashSet();
    }

    // Constructor for Entity Framework
    private Employer()
    { }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override EmployerDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            Employees = _Employees.Select(a => a.AsDto())
        };

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static async Task<Employer> Create(IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, CreateEmployer command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));
        DayOfTheWeek? weeklyPayday = command.WeeklyPayday is null ? null : new DayOfTheWeek(command.WeeklyPayday.Value);
        DayOfTheMonth? monthlyPayday = command.WeeklyPayday is null ? null : new DayOfTheMonth(command.MonthlyPayday!.Value);
        DayOfTheMonth? secondMonthlyPayday = command.WeeklyPayday is null ? null : new DayOfTheMonth(command.SecondMonthlyPayday!.Value);
        PaydaySchedule paydaySchedule = PaydaySchedule.Create(GenerateSimpleStringId(), new PaydayRecurrence(command.PaydayRecurrence), weeklyPayday,
            monthlyPayday, secondMonthlyPayday);
        var employer = new Employer(GenerateSimpleStringId(), command.MerchantId, paydaySchedule, Array.Empty<Employee>());
        employer.Enforce(new MerchantMustBeActiveToCreateAggregate<Employer>(merchant));
        employer.Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        employer.Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(command.MerchantId, user));

        employer.Publish(new EmployerHasBeenCreated(employer, command.MerchantId, command.UserId));

        return employer;
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task Remove(IRetrieveUsers userRetriever, RemoveEmployer command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(command.MerchantId, user));

        // TODO: How will this work in practice? We force merchants to go to the portal and mark all as delivered or deliver all direct deposit?
        Enforce(new EmployerMustNotHaveUndeliveredPaychecks(this));

        Publish(new EmployerHasBeenRemoved(this, command.MerchantId, command.UserId));
    }

    #endregion
}