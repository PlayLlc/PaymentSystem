using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Identity.Domain.Enumss;
using Play.TimeClock.Contracts.Dtos;
using Play.TimeClock.Domain.Aggregates._Shared.Rules;
using Play.TimeClock.Domain.Aggregates.Employees.DomainEvents;
using Play.TimeClock.Domain.Aggregates.Employees.Rules;
using Play.TimeClock.Domain.Entities;
using Play.TimeClock.Domain.Services;
using Play.TimeClock.Domain.ValueObject;

namespace Play.TimeClock.Domain.Aggregates.Employees;

public class Employee : Aggregate<SimpleStringId>
{
    #region Instance Values

    /// <summary>
    ///     The last four digits of the user's Social Security Number
    /// </summary>
    private readonly SimpleStringId _MerchantId;

    private readonly SimpleStringId _UserId;

    private readonly CompensationType _CompensationType;
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
        _CompensationType = new CompensationType(dto.CompensationType);
        _TimeClock = new Entities.TimeClock(dto.TimeClock);
    }

    /// <exception cref="ValueObjectException"></exception>
    private Employee(string id, string merchantId, CompensationTypes compensationType, Entities.TimeClock timeClock)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _CompensationType = new CompensationType(compensationType);
        _TimeClock = timeClock;
    }

    #endregion

    #region Instance Members

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

        _TimeClock.ClockOut(GenerateSimpleStringId);

        Publish(new EmployeeHasClockedIn(this));
    }

    public override SimpleStringId GetId() => Id;

    public override EmployeeDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            CompensationType = _CompensationType
        };

    #endregion
}