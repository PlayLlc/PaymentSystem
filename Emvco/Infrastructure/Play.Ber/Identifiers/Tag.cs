﻿using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers.Long;
using Play.Ber.Identifiers.Short;

namespace Play.Ber.Identifiers;

public readonly record struct Tag
{
    #region Instance Values

    public readonly uint _Value;

    #endregion

    #region Constructor

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Tag(ReadOnlySpan<byte> value)
    {
        if (ShortIdentifier.IsValid(value[0]))
        {
            _Value = value[0];

            return;
        }

        byte byteCount = LongIdentifier.GetByteCount(value);
        LongIdentifier.Validate(value[..byteCount]);

        _Value = PlayEncoding.UnsignedInteger.GetUInt32(value[..byteCount]);
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    public Tag(uint value)
    {
        if (ShortIdentifier.IsValid(value))
        {
            _Value = value;

            return;
        }

        LongIdentifier.Validate(value);
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override string ToString() =>
        $"Hex: {PlayEncoding.Hexadecimal.GetString(Serialize())}; Binary: {PlayEncoding.Binary.GetString(_Value)}";

    public readonly byte GetByteCount() => GetByteCount(_Value);

    private static byte GetByteCount(uint value)
    {
        if (IsShortTag(value))
            return ShortIdentifier.ByteCount;

        return LongIdentifier.GetByteCount(value);
    }

    public static byte GetByteCount(IEncodeBerDataObjects value)
    {
        if (IsShortTag(value.GetTag()))
            return ShortIdentifier.ByteCount;

        return LongIdentifier.GetByteCount(value.GetTag());
    }

    /// <summary>
    ///     The identifier octets shall encode the ASN.1 tag (class and number) of the type of the data value.
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> 8.1.2.1
    /// </remarks>
    public readonly ClassType GetClass() => GetClass(_Value);

    /// <exception cref="BerException"></exception>
    private static ClassType GetClass(uint value)
    {
        if (IsShortTag(value))
            return ShortIdentifier.GetClassType((byte) value);

        return LongIdentifier.GetClass(value);
    }

    /// <summary>
    ///     Bit 6 shall be set to zero if the encoding is primitive, and shall be set to one if the encoding is constructed.
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.2.5
    /// </remarks>
    public readonly DataObjectType GetDataObject() => GetDataObject(_Value);

    private static DataObjectType GetDataObject(uint value)
    {
        if (IsShortTag(value))
            return ShortIdentifier.GetDataObject((byte) value);

        return LongIdentifier.GetDataObject(value);
    }

    /// <summary>
    ///     The Tag Number of this Tag
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.2.2 c
    ///     <see cref="ITUT_X690" /> Section 8.1.2.4.2
    /// </remarks>
    /// <returns>byte</returns>
    /// <exception cref="BerException"></exception>
    public readonly ushort GetTagNumber() => GetTagNumber(_Value);

    /// <exception cref="BerException"></exception>
    private static ushort GetTagNumber(uint value)
    {
        if (IsShortTag(value))
            return ShortIdentifier.GetTagNumber((byte) value);

        return LongIdentifier.GetTagNumber(value);
    }

    public readonly bool IsPrimitive() => GetDataObject().IsPrimitive();
    private static bool IsShortTag(uint value) => ShortIdentifier.IsValid(value);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static bool IsValid(uint value) => ShortIdentifier.IsValid(value) || LongIdentifier.IsValid(value);

    public static bool IsValid(ReadOnlySpan<byte> value) => ShortIdentifier.IsValid(value) || LongIdentifier.IsValid(value);
    public int CompareTo(Tag other) => _Value.CompareTo(other._Value);
    public int CompareTo(uint other) => _Value.CompareTo(other);

    #endregion

    #region Serialization

    /// <exception cref="BerException"></exception>
    public byte[] Serialize() => PlayEncoding.UnsignedInteger.GetBytes(_Value, true);

    public static byte[] Serialize(IEncodeBerDataObjects value) => PlayEncoding.UnsignedInteger.GetBytes(value.GetTag(), true);

    #endregion

    #region Equality

    public bool Equals(Tag other) => other._Value == _Value;
    public override int GetHashCode() => GetHashCode(this);

    public int GetHashCode(Tag obj)
    {
        const int largePrime = 486187739;

        unchecked // overflow is fine, just wraps
        {
            return largePrime + _Value.GetHashCode();
        }
    }

    #endregion

    #region Operator Overrides

    public static implicit operator uint(Tag tag) => tag._Value;
    public static implicit operator Tag(uint tag) => new(tag);

    #endregion
}