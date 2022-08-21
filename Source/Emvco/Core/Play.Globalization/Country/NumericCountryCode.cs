using System.Diagnostics.CodeAnalysis;

namespace Play.Globalization.Country;

public readonly struct NumericCountryCode
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public NumericCountryCode(ushort value)
    {
        //if (!CountryCodeRepository.IsValid(value))
        //{
        //    throw new ArgumentOutOfRangeException(nameof(value), $"The argument {nameof(value)} must be ISO 3166 compliant");
        //}

        _Value = value;
    }

    #endregion

    #region Equality

    public bool Equals(NumericCountryCode other) => _Value == other._Value;
    public bool Equals(NumericCountryCode x, NumericCountryCode y) => x.Equals(y);
    public override bool Equals([AllowNull] object obj) => obj is NumericCountryCode countryCode && Equals(countryCode);
    public int GetHashCode(NumericCountryCode obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(786433 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator ==(NumericCountryCode left, NumericCountryCode right) => left.Equals(right);
    public static implicit operator ushort(NumericCountryCode value) => value._Value;
    public static bool operator !=(NumericCountryCode left, NumericCountryCode right) => !left.Equals(right);

    #endregion
}