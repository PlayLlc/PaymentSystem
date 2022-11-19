using Play.Core;
using Play.Core.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Enums;

namespace Play.Inventory.Domain.ValueObjects;

public record StockAction : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private StockAction()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public StockAction(string value) : base(value)
    {
        if (!StockActions.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(StockAction)} provided was invalid: [{value}]");
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>

    #endregion

    #region Operator Overrides

    public static implicit operator string(StockAction value)
    {
        return value.Value;
    }

    public static implicit operator StockActions(StockAction value)
    {
        if (StockActions.Empty.TryGet(value.Value, out EnumObjectString? result))
            throw new PlayInternalException($"This should never happen");

        return (StockActions) result!;
    }

    #endregion
}