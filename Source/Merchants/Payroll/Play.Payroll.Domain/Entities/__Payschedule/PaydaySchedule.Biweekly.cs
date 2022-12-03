using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Payroll.Contracts.Commands;

namespace Play.Payroll.Domain.Entities;

public partial class PaydaySchedule : Entity<SimpleStringId>
{
    #region Instance Members

    public static PaydaySchedule Create(CreateBiweeklyPaySchedule command)
    { }

    private PayPeriod GetNextBiweeklyPayPeriod()
    { }

    #endregion
}