using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Payroll.Contracts.Dtos;

namespace Play.Payroll.Domain.Entities;

public class CheckingAccount : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly RoutingNumber _RoutingNumber;
    private readonly AccountNumber _AccountNumber;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private CheckingAccount()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal CheckingAccount(CheckingAccountDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _RoutingNumber = new RoutingNumber(dto.RoutingNumber);
        _AccountNumber = new AccountNumber(dto.AccountNumber);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal CheckingAccount(string id, string routingNumber, string accountNumber)
    {
        Id = new SimpleStringId(id);
        _RoutingNumber = new RoutingNumber(routingNumber);
        _AccountNumber = new AccountNumber(accountNumber);
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override CheckingAccountDto AsDto() =>
        new()
        {
            Id = Id,
            AccountNumber = _AccountNumber,
            RoutingNumber = _RoutingNumber
        };

    #endregion
}