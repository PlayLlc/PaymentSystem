using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Globalization.Currency;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.ValueObject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Payroll.Domain.Entities;

public abstract class PaydaySchedule : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly RecurrenceType _Recurrence;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    protected PaydaySchedule(SimpleStringId id, RecurrenceType recurrence)
    {
        Id = id;
        _Recurrence = recurrence;
    }

    #endregion

    #region Instance Members

    public abstract PayPeriod GetPayPeriod();

    #endregion
}