using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

/// <summary>
///     The Account Number associated to Issuer Card
/// </summary>
public readonly struct PrimaryAccountNumber
{
    #region Static Metadata

    private const byte _MaxByteLength = 10;
    private const byte _MaxDigitLength = 19;

    #endregion

    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public PrimaryAccountNumber(ReadOnlySpan<byte> value)
    {
        Validate(value);
        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Returns the <see cref="PrimaryAccountNumber" /> as a byte array in CompressedNumeric format
    /// </summary>
    /// <returns></returns>
    public byte[] AsCompressedNumericByteArray() => _Value[..(GetNumberOfDigits() + (GetNumberOfDigits() % 2))];

    /// <summary>
    ///     Returns the <see cref="PrimaryAccountNumber" /> as a byte array in Numeric format
    /// </summary>
    /// <returns></returns>
    public byte[] AsNumericByteArray()
    {
        int numberOfDigits = GetNumberOfDigits();

        if (numberOfDigits.IsEven())
            return _Value[..numberOfDigits];

        return _Value[..numberOfDigits].ShiftRightOneNibble();
    }

    public int GetByteCount() => _Value.Length;
    public int GetNumberOfDigits() => PlayEncoding.CompressedNumeric.GetNumberOfDigits(_Value);

    private static void Validate(ReadOnlySpan<byte> value)
    {
        if (!PlayEncoding.CompressedNumeric.IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was not a valid {nameof(CompressedNumeric)} format");
        }

        if (value.Length > _MaxByteLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be less than {_MaxByteLength} bytes in length to instantiate a {nameof(PrimaryAccountNumber)} object");
        }

        if (!PlayEncoding.Numeric.IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be a valid numeric digit to instantiate a {nameof(PrimaryAccountNumber)} object");
        }

        if (PlayEncoding.Numeric.GetCharCount(value) > _MaxDigitLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be contain less than than {_MaxByteLength} digits to instantiate a {nameof(PrimaryAccountNumber)} object");
        }
    }

    private static void Validate(ReadOnlySpan<char> value)
    {
        if (!PlayEncoding.Numeric.IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be a valid numeric digit to instantiate a {nameof(PrimaryAccountNumber)} object");
        }

        if (value.Length > _MaxDigitLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be contain less than than {_MaxByteLength} digits to instantiate a {nameof(PrimaryAccountNumber)} object");
        }
    }

    #endregion

    #region Equality

    public bool Equals(PrimaryAccountNumber other) => _Value == other._Value;
    public override bool Equals(object? obj) => obj is PrimaryAccountNumber primaryAccountNumber && Equals(primaryAccountNumber);
    public int GetHashCode(PrimaryAccountNumber other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 227;

        unchecked
        {
            int result = 0;
            for (int i = 0; i < _Value.Length; i++)
                result += hash * _Value[i].GetHashCode();

            return result;
        }
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(PrimaryAccountNumber left, PrimaryAccountNumber right) => left._Value == right._Value;
    public static bool operator !=(PrimaryAccountNumber left, PrimaryAccountNumber right) => !(left == right);

    #endregion
}