﻿using System.Collections.Immutable;

using Play.Codecs;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.Enums;

public sealed record CryptogramTypes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly CryptogramTypes Empty = new();
    private static readonly ImmutableSortedDictionary<byte, CryptogramTypes> _ValueObjectMap;

    /// <value>Binary: 0000 0000; Hexadecimal: 0x00</value>
    public static readonly CryptogramTypes ApplicationAuthenticationCryptogram;

    /// <value>Binary: 1000 0000; Hexadecimal: 0x80</value>
    public static readonly CryptogramTypes AuthorizationRequestCryptogram;

    /// <value>Binary: 0100 0000; Hexadecimal: 0x40</value>
    public static readonly CryptogramTypes TransactionCryptogram;

    private const byte _UnrelatedBits = 0b00111111;

    #endregion

    #region Constructor

    public CryptogramTypes()
    { }

    static CryptogramTypes()
    {
        const byte applicationAuthenticationCryptogram = 0;
        const byte transactionCryptogram = 0x40;
        const byte authorizationRequestCryptogram = 0x80;

        ApplicationAuthenticationCryptogram = new CryptogramTypes(applicationAuthenticationCryptogram);
        TransactionCryptogram = new CryptogramTypes(transactionCryptogram);
        AuthorizationRequestCryptogram = new CryptogramTypes(authorizationRequestCryptogram);
        _ValueObjectMap = new Dictionary<byte, CryptogramTypes>
        {
            {applicationAuthenticationCryptogram, ApplicationAuthenticationCryptogram},
            {transactionCryptogram, TransactionCryptogram},
            {authorizationRequestCryptogram, AuthorizationRequestCryptogram}
        }.ToImmutableSortedDictionary();
    }

    private CryptogramTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override CryptogramTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out CryptogramTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value.GetMaskedValue(_UnrelatedBits));
    public static bool TryGet(byte value, out CryptogramTypes? result) => _ValueObjectMap.TryGetValue(value.GetMaskedValue(_UnrelatedBits), out result);
    public override string ToString() => $"0x{PlayCodec.HexadecimalCodec.DecodeToString(new[] {_Value})}";

    #endregion

    #region Equality

    public bool Equals(CryptogramTypes? other) => other is not null && (_Value == other._Value);
    public bool Equals(CryptogramTypes x, CryptogramTypes y) => x.Equals(y);
    public override int GetHashCode() => 12251 * _Value.GetHashCode();
    public int GetHashCode(CryptogramTypes obj) => obj.GetHashCode();
    public int CompareTo(CryptogramTypes other) => _Value.CompareTo(other._Value);

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CryptogramTypes cryptogramTypes) => cryptogramTypes._Value;
    public static implicit operator CryptogramType(CryptogramTypes cryptogramTypes) => new(cryptogramTypes._Value);

    #endregion
}