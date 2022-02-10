﻿using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public sealed record CryptogramTypes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CryptogramTypes> _ValueObjectMap;

    /// <value>Decimal: 0; Hexadecimal: 0x00</value>
    public static readonly CryptogramTypes ApplicationAuthenticationCryptogram;

    /// <value>Decimal: 128; Hexadecimal: 0x80</value>
    public static readonly CryptogramTypes AuthorizationRequestCryptogram;

    /// <value>Decimal: 64; Hexadecimal: 0x40</value>
    public static readonly CryptogramTypes TransactionCryptogram;

    #endregion

    #region Constructor

    static CryptogramTypes()
    {
        const byte applicationAuthenticationCryptogram = 0;
        const byte transactionCertificate = 0b01000000;
        const byte authorizationRequestCryptogram = 0b10000000;

        ApplicationAuthenticationCryptogram = new CryptogramTypes(applicationAuthenticationCryptogram);
        TransactionCryptogram = new CryptogramTypes(transactionCertificate);
        AuthorizationRequestCryptogram = new CryptogramTypes(authorizationRequestCryptogram);
        _ValueObjectMap = new Dictionary<byte, CryptogramTypes>
        {
            {applicationAuthenticationCryptogram, ApplicationAuthenticationCryptogram},
            {transactionCertificate, TransactionCryptogram},
            {authorizationRequestCryptogram, AuthorizationRequestCryptogram}
        }.ToImmutableSortedDictionary();
    }

    private CryptogramTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(CryptogramTypes other) => _Value.CompareTo(other._Value);

    public static bool TryGet(byte value, out CryptogramTypes? result)
    {
        const byte bitMask = 0b00111111;

        return _ValueObjectMap.TryGetValue(value.GetMaskedValue(bitMask), out result);
    }

    #endregion

    #region Equality

    public bool Equals(CryptogramTypes? other) => other is not null && (_Value == other._Value);
    public bool Equals(CryptogramTypes x, CryptogramTypes y) => x.Equals(y);
    public override int GetHashCode() => 12251 * _Value.GetHashCode();
    public int GetHashCode(CryptogramTypes obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CryptogramTypes cryptogramTypes) => cryptogramTypes._Value;

    #endregion
}