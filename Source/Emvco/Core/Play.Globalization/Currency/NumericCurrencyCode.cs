using Play.Codecs;
using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Globalization.Currency;

public readonly record struct NumericCurrencyCode
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public NumericCurrencyCode(ushort value)
    {
        CheckCore.ForMaximumValue(value.GetNumberOfDigits(), 3, nameof(NumericCurrencyCode));

        _Value = value;
    }

    #endregion

    #region Serialization

    public byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value);

    #endregion

    #region Equality

    public int CompareTo(NumericCurrencyCode other) => _Value.CompareTo(other._Value);

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(NumericCurrencyCode value) => value._Value;

    #endregion
}