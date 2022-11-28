using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Services;

public interface IISendAchTransfers
{
    #region Instance Members

    public Task<bool> SendPaycheck(SimpleStringId employeeId, DateTimeUtc dateIssued, Money amount, DirectDeposit directDeposit);

    #endregion
}