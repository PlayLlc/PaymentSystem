using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Codecs;
using Play.Codecs.Integers;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class BinaryEmvCodec : Codec
{
    #region Static Metadata

    private static readonly UnsignedInteger _UnsignedIntegerCodec = PlayEncoding.UnsignedInteger;

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<byte> value) => true;

    public override byte[] Encode<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize <= Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (byteSize <= Specs.Integer.UInt16.ByteSize)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteSize)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    public override byte[] Encode<T>(T value, int length)
    {
        if (length == Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteSize)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<T, uint>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteSize)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value), length);
    }

    public override byte[] Encode<T>(T[] value) where T : struct
    {
        if (typeof(T) != typeof(byte[]))
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(BinaryEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }

        return Encode(Unsafe.As<T[], byte[]>(ref value));
    }

    public byte[] Encode(byte[] value) => value;
    public override byte[] Encode<T>(T[] value, int length) where T : struct => Encode(Unsafe.As<T[], byte[]>(ref value), length);

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize <= Specs.Integer.UInt8.ByteSize)
            Encode(Unsafe.As<T, byte>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt16.ByteSize)
            Encode(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt32.ByteSize)
            Encode(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<T, BigInteger>(ref value), buffer, ref offset);
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        if (length == Specs.Integer.UInt8.ByteSize)
            Encode(Unsafe.As<T, byte>(ref value), buffer, ref offset);
        else if (length == Specs.Integer.UInt16.ByteSize)
            Encode(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        else if (length == 3)
            Encode(Unsafe.As<T, uint>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt32.ByteSize)
            Encode(Unsafe.As<T, uint>(ref value));
        else if (length < Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<T, BigInteger>(ref value), length, buffer, ref offset);
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) != typeof(byte[]))
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(BinaryEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }

        Encode(Unsafe.As<T[], byte[]>(ref value), buffer, ref offset);
    }

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) != typeof(byte[]))
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(BinaryEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }

        Encode(Unsafe.As<T[], byte[]>(ref value), length, buffer, ref offset);
    }

    public byte[] Encode(byte value)
    {
        return new[] {value};
    }

    public byte[] Encode(ushort value) => _UnsignedIntegerCodec.GetBytes(value, true);
    public byte[] Encode(uint value) => _UnsignedIntegerCodec.GetBytes(value, true);
    public byte[] Encode(uint value, int length) => _UnsignedIntegerCodec.GetBytes(value)[(Specs.Integer.UInt32.ByteSize - length)..];
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

        if (byteSize <= Specs.Integer.UInt8.ByteSize)
            return 1;
        if (byteSize <= Specs.Integer.UInt16.ByteSize)
            return Unsafe.As<T, ushort>(ref value).GetMostSignificantByte();
        if (byteSize <= Specs.Integer.UInt32.ByteSize)
            return Unsafe.As<T, uint>(ref value).GetMostSignificantByte();
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Unsafe.As<T, ulong>(ref value).GetMostSignificantByte();

        throw new InternalEmvEncodingException($"The {nameof(BinaryEmvCodec)} could not find the byte count for a type of {typeof(T)}");
    }

    public override ushort GetByteCount<T>(T[] value) => checked((ushort) value.Length);

    protected override void Validate(ReadOnlySpan<byte> value)
    { }

    #endregion
}