using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core;
using Play.Domain.Common.Entities;
using Play.Globalization.Currency;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Application.Services;

public class AchClient : ISendAchTransfers
{
    #region Instance Members

    public Task<Result> SendPaycheck(string employeeId, DateTimeUtc dateIssued, Money amount, CheckingAccount checkingAccount) =>
        throw new NotImplementedException();

    public Task<Result> SendPaycheck(DirectDepositDto directDeposit, PaycheckDto paycheck) => throw new NotImplementedException();

    public Task<Result> SendPaychecks(DirectDepositDto directDeposit, IEnumerable<PaycheckDto> paychecks) => throw new NotImplementedException();

    #endregion
}