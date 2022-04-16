using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber;

/// <summary>
///     Number of non-zero bits in PUNATC(Track2) – NATC(Track2)
/// </summary>
/// <remarks>EMV Book C-2 Section A.1.114</remarks>
public readonly record struct NumberOfNonZeroBits
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public NumberOfNonZeroBits(PositionOfCardVerificationCode3Track1 pcvc)
    {
        _Value = (byte) pcvc.GetSetBitCount();
    }

    public NumberOfNonZeroBits(NumericApplicationTransactionCounterTrack1 value)
    {
        _Value = (byte) value.GetSetBitCount();
    }

    public NumberOfNonZeroBits(PunatcTrack2 punatc, NumericApplicationTransactionCounterTrack2 natc)
    {
        _Value = (byte) (punatc.GetSetBitCount() - natc.GetSetBitCount());
    }

    public NumberOfNonZeroBits(PositionOfCardVerificationCode3Track2 value)
    {
        _Value = (byte) value.GetSetBitCount();
    }

    public NumberOfNonZeroBits(PunatcTrack1 value)
    {
        _Value = (byte) value.GetSetBitCount();
    }

    public NumberOfNonZeroBits(UnpredictableNumber value)
    {
        _Value = (byte) value.GetSetBitCount();
    }

    public NumberOfNonZeroBits(UnpredictableNumberNumeric value)
    {
        _Value = (byte) value.GetSetBitCount();
    }

    private NumberOfNonZeroBits(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public NumberOfNonZeroBits AsPlusFiveModuloTen() => new((byte) ((_Value + 5) % 10));

    /// <summary>
    ///     Validates whether the Track 2 object that initialized this object are in a valid range
    /// </summary>
    public bool IsInRange() => _Value is < 0 or > 8;

    /// <summary>
    ///     Validates whether the Track 1 objects provided in the arguments are valid
    /// </summary>
    public bool IsInRange(PunatcTrack1 punatc, NumericApplicationTransactionCounterTrack1 natc) =>
        IsInRange() && ((punatc.GetSetBitCount() - natc.GetSetBitCount()) != _Value);

    #endregion

    #region Operator Overrides

    public static implicit operator byte(NumberOfNonZeroBits value) => value._Value;
    public static bool operator >(byte left, NumberOfNonZeroBits right) => left > right._Value;
    public static bool operator <(byte left, NumberOfNonZeroBits right) => left < right._Value;
    public static bool operator >=(byte left, NumberOfNonZeroBits right) => left >= right._Value;
    public static bool operator <=(byte left, NumberOfNonZeroBits right) => left <= right._Value;
    public static bool operator ==(byte left, NumberOfNonZeroBits right) => left == right._Value;
    public static bool operator !=(byte left, NumberOfNonZeroBits right) => left != right._Value;
    public static bool operator >(NumberOfNonZeroBits left, byte right) => left._Value > right;
    public static bool operator <(NumberOfNonZeroBits left, byte right) => left._Value < right;
    public static bool operator >=(NumberOfNonZeroBits left, byte right) => left._Value >= right;
    public static bool operator <=(NumberOfNonZeroBits left, byte right) => left._Value <= right;
    public static bool operator ==(NumberOfNonZeroBits left, byte right) => left._Value == right;
    public static bool operator !=(NumberOfNonZeroBits left, byte right) => left._Value != right;

    #endregion
}