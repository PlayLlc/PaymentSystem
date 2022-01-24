using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.Icc;

public sealed record CryptogramType : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CryptogramType> _ValueObjectMap;

    /// <value>Decimal: 0; Hexadecimal: 0x00</value>
    public static readonly CryptogramType ApplicationAuthenticationCryptogram;

    /// <value>Decimal: 128; Hexadecimal: 0x80</value>
    public static readonly CryptogramType AuthorizationRequestCryptogram;

    /// <value>Decimal: 64; Hexadecimal: 0x40</value>
    public static readonly CryptogramType TransactionCryptogram;

    #endregion

    #region Constructor

    static CryptogramType()
    {
        const byte applicationAuthenticationCryptogram = 0;
        const byte transactionCertificate = 0b01000000;
        const byte authorizationRequestCryptogram = 0b10000000;

        ApplicationAuthenticationCryptogram = new CryptogramType(applicationAuthenticationCryptogram);
        TransactionCryptogram = new CryptogramType(transactionCertificate);
        AuthorizationRequestCryptogram = new CryptogramType(authorizationRequestCryptogram);
        _ValueObjectMap = new Dictionary<byte, CryptogramType>
        {
            {applicationAuthenticationCryptogram, ApplicationAuthenticationCryptogram},
            {transactionCertificate, TransactionCryptogram},
            {authorizationRequestCryptogram, AuthorizationRequestCryptogram}
        }.ToImmutableSortedDictionary();
    }

    private CryptogramType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(CryptogramType other) => _Value.CompareTo(other._Value);

    public static bool TryGet(byte value, out CryptogramType? result)
    {
        const byte bitMask = 0b00111111;

        return _ValueObjectMap.TryGetValue(value.GetMaskedValue(bitMask), out result);
    }

    #endregion

    #region Equality

    public bool Equals(CryptogramType? other) => other is not null && (_Value == other._Value);
    public bool Equals(CryptogramType x, CryptogramType y) => x.Equals(y);
    public override int GetHashCode() => 12251 * _Value.GetHashCode();
    public int GetHashCode(CryptogramType obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CryptogramType cryptogramType) => cryptogramType._Value;

    #endregion
}