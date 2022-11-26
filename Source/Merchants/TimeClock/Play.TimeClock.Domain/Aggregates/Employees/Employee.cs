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

    private readonly TimePuncher _TimePuncher;
    private readonly HashSet<TimeEntry> _TimeEntries;
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
        _TimePuncher = new TimePuncher(dto.TimeClock);
        _TimeEntries = dto.TimeEntries.Select(a => new TimeEntry(a)).ToHashSet();
    }

    /// <exception cref="ValueObjectException"></exception>
    private Employee(string id, string merchantId, string userId, TimePuncher timePuncher, IEnumerable<TimeEntry> timeEntries)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _UserId = new SimpleStringId(userId);
        _TimePuncher = timePuncher;
        _TimeEntries = timeEntries.ToHashSet();
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
        TimePuncher timePuncher = new TimePuncher(GenerateSimpleStringId(), employeeId, TimeClockStatuses.ClockedOut, null);
        Employee employee = new Employee(employeeId, command.MerchantId, command.UserId, timePuncher, Array.Empty<TimeEntry>());

        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantRetriever.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

        employee.Enforce(new UserMustBeActiveToUpdateAggregate<Employee>(user));
        employee.Enforce(new MerchantMustBeActiveToCreateAggregate<Employee>(merchant));
        employee.Enforce(new EmployeeMustNotAlreadyExist(uniqueEmployeeChecker, user.Id, merchant.Id));

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
    public async Task ClockIn(IRetrieveUsers userRetriever, UpdateTimeClock command)
    {
        // Enforce
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employee>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employee>(_MerchantId, user));
        Enforce(new EmployeeMustBeClockedOut(_TimePuncher.GetTimeClockStatus()));
        Enforce(new EmployeeMustClockThemselvesInAndOut(user, _UserId));

        _TimePuncher.ClockIn();

        Publish(new EmployeeHasClockedIn(this));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task ClockOut(IRetrieveUsers userRetriever, UpdateTimeClock command)
    {
        // Enforce
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employee>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employee>(_MerchantId, user));
        Enforce(new EmployeeMustBeClockedIn(_TimePuncher.GetTimeClockStatus()));
        Enforce(new EmployeeMustClockThemselvesInAndOut(user, _UserId));
        TimeEntry timeEntry = _TimePuncher.ClockOut(GenerateSimpleStringId);
        _TimeEntries.Add(timeEntry);
        Publish(new EmployeeHasClockedOut(this, timeEntry));
    }

    public async Task EditTimeEntry(IRetrieveUsers userRetriever, EditTimeEntry command)
    {
        User user = await userRetriever.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Employee>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Employee>(_MerchantId, user));

        _TimeEntries.First(a => a.Id == command.TimeEntryId);
    }

    public override SimpleStringId GetId() => Id;

    public override EmployeeDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            UserId = _UserId,
            TimeClock = _TimePuncher.AsDto()
        };

    #endregion
}