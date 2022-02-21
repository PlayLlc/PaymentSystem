﻿using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Integers;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class UnsignedBinaryCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly UnsignedInteger _UnsignedIntegerCodec = PlayEncoding.UnsignedInteger;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(UnsignedBinaryCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => true;

    public override byte[] Encode<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize <= Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (byteSize <= Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    public override byte[] Encode<T>(T value, int length)
    {
        if (length == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<T, uint>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value), length);
    }

    public override byte[] Encode<T>(T[] value) where T : struct => Encode(Unsafe.As<T[], byte[]>(ref value));
    public byte[] Encode(byte[] value) => value;
    public override byte[] Encode<T>(T[] value, int length) where T : struct => Encode(Unsafe.As<T[], byte[]>(ref value), length);

    public byte[] Encode(byte value)
    {
        return new[] {value};
    }

    public byte[] Encode(ushort value) => _UnsignedIntegerCodec.GetBytes(value, true);
    public byte[] Encode(uint value) => _UnsignedIntegerCodec.GetBytes(value, true);
    public byte[] Encode(uint value, int length) => _UnsignedIntegerCodec.GetBytes(value)[(Specs.Integer.UInt32.ByteCount - length)..];
    public byte[] Encode(ulong value) => _UnsignedIntegerCodec.GetBytes(value, true);
    public byte[] Encode(ulong value, int length) => _UnsignedIntegerCodec.GetBytes(value)[(Specs.Integer.UInt64.ByteCount - length)..];
    public byte[] Encode(BigInteger value) => _UnsignedIntegerCodec.GetBytes(value);

    public byte[] Encode(BigInteger value, int length)
    {
        if (value.GetMostSignificantByte() == length)
            return _UnsignedIntegerCodec.GetBytes(value);

        // TODO: .....Huh?
        return value.GetMostSignificantByte() > length
            ? _UnsignedIntegerCodec.GetBytes(value)[((value.GetMostSignificantBit() - length) + 1)..]
            : _UnsignedIntegerCodec.GetBytes(value)[((length - value.GetMostSignificantBit()) + 1)..];
    }

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override ushort GetByteCount<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize <= Specs.Integer.UInt8.ByteCount)
            return 1;
        if (byteSize <= Specs.Integer.UInt16.ByteCount)
            return Unsafe.As<T, ushort>(ref value).GetMostSignificantByte();
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return Unsafe.As<T, uint>(ref value).GetMostSignificantByte();
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Unsafe.As<T, ulong>(ref value).GetMostSignificantByte();

        throw new InternalEmvEncodingException(
            $"The {nameof(UnsignedBinaryCodec)} could not find the byte count for a type of {typeof(T)}");
    }

    public override ushort GetByteCount<T>(T[] value) => checked((ushort) value.Length);

    protected override void Validate(ReadOnlySpan<byte> value)
    { }

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length <= Specs.Integer.UInt8.ByteCount)
            return new DecodedResult<byte>(value[0], value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt16.ByteCount)
            return new DecodedResult<ushort>(_UnsignedIntegerCodec.GetUInt16(value), value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt32.ByteCount)
            return new DecodedResult<uint>(_UnsignedIntegerCodec.GetUInt32(value), value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt64.ByteCount)
            return new DecodedResult<ulong>(_UnsignedIntegerCodec.GetUInt64(value), value[0].GetNumberOfDigits());

        return new DecodedResult<BigInteger>(_UnsignedIntegerCodec.GetBigInteger(value), value[0].GetNumberOfDigits());
    }

    #endregion
}