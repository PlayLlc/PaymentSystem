﻿using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Core.Extensions.Types;
using Play.Core.Specifications;

namespace Play.Codecs;

public class BinaryCodec : PlayCodec
{
    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(BinaryCodec));

    private static readonly Dictionary<byte, char[]> _CharArrayMap = new()
    {
        {0b0000, new[] {'0', '0', '0', '0'}},
        {0b0001, new[] {'0', '0', '0', '1'}},
        {0b0010, new[] {'0', '0', '1', '0'}},
        {0b0011, new[] {'0', '0', '1', '1'}},
        {0b0100, new[] {'0', '1', '0', '0'}},
        {0b0101, new[] {'0', '1', '0', '1'}},
        {0b0110, new[] {'0', '1', '1', '0'}},
        {0b0111, new[] {'0', '1', '1', '1'}},
        {0b1000, new[] {'1', '0', '0', '0'}},
        {0b1001, new[] {'1', '0', '0', '1'}},
        {0b1010, new[] {'1', '0', '1', '0'}},
        {0b1011, new[] {'1', '0', '1', '1'}},
        {0b1100, new[] {'1', '1', '0', '0'}},
        {0b1101, new[] {'1', '1', '0', '1'}},
        {0b1110, new[] {'1', '1', '1', '0'}},
        {0b1111, new[] {'1', '1', '1', '1'}}
    };

    private static readonly Dictionary<Memory<char>, byte> _NibbleMap = new()
    {
        {new[] {'0', '0', '0', '0'}, 0b0000},
        {new[] {'0', '0', '0', '1'}, 0b0001},
        {new[] {'0', '0', '1', '0'}, 0b0010},
        {new[] {'0', '0', '1', '1'}, 0b0011},
        {new[] {'0', '1', '0', '0'}, 0b0100},
        {new[] {'0', '1', '0', '1'}, 0b0101},
        {new[] {'0', '1', '1', '0'}, 0b0110},
        {new[] {'0', '1', '1', '1'}, 0b0111},
        {new[] {'1', '0', '0', '0'}, 0b1000},
        {new[] {'1', '0', '0', '1'}, 0b1001},
        {new[] {'1', '0', '1', '0'}, 0b1010},
        {new[] {'1', '0', '1', '1'}, 0b1011},
        {new[] {'1', '1', '0', '0'}, 0b1100},
        {new[] {'1', '1', '0', '1'}, 0b1101},
        {new[] {'1', '1', '1', '0'}, 0b1110},
        {new[] {'1', '1', '1', '1'}, 0b1111}
    };

    #endregion

    #region Count

    public int GetByteCount(char[] chars, int index, int count) => (count / 8) + (count % 8);
    public int GetMaxByteCount(int charCount) => (charCount / 8) + (charCount % 8);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override ushort GetByteCount<T>(T value) where T : struct
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        Type type = typeof(T);
        nint byteSize = Unsafe.SizeOf<T>();

        if (!type.IsNumericType())
            throw new CodecParsingException($"The {nameof(BinaryCodec)} could not find the byte count for a type of {typeof(T)}");

        if (byteSize <= Specs.Integer.UInt8.ByteCount)
            return 1;
        if (byteSize <= Specs.Integer.UInt16.ByteCount)
            return Unsafe.As<T, ushort>(ref value).GetMostSignificantByte();
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return Unsafe.As<T, uint>(ref value).GetMostSignificantByte();
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Unsafe.As<T, ulong>(ref value).GetMostSignificantByte();
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Unsafe.As<T, ulong>(ref value).GetMostSignificantByte();

        return checked((ushort) Unsafe.As<T, BigInteger>(ref value).GetByteCount(true));
    }

    public override ushort GetByteCount<T>(T[] value) where T : struct => checked((ushort) value.Length);
    public int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();
    public int GetMaxCharCount(int byteCount) => throw new NotImplementedException();

    #endregion

    #region Validation

    public bool IsValid(ReadOnlySpan<char> value)
    {
        if ((value.Length % 8) != 0)
            return false;

        for (int i = 0; i < value.Length; i++)
        {
            if ((value[i] != '0') && (value[i] != '1'))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value) => true;

    #endregion

    #region Encode

    public bool TryEncoding(ReadOnlySpan<char> value, out byte[] result) => throw new NotImplementedException();

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T value) where T : struct
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(T);

        if (type.IsChar())
            return Encode(Unsafe.As<T, char>(ref value));

        if (!type.IsNumericType())
            throw new CodecParsingException(this, typeof(T));

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

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T value, int length) where T : struct
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(T);

        if (type.IsChar())
            return Encode(Unsafe.As<T, char>(ref value));

        if (!type.IsNumericType())
            throw new CodecParsingException(this, typeof(T));

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

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T[] value) where T : struct
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(T);

        if (type.IsChar())
            return Encode(Unsafe.As<T[], char[]>(ref value));
        if (type.IsByte())
            return Unsafe.As<T[], byte[]>(ref value);

        throw new CodecParsingException(this, typeof(T));
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T[] value, int length) where T : struct
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(T);

        if (type.IsChar())
            return Encode(Unsafe.As<T[], byte[]>(ref value), length);
        if (type.IsNumericType())
            return Unsafe.As<T[], byte[]>(ref value);

        throw new CodecParsingException(this, typeof(T));
    }

    public byte[] Encode(byte value)
    {
        return new[] {value};
    }

    public byte[] Encode(ushort value) => UnsignedIntegerCodec.Encode(value, true);
    public byte[] Encode(uint value) => UnsignedIntegerCodec.Encode(value, true);
    public byte[] Encode(uint value, int length) => UnsignedIntegerCodec.Encode(value)[(Specs.Integer.UInt32.ByteCount - length)..];
    public byte[] Encode(ulong value) => UnsignedIntegerCodec.Encode(value, true);
    public byte[] Encode(ulong value, int length) => UnsignedIntegerCodec.Encode(value)[(Specs.Integer.UInt64.ByteCount - length)..];
    public byte[] Encode(BigInteger value) => UnsignedIntegerCodec.Encode(value);

    public byte[] Encode(BigInteger value, int length)
    {
        if (value.GetMostSignificantByte() == length)
            return UnsignedIntegerCodec.Encode(value);

        // TODO: .....Huh?
        return value.GetMostSignificantByte() > length
            ? UnsignedIntegerCodec.Encode(value)[((value.GetMostSignificantBit() - length) + 1)..]
            : UnsignedIntegerCodec.Encode(value)[((length - value.GetMostSignificantBit()) + 1)..];
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value)
    {
        Validate(value);

        char[] charBuffer = new char[8];
        int byteCount = value.Length / 8;

        if (byteCount > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteCount);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 8)
            {
                value[j..(j + 8)].CopyTo(charBuffer);
                Encode(charBuffer, i, buffer);
            }

            return buffer.ToArray();
        }
        else
        {
            Span<byte> buffer = stackalloc byte[byteCount];

            for (int i = 0, j = 0; i < value.Length; i++, j += 8)
            {
                value[j..(j + 8)].CopyTo(charBuffer);
                Encode(charBuffer, i, buffer);
            }

            return buffer.ToArray();
        }
    }

    public void Encode(ushort value, Span<byte> buffer, ref int offset) => UnsignedIntegerCodec.Encode(value, buffer, ref offset);
    public void Encode(uint value, Span<byte> buffer, ref int offset) => UnsignedIntegerCodec.Encode(value, buffer, ref offset);
    public void Encode(uint value, int length, Span<byte> buffer, ref int offset) => UnsignedIntegerCodec.Encode(value, buffer, ref offset);
    public void Encode(ulong value, Span<byte> buffer, ref int offset) => UnsignedIntegerCodec.Encode(value, buffer, ref offset);
    public void Encode(ulong value, int length, Span<byte> buffer, ref int offset) => UnsignedIntegerCodec.Encode(value, buffer, ref offset);

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(T);

        if (type.IsChar())
        {
            buffer[offset++] = 1;

            return;
        }

        if (!type.IsNumericType())
            throw new CodecParsingException(this, typeof(T));

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

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(T);

        if (type.IsChar())
        {
            buffer[offset++] = 1;

            return;
        }

        if (!type.IsNumericType())
            throw new CodecParsingException(this, typeof(T));

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

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow 
        Type type = typeof(T);

        if (type.IsChar())
            Encode(Unsafe.As<T[], char[]>(ref value), buffer, ref offset);
        else if (typeof(T).IsNumericType())
            throw new CodecParsingException($"The {nameof(BinaryCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        else
            Encode(Unsafe.As<T[], byte[]>(ref value), buffer, ref offset);
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow 
        Type type = typeof(T);

        if (type.IsChar())
            Encode(Unsafe.As<T[], char[]>(ref value)[..length], buffer, ref offset);
        else if (type.IsNumericType())
            Encode(Unsafe.As<T[], byte[]>(ref value), length, buffer, ref offset);
        else
            throw new CodecParsingException($"The {nameof(BinaryCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public void Encode(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset)
    {
        Validate(value);

        char[] charBuffer = new char[8];

        for (int i = 0, j = 0; i < value.Length; i++, j += 8)
        {
            value[j..(j + 8)].CopyTo(charBuffer);
            Encode(charBuffer, i + offset++, buffer);
        }
    }

    /// <exception cref="CodecParsingException"></exception>
    private void Encode(char[] charBuffer, int offset, Span<byte> buffer)
    {
        if (charBuffer.Length != 8)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(charBuffer), "The sequence must have a length of 8"));

        buffer[offset] = (byte) (_NibbleMap[charBuffer[..4]] >> 4);
        buffer[offset] = _NibbleMap[charBuffer[4..]];
        buffer.Clear();
    }

    #endregion

    #region Decode To Chars

    public void DecodeToChars(byte value, int offset, Span<char> buffer)
    {
        _CharArrayMap[value.GetMaskedValue(0xF0)].CopyTo(buffer[..(offset + 4)]);
        _CharArrayMap[(byte) (value >> 4)].CopyTo(buffer[(offset + 4)..]);
    }

    #endregion

    #region Decode To String

    public string DecodeToString(byte value) => Convert.ToString(value, 2);
    public string DecodeToString(short value) => Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string DecodeToString(ushort value) => Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string DecodeToString(int value) => Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string DecodeToString(uint value) => Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string DecodeToString(long value) => Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string DecodeToString(ulong value) => throw new NotImplementedException();

    public string DecodeToString(ReadOnlySpan<byte> value)
    {
        int charCount = value.Length * 8;

        if (charCount > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(charCount);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 8)
                DecodeToChars(value[i], j, buffer);

            return new string(buffer);
        }
        else
        {
            Span<char> buffer = stackalloc char[charCount];
            for (int i = 0, j = 0; i < value.Length; i++, j += 8)
                DecodeToChars(value[i], j, buffer);

            return new string(buffer);
        }
    }

    #endregion

    #region Decode To Integers

    public BigInteger DecodeToBigInteger(ReadOnlySpan<byte> value) => UnsignedIntegerCodec.DecodeToBigInteger(value);
    public byte DecodeToByte(ReadOnlySpan<byte> value) => UnsignedIntegerCodec.DecodeToByte(value);
    public uint DecodeToUInt32(ReadOnlySpan<byte> value) => UnsignedIntegerCodec.DecodeToUInt32(value);
    public ulong DecodeToUInt64(ReadOnlySpan<byte> value) => UnsignedIntegerCodec.DecodeToUInt64(value);
    public ushort DecodeToUInt16(ReadOnlySpan<byte> value) => UnsignedIntegerCodec.DecodeToUInt16(value);

    #endregion

    #region Decode To DecodedMetadata

    // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length <= Specs.Integer.UInt8.ByteCount)
            return new DecodedResult<byte>(value[0], value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt16.ByteCount)
            return new DecodedResult<ushort>(UnsignedIntegerCodec.DecodeToUInt16(value), value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt32.ByteCount)
            return new DecodedResult<uint>(UnsignedIntegerCodec.DecodeToUInt32(value), value[0].GetNumberOfDigits());
        if (value.Length <= Specs.Integer.UInt64.ByteCount)
            return new DecodedResult<ulong>(UnsignedIntegerCodec.DecodeToUInt64(value), value[0].GetNumberOfDigits());

        return new DecodedResult<BigInteger>(UnsignedIntegerCodec.DecodeToBigInteger(value), value[0].GetNumberOfDigits());
    }

    #endregion

    #region Instance Members

    private void Validate(ReadOnlySpan<char> value)
    {
        if ((value.Length % 8) != 0)
            throw new ArgumentOutOfRangeException(nameof(value), $"The {nameof(BinaryCodec)} Encoding expects a string that is divisible by 8");

        for (int i = 0; i < value.Length; i++)
        {
            if ((value[i] != '0') && (value[i] != '1'))
                throw new ArgumentOutOfRangeException($"The {nameof(BinaryCodec)} Encoding expects all string values to be either a '1' or a '0'");
        }
    }

    protected void Validate(ReadOnlySpan<byte> value)
    { }

    #endregion
}