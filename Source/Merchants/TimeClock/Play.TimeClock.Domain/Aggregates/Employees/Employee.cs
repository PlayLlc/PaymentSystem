using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Identity.Domain.Enumss;
using Play.Identity.Domain.ValueObjectsd;
using Play.TimeClock.Contracts.Dtos;

namespace Play.TimeClock.Domain.Entities;

public class Employee : Aggregate<SimpleStringId>
{
    #region Instance Values

    /// <summary>
    ///     The last four digits of the user's Social Security Number
    /// </summary>
    private readonly SimpleStringId _MerchantId;

    private readonly CompensationType _CompensationType;
    private readonly TimeClock _TimeClock;
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
        _TimeClock = new TimeClock(dto.TimeClock);
    }

    /// <exception cref="ValueObjectException"></exception>
    private Employee(string id, string merchantId, CompensationTypes compensationType, TimeClock timeClock)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _CompensationType = new CompensationType(compensationType);
        _TimeClock = timeClock;
    }

    #endregion

    #region Instance Members

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