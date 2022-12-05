﻿using Play.Domain.Events;

namespace Play.Payroll.Domain.Aggregates;

public record EmployeePaychecksHaveBeenDelivered : DomainEvent
{
    #region Instance Values

    public readonly Employer Employer;

    #endregion

    #region Constructor

    public EmployeePaychecksHaveBeenDelivered(Employer employer) : base(
        $"The {nameof(Employer)} with the ID: [{employer.Id}] has delivered employee paychecks for this pay period;")
    {
        Employer = employer;
    }

    #endregion
}