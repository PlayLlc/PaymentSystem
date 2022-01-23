using Play.Emv.DataElements;
using Play.Random;

namespace Play.Emv.Sessions;

/// <summary>
///     A value that can be used to correlate an Application Session passed between physical or logical boundaries
/// </summary>
public readonly struct TransactionSessionId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public TransactionSessionId(TransactionType transactionType)
    {
        _Value = GetConstructorValueTransactionType(transactionType) | Randomize.Integers.ULong();
    }

    #endregion

    #region Instance Members

    private static ulong GetConstructorValueTransactionType(TransactionType transactionType) => (ulong) transactionType << (7 * 8);
    public TransactionType GetTransactionType() => new((byte) (_Value >> (7 * 8)));

    #endregion

    #region Equality

    public bool Equals(TransactionSessionId transactionSessionId) => transactionSessionId._Value == _Value;
    public bool Equals(TransactionSessionId x, TransactionSessionId y) => x.Equals(y);
    public override bool Equals(object? obj) => obj is TransactionSessionId transactionSessionId && Equals(transactionSessionId);
    public int GetHashCode(TransactionSessionId other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 543287;

        return unchecked(hash * _Value.GetHashCode());
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(TransactionSessionId left, TransactionSessionId right) => left._Value == right._Value;
    public static explicit operator ulong(TransactionSessionId value) => value._Value;
    public static bool operator !=(TransactionSessionId left, TransactionSessionId right) => !(left == right);

    #endregion
}