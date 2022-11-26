using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.TimeClock.Contracts.Commands;
using Play.TimeClock.Contracts.Dtos;
using Play.TimeClock.Domain.Entities;
using Play.TimeClock.Domain.Enums;
using Play.TimeClock.Domain.Services;
using Play.TimeClock.Domain.ValueObject;

namespace Play.TimeClock.Domain.Aggregates;

public class Employee : Aggregate<SimpleStringId>
{
    #region Instance Values

    /// <summary>
    ///     The last four digits of the user's Social Security Number
    /// </summary>
    private readonly SimpleStringId _MerchantId;

    private readonly SimpleStringId _UserId;

    private readonly Entities.TimeClock _TimeClock;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Employee()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Employee(EmployeeDto dto)
    {
        Id = new SimpleStringId(dto.Id!);
        _MerchantId = new SimpleStringId(dto.MerchantId);
        _UserId = new SimpleStringId(dto.UserId);
        _TimeClock = new Entities.TimeClock(dto.TimeClock);
    }

    /// <exception cref="ValueObjectException"></exception>
    private Employee(string id, string merchantId, string userId, Entities.TimeClock timeClock)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _UserId = new SimpleStringId(userId);
        _TimeClock = timeClock;
    }

    #endregion

    #region Instance Members

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static async Task<Employee> Create(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, IEnsureEmployeeDoesNotExist uniqueEmployeeChecker, CreateEmployee command)
    {
        SimpleStringId employeeId = new(GenerateSimpleStringId());
        Entities.TimeClock timeClock = new Entities.TimeClock(GenerateSimpleStringId(), employeeId, TimeClockStatuses.ClockedOut, null);
        Employee employee = new Employee(employeeId, command.MerchantId, command.UserId, timeClock);

        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantRetriever.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

        employee.Enforce(new UserMustBeActiveToUpdateAggregate<Employee>(user));
        employee.Enforce(new MerchantMustBeActiveToCreateAggregate<Employee>(merchant));
        employee.Enforce(new EmployeeMustNotAlreadyExist(uniqueEmployeeChecker, command.UserId, command.MerchantId));

        employee.Publish(new EmployeeHasBeenCreated(employee, merchant.Id, user.Id));

        return employee;
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task Remove(IRetrieveUsers userRetriever, RemoveEmployee command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employee>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employee>(_MerchantId, user));

        Publish(new EmployeeHasBeenRemoved(this));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task ClockIn(IRetrieveUsers userRetriever, string userId)
    {
        // Enforce
        User user = await userRetriever.GetByIdAsync(userId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employee>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employee>(_MerchantId, user));
        Enforce(new EmployeeMustBeClockedOut(_TimeClock.GetTimeClockStatus()));
        Enforce(new EmployeeMustClockThemselvesInAndOut(user, userId));

        _TimeClock.ClockIn();

        Publish(new EmployeeHasClockedIn(this));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task ClockOut(IRetrieveUsers userRetriever, string userId)
    {
        // Enforce
        User user = await userRetriever.GetByIdAsync(userId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employee>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employee>(_MerchantId, user));
        Enforce(new EmployeeMustBeClockedIn(_TimeClock.GetTimeClockStatus()));
        Enforce(new EmployeeMustClockThemselvesInAndOut(user, userId));

        var timeEntry = _TimeClock.ClockOut(GenerateSimpleStringId);
        Publish(new EmployeeHasClockedOut(this, timeEntry));
    }

    public override SimpleStringId GetId() => Id;

    public override EmployeeDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            UserId = _UserId,
            TimeClock = _TimeClock.AsDto()
        };

    #endregion
}