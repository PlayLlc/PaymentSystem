using Play.Core;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Services;

public interface IISendAchTransfers
{
    #region Instance Members

    public Task<Result> SendPaycheck(string employeeId, DateTimeUtc dateIssued, Money amount, CheckingAccount checkingAccount);

    public Task<Result> SendPaycheck(DirectDepositDto directDeposit, PaycheckDto paycheck);
    public Task<Result> SendPaychecks(DirectDepositDto directDeposit, IEnumerable<PaycheckDto> paychecks);

    #endregion
}