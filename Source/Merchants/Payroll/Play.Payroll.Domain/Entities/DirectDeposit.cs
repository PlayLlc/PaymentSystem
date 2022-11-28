using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Loyalty.Contracts.Dtosd;

namespace Play.Payroll.Domain.Entities;

public class DirectDeposit : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly CheckingAccount _CheckingAccount;
    private readonly SimpleStringId _EmployeeId;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private DirectDeposit()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal DirectDeposit(DirectDepositDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _EmployeeId = new SimpleStringId(dto.EmployeeId);
        _CheckingAccount = new CheckingAccount(dto.CheckingAccount);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal DirectDeposit(string id, string employeeId, CheckingAccount checkingAccount)
    {
        Id = new SimpleStringId(id);
        _EmployeeId = new SimpleStringId(employeeId);
        _CheckingAccount = checkingAccount;
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override DirectDepositDto AsDto() =>
        new()
        {
            Id = Id,
            EmployeeId = _EmployeeId,
            CheckingAccount = _CheckingAccount.AsDto()
        };

    #endregion
}