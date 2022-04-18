using Play.Globalization.Language;

namespace Play.Globalization.Currency;

public readonly record struct NumericCurrencyCode
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public NumericCurrencyCode(ushort value)
    {
        // HACK: This validation causes circular references. Let's try and create an EnumObject or something similar that allows some validation logic

        //if (!CurrencyCodeRepository.IsValid(value))
        //{
        //    throw new ArgumentOutOfRangeException(nameof(value), $"The argument {nameof(value)} must be 3 digits or less according to ISO 4217");
        //}

        _Value = value;
    }

    #endregion

    #region Equality

    public int CompareTo(NumericCurrencyCode other) => _Value.CompareTo(other._Value);

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(NumericCurrencyCode value) => value._Value;

    #endregion
}