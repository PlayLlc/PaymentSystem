using Play.Domain;
using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Domain.Entities;

public class WeeklySchedule : PaydaySchedule
{
    #region Constructor

    public WeeklySchedule(SimpleStringId id, RecurrenceType recurrenceType) : base(id, recurrenceType)
    { }

    #endregion

    #region Instance Members

    public override IDto AsDto() => throw new NotImplementedException();

    public override SimpleStringId GetId() => Id;

    public override PayPeriod GetPayPeriod() => throw new NotImplementedException();

    #endregion
}