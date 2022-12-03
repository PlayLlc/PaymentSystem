using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Services;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Aggregates.Employers.Rules;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Domain.Aggregates.Employers;

public class Employer : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    // PaySchedule
    private readonly HashSet<Employee> _Employees;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    internal Employer(string id, string merchantId, IEnumerable<Employee> employees)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _Employees = employees.ToHashSet();
    }

    // Constructor for Entity Framework
    private Employer()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Employer(EmployerDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _MerchantId = new SimpleStringId(dto.MerchantId);
    }

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

    #endregion

    #region Create/Remove

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static async Task<Employer> Create(IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, CreateOrRemoveEmployer command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));
        var employer = new Employer(GenerateSimpleStringId(), command.MerchantId, Array.Empty<Employee>());
        employer.Enforce(new MerchantMustBeActiveToCreateAggregate<Employer>(merchant));
        employer.Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        employer.Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(command.MerchantId, user));

        // Enforce

        employer.Publish(new EmployerHasBeenCreated(employer, command.MerchantId, command.UserId));

        return employer;
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task Remove(IRetrieveUsers userRetriever, CreateOrRemoveEmployer command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(command.MerchantId, user));

        // Enforce

        Publish(new EmployerHasBeenRemoved(this, command.MerchantId, command.UserId));
    }

    #endregion

    #region Employee

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task CreateEmployee(IRetrieveUsers userRetriever, CreateEmployee command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(user.MerchantId, user));

        // Enforce
        Compensation compensation = new(GenerateSimpleStringId(), command.CompensationType, command.CompensationRate);

        if (!_Employees.Add(Employee.Create(GenerateSimpleStringId(), command.UserId, compensation)))
            return;
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveEmployee(IRetrieveUsers userRetriever, RemoveEmployee command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employer>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employer>(user.MerchantId, user));
        Enforce(new EmployeeMustExist(command.EmployeeId, _Employees));
        var employee = _Employees.First(a => a.Id == command.EmployeeId);
        Enforce(new EmployeeMustNotHaveUndeliveredPaychecks(employee));

        // Enforce
        _Employees.RemoveWhere(a => a.Id == command.EmployeeId);

        // Publish
    }

    #endregion

    #region Paychecks

    public async Task CutPaychecks(IISendAchTransfers achClient, CutChecks commands)
    {
        // Enforce
        PayPeriod payPeriod = new(commands.PayPeriod);

        // HACK: TRANSACTIONAL CONSISTENCY!!!!!!
        // BUG: TRANSACTIONAL CONSISTENCY!!!!!
        // HACK: TRANSACTIONAL CONSISTENCY!!!!!!
        foreach (var employee in _Employees)
        {
            employee.AddPaycheck(CutPaycheck(payPeriod, employee));
            await employee.TryDispursingUndeliveredChecks(achClient).ConfigureAwait(false);
        }

        // Publish
    }

    private Paycheck CutPaycheck(PayPeriod payPeriod, Employee employee)
    {
        TimeSheet timeSheet = TimeSheet.Create(GenerateSimpleStringId(), employee.Id, payPeriod, employee.GetTimeEntries(payPeriod));
        Money earnedWage = employee.CalculatePaycheckEarnings(timeSheet);

        return Paycheck.Create(GenerateSimpleStringId(), employee.Id, earnedWage, timeSheet, payPeriod);
    }

    #endregion
}