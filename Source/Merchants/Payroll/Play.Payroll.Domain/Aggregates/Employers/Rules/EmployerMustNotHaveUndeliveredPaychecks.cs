using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;

namespace Play.Payroll.Domain.Aggregates;

public class EmployerMustNot
    #region Instance ValuessinessRule<Employer, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Employer)} cannot be removed b

    #endregion

    #region Constructorndelivered paychecks;";

    #endregion

    #region Constructor

    internal EmployerMustNotHaveUndeliveredPaychecks(Employer employer)
 

    #endregion

    #region Instance Membersaychecks();
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override EmployerHasUndeliveredPaychecks CreateBusinessR

    #endregion
ent(Employer aggregate) => new(aggregate, this);

    #endregion
}