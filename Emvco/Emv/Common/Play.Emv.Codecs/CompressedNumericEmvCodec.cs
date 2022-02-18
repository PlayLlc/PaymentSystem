﻿using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class CompressedNumericEmvCodec : IPlayCodec
{
    #region Static Metadata

    private static readonly CompressedNumeric _Codec = new();

    #endregion

    #region Instance Members

    public bool IsValid(ReadOnlySpan<byte> value) => _Codec.IsValid(value);
    public byte[] Encode<T>(T[] value) where T : struct => throw new NotImplementedException();
    public byte[] Encode<T>(T[] value, int length) where T : struct => throw new NotImplementedException();

    public void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteSize)
        {
            buffer[offset++] = Unsafe.As<T, byte>(ref value);

            return;
        }

        if (byteSize == Specs.Integer.UInt16.ByteSize)
            Encode(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        if (byteSize <= Specs.Integer.UInt32.ByteSize)
            Encode(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);

        Encode(Unsafe.As<T, BigInteger>(ref value), buffer, ref offset);
    }

    public void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public byte[] Encode<T>(T value) where T : struct
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteSize)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteSize)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    public byte[] Encode<T>(T value, int length) where T : struct
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

    public byte[] Encode(byte value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt8.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(ushort value) => _Codec.GetBytes(value);
    public byte[] Encode(uint value) => _Codec.GetBytes(value);
    public byte[] Encode(uint value, int length) => _Codec.GetBytes(value);
    public byte[] Encode(ulong value) => _Codec.GetBytes(value);
    public byte[] Encode(ulong value, int length) => _Codec.GetBytes(value);
    public byte[] Encode(BigInteger value) => _Codec.GetBytes(value);
    public byte[] Encode(BigInteger value, int length) => _Codec.GetBytes(value);
    public ushort GetByteCount<T>(T value) where T : struct => checked((ushort) Unsafe.SizeOf<T>());

    public ushort GetByteCount<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(byte))
            return (ushort) ((ushort) value.Length * 2);

        if (typeof(T) == typeof(char))
            return (ushort) (((ushort) value.Length / 2) + (value.Length % 2));

        throw new NotImplementedException();
    }

    public BigInteger DecodeBigInteger(ReadOnlySpan<byte> value) => _Codec.GetBigInteger(value);
    public byte DecodeByte(byte value) => _Codec.GetByte(value);
    public ushort DecodeUInt16(ReadOnlySpan<byte> value) => _Codec.GetUInt16(value);
    public uint DecodeUInt32(ReadOnlySpan<byte> value) => _Codec.GetUInt32(value);
    public ulong DecodeUInt64(ReadOnlySpan<byte> value) => _Codec.GetUInt64(value);

    #endregion

    #region Serialization

    public DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        BigInteger maximumIntegerResult = (BigInteger) Math.Pow(2, value.Length * 8);

        if (maximumIntegerResult <= byte.MaxValue)
        {
            byte result = _Codec.GetByte(value[0]);

            return new DecodedResult<byte>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= ushort.MaxValue)
        {
            ushort result = _Codec.GetUInt16(value);

            return new DecodedResult<ushort>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= uint.MaxValue)
        {
            uint result = _Codec.GetUInt32(value);

            return new DecodedResult<uint>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= ulong.MaxValue)
        {
            ulong result = _Codec.GetUInt64(value);

            return new DecodedResult<ulong>(result, result.GetNumberOfDigits());
        }
        else
        {
            BigInteger result = _Codec.GetBigInteger(value);

            return new DecodedResult<BigInteger>(result, result.GetNumberOfDigits());
        }
    }

    #endregion
}