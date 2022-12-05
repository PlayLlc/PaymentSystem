using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.Entitieses.Employers.DomainEvents._Employees;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Aggregates;

public class EmployeeMu
    #region Instance Values : BusinessRule<Employer, SimpleStringId>
{
    #region Instance Values

   
        eadonly bool _IsValid;

    public override string Message =>
        $"The {nameof(Employer)} cannot remove the {nameof(Employee)} because th

    #endregion

    #region Constructor have yet to be delivered;";

    #endregion

    #region Constructor

    internal EmployeeMustNotHaveUndeliveredPaychecks(Employee employee)
  

    #endregion

    #region Instance Membersychecks().Any();
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployeeHasUndeliveredPaychecks CreateBusi

    #endregion
ainEvent(Employer aggregate) => new(aggregate, this);

    #endregion
}