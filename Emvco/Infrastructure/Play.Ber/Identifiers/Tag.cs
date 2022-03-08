﻿using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers.Long;
using Play.Ber.Identifiers.Short;
using Play.Codecs;

namespace Play.Ber.Identifiers;

public readonly record struct Tag
{
    #region Instance Values

    public readonly uint _Value;

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Core.Exceptions.PlayInternalException"></exception>
    public Tag(ReadOnlySpan<byte> value)
    {
        if (ShortIdentifier.IsValid(value[0]))
        {
            _Value = value[0];

            return;
        }

        byte byteCount = LongIdentifier.GetByteCount(value);
        LongIdentifier.Validate(value[..byteCount]);

        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(value[..byteCount]);
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
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
        $"Hex: {PlayCodec.HexadecimalCodec.DecodeToString(Serialize())}; Binary: {PlayCodec.BinaryCodec.DecodeToString(_Value)}";

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

    /// <exception cref="BerParsingException"></exception>
    private static ClassType GetClass(uint value)
    {
        if (IsShortTag(value))
            return ShortIdentifier.GetClassType((byte) value);

        return LongIdentifier.GetClassType(value);
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

    public readonly ClassType GetClassType() => GetClassType(_Value);

    /// <summary>
    ///     GetClassType
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    private static ClassType GetClassType(uint value)
    {
        if (IsShortTag(value))
            return ShortIdentifier.GetClassType((byte) value);

        return LongIdentifier.GetClassType(value);
    }

    /// <summary>
    ///     The Tag Number of this Tag
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.2.2 c
    ///     <see cref="ITUT_X690" /> Section 8.1.2.4.2
    /// </remarks>
    /// <returns>byte</returns>
    /// <exception cref="BerParsingException"></exception>
    public readonly ushort GetTagNumber() => GetTagNumber(_Value);

    /// <exception cref="BerParsingException"></exception>
    private static ushort GetTagNumber(uint value)
    {
        if (IsShortTag(value))
            return ShortIdentifier.GetTagNumber((byte) value);

        return LongIdentifier.GetTagNumber(value);
    }

    public readonly bool IsPrimitive() => GetDataObject().IsPrimitive();
    public readonly bool IsConstructed() => !GetDataObject().IsPrimitive();
    public readonly bool IsUniversal() => ClassType.IsUniversal(GetClassType());
    public readonly bool IsApplicationSpecific() => ClassType.IsUniversal(GetClassType());
    private static bool IsShortTag(uint value) => ShortIdentifier.IsValid(value);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static bool IsValid(uint value) => ShortIdentifier.IsValid(value) || LongIdentifier.IsValid(value);

    public static bool IsValid(ReadOnlySpan<byte> value) => ShortIdentifier.IsValid(value) || LongIdentifier.IsValid(value);
    public int CompareTo(Tag other) => _Value.CompareTo(other._Value);
    public int CompareTo(uint other) => _Value.CompareTo(other);

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public byte[] Serialize() => PlayCodec.UnsignedIntegerCodec.Encode(_Value, true);

    public static byte[] Serialize(IEncodeBerDataObjects value) => PlayCodec.UnsignedIntegerCodec.Encode(value.GetTag(), true);

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