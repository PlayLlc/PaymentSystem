using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Codecs;
using Play.Codecs.Integers;
using Play.Codecs.Metadata;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class BinaryEmvCodec : IPlayCodec
{
    #region Static Metadata

    #region Metadata

    private static readonly UnsignedInteger _UnsignedIntegerCodec = PlayEncoding.UnsignedInteger;

    #endregion

    #endregion

    #region Serialization

    #region Decode To DecodedMetadata

    public DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length <= Specs.Integer.UInt8.ByteCount)
            return new DecodedResult<byte>(value[0], value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt16.ByteCount)
            return new DecodedResult<ushort>(PlayEncoding.UnsignedInteger.GetUInt16(value), value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt32.ByteCount)
            return new DecodedResult<uint>(PlayEncoding.UnsignedInteger.GetUInt32(value), value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt64.ByteCount)
            return new DecodedResult<ulong>(PlayEncoding.UnsignedInteger.GetUInt64(value), value[0].GetNumberOfDigits());

        return new DecodedResult<BigInteger>(PlayEncoding.UnsignedInteger.GetBigInteger(value), value[0].GetNumberOfDigits());
    }

    #endregion

    #endregion

    #region Count

    public PlayEncodingId GetEncodingId() => throw new NotImplementedException();

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ushort GetByteCount<T>(T value) where T : struct
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

        throw new InternalEmvEncodingException($"The {nameof(BinaryEmvCodec)} could not find the byte count for a type of {typeof(T)}");
    }

    public ushort GetByteCount<T>(T[] value) where T : struct => checked((ushort) value.Length);

    #endregion

    #region Validation

    public bool IsValid(ReadOnlySpan<byte> value) => true;

    protected void Validate(ReadOnlySpan<byte> value)
    { }

    #endregion

    #region Encode

    public byte[] Encode<T>(T value) where T : struct
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

    public byte[] Encode<T>(T value, int length) where T : struct
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

    public byte[] Encode<T>(T[] value) where T : struct
    {
        if (typeof(T) != typeof(byte[]))
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(BinaryEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }

        return Encode(Unsafe.As<T[], byte[]>(ref value));
    }

    public byte[] Encode(byte[] value) => value;
    public byte[] Encode<T>(T[] value, int length) where T : struct => Encode(Unsafe.As<T[], byte[]>(ref value), length);

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

    public void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize <= Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<T, byte>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<T, BigInteger>(ref value), buffer, ref offset);
    }

    public void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        if (length == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<T, byte>(ref value), buffer, ref offset);
        else if (length == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        else if (length == 3)
            Encode(Unsafe.As<T, uint>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<T, uint>(ref value));
        else if (length < Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<T, BigInteger>(ref value), length, buffer, ref offset);
    }

    public void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) != typeof(byte[]))
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(BinaryEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }

        Encode(Unsafe.As<T[], byte[]>(ref value), buffer, ref offset);
    }

    public void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) != typeof(byte[]))
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(BinaryEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }

        Encode(Unsafe.As<T[], byte[]>(ref value), length, buffer, ref offset);
    }

    #endregion
}