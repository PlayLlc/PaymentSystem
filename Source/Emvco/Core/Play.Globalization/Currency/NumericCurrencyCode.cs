using System;

using Play.Codecs;

namespace Play.Globalization.Currency;

public readonly record struct NumericCurrencyCode
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public NumericCurrencyCode(ushort value, bool validateCurrency = true)
    {
        // TODO: This validation causes circular references. Let's try and create an EnumObject or something similar that allows some validation logic
        // HACK: Changing constructor signature to have default flag in order to bypass the circular reference to build up the dictionary(repo).
      
        if (validateCurrency && !CurrencyCodeRepository.IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"The argument {nameof(value)} must be 3 digits or less according to ISO 4217");
        }

        _Value = value;
    }

    #endregion

    #region Equality

    public int CompareTo(NumericCurrencyCode other) => _Value.CompareTo(other._Value);

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(NumericCurrencyCode value) => value._Value;

    #endregion

    #region Serialization

    public byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value);

    #endregion
}