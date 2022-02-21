using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Codecs.Metadata;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class SignedNumericCodec : PlayCodec
{
    #region Instance Members

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ushort DecodeToInt16(ReadOnlySpan<byte> value)
    {
        ushort result = 0;

        return (ushort) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint DecodeToInt32(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new ArgumentOutOfRangeException(nameof(value));

        uint result = 0;

        return (uint) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ulong DecodeToInt64(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        ulong result = 0;

        return (ulong) BuildInteger(result, value);
    }

    #endregion

    #region Serialization

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value) => throw new NotImplementedException();

    #endregion

    #endregion

    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(nameof(SignedNumericCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap = Enumerable.Range(0, 10)
        .Concat(new[] {67, 68}).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(0, 10).Concat(new[] {67, 68}).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    private const char _Positive = 'C';
    private const char _Negative = 'D';

    #endregion

    #region Count

    public int GetByteCount(char[] chars, int index, int count) => count;
    public int GetMaxByteCount(int charCount) => charCount;
    public override ushort GetByteCount<_T>(_T value) => (ushort) Unsafe.SizeOf<_T>();
    public override ushort GetByteCount<_T>(_T[] value) => (ushort) value.Length;
    public int GetCharCount(byte[] bytes, int index, int count) => count;
    public int GetMaxCharCount(int byteCount) => byteCount;

    #endregion

    #region Validation

    public bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public bool IsValid(byte value) => IsNibbleValid((byte) (value >> 4)) && IsNibbleValid(value.GetMaskedValue(0xF0));

    public bool IsValid(char value)
    {
        if (_ByteMap.Keys.Contains(value))
            return true;

        return false;
    }

    private static bool IsNibbleValid(byte value)
    {
        if (_CharMap.Keys.Contains(value))
            return true;

        return false;
    }

    #endregion

    #region Encode

    public bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = Encode(value, value.Length);

        return true;
    }

    public byte[] Encode(string value)
    {
        if (value.Length == 0)
            return Array.Empty<byte>();

        if ((value[0] != _Positive) && (value[0] != _Negative))
            throw new ArgumentOutOfRangeException(nameof(value));

        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[value.Length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
    }

    public override byte[] Encode<_T>(_T value)
    {
        Type type = typeof(_T);

        if (!type.IsByte())
            throw new InternalPlayEncodingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.Int8.ByteCount)
            return Encode(Unsafe.As<_T, sbyte>(ref value));
        if (byteSize == Specs.Integer.Int16.ByteCount)
            return Encode(Unsafe.As<_T, short>(ref value));
        if (byteSize <= Specs.Integer.Int32.ByteCount)
            return Encode(Unsafe.As<_T, int>(ref value));
        if (byteSize <= Specs.Integer.Int64.ByteCount)
            return Encode(Unsafe.As<_T, long>(ref value));

        return Encode(Unsafe.As<_T, BigInteger>(ref value));
    }

    public override byte[] Encode<_T>(_T value, int length)
    {
        Type type = typeof(_T);

        if (!type.IsSignedInteger())
            throw new InternalPlayEncodingException(this, type);

        if (length == Specs.Integer.Int8.ByteCount)
            return Encode(Unsafe.As<_T, sbyte>(ref value));
        if (length == Specs.Integer.Int16.ByteCount)
            return Encode(Unsafe.As<_T, short>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<_T, int>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<_T, int>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, long>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, long>(ref value));

        return Encode(Unsafe.As<_T, BigInteger>(ref value), length);
    }

    public override byte[] Encode<_T>(_T[] value)
    {
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T[], char[]>(ref value));
        if (type.IsByte())
            return Encode(Unsafe.As<_T[], byte[]>(ref value));

        throw new InternalPlayEncodingException(this, typeof(_T));
    }

    public override byte[] Encode<_T>(_T[] value, int length)
    {
        if (typeof(_T) == typeof(char))
            return Encode(Unsafe.As<_T[], char[]>(ref value), length);
        if (typeof(_T) == typeof(byte))
            return Encode(Unsafe.As<_T[], byte[]>(ref value));

        throw new InternalPlayEncodingException(this, typeof(_T));
    }

    public byte[] Encode(ReadOnlySpan<char> value) => Encode(value, value.Length);

    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == 0)
            return Array.Empty<byte>();

        if ((value[0] != _Positive) && (value[0] != _Negative))
            throw new ArgumentOutOfRangeException(nameof(value));

        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[value.Length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
    }

    public byte[] Encode(byte value)
    {
        return new[] {value};
    }

    public byte[] Encode(short value)
    {
        const byte byteSize = Specs.Integer.Int16.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(short value, Span<byte> buffer, ref int offset)
    {
        const byte byteSize = Specs.Integer.Int16.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        buffer[offset++] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = offset + (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2, offset++)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(int value)
    {
        const byte byteSize = Specs.Integer.Int32.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(int value, Span<byte> buffer, ref int offset)
    {
        const byte byteSize = Specs.Integer.Int32.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        buffer[offset++] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = offset + (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2, offset++)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(int value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = (length - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < length; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(long value)
    {
        const byte byteSize = Specs.Integer.Int64.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(long value, Span<byte> buffer, ref int offset)
    {
        const byte byteSize = Specs.Integer.Int64.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        buffer[offset++] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = offset + (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2, offset++)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(long value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = (length - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < length; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    // TODO: This is likely wrong
    public byte[] Encode(BigInteger value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((numberOfDigits % 2) + (numberOfDigits / 2) + 1);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = ((buffer.Length * 2) - numberOfDigits) + 1, j = numberOfDigits - 1; i < (buffer.Length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(BigInteger value, Span<byte> buffer, ref int offset)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        buffer[offset++] = (byte) (value < 0 ? _Negative : _Positive);

        for (int i = offset + ((buffer.Length * 2) - numberOfDigits) + 1, j = numberOfDigits - 1;
            i < (buffer.Length * 2);
            i++, j--, offset++)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        return buffer.ToArray();
    }

    // TODO: This is likely wrong
    public byte[] Encode(BigInteger value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        if (length == numberOfBytes)
            return Encode(value);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((numberOfDigits % 2) + (numberOfDigits / 2));
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? _Negative : _Positive);

        if (length > numberOfBytes)
        {
            for (int i = (length - numberOfBytes) + 1, j = numberOfDigits - 1; i < length; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
            }

            return buffer.ToArray();
        }

        for (int i = (length - numberOfBytes) + 1, j = numberOfDigits - (numberOfDigits - (length * 2)) - 1; i < length; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        return buffer.ToArray();
    }

    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = Encode(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    public override void Encode<_T>(_T value, Span<byte> buffer, ref int offset)
    {
        Type type = typeof(_T);

        if (!type.IsSignedInteger())
            throw new InternalPlayEncodingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.Int8.ByteCount)
            Encode(Unsafe.As<_T, sbyte>(ref value), buffer, ref offset);
        if (byteSize == Specs.Integer.Int16.ByteCount)
            Encode(Unsafe.As<_T, short>(ref value), buffer, ref offset);
        if (byteSize <= Specs.Integer.Int32.ByteCount)
            Encode(Unsafe.As<_T, int>(ref value), buffer, ref offset);
        if (byteSize <= Specs.Integer.Int64.ByteCount)
            Encode(Unsafe.As<_T, long>(ref value), buffer, ref offset);

        Encode(Unsafe.As<_T, BigInteger>(ref value), buffer, ref offset);
    }

    public override void Encode<_T>(_T value, int length, Span<byte> buffer, ref int offset)
    {
        throw new InternalPlayEncodingException(this, typeof(_T));
    }

    public override void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset)
    {
        if (!typeof(_T).IsChar())
            throw new InternalPlayEncodingException(this, typeof(_T));

        Encode(Unsafe.As<_T[], char[]>(ref value), buffer, ref offset);
    }

    public override void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (!typeof(_T).IsChar())
            throw new InternalPlayEncodingException(this, typeof(_T));

        Encode(Unsafe.As<_T[], char[]>(ref value)[..length], buffer, ref offset);
    }

    #endregion

    #region Decode To Chars

    public int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _CharMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _CharMap[value[i]];

            return buffer.ToArray();
        }
    }

    #endregion

    #region Decode To String

    public string DecodeToString(ReadOnlySpan<byte> value) => new(DecodeToChars(value));

    public bool TryDecodingToString(ReadOnlySpan<byte> value, out string result)
    {
        if (!IsValid(value))
        {
            result = string.Empty;

            return false;
        }

        result = DecodeToString(value);

        return true;
    }

    #endregion

    #region Decode To Integers

    public BigInteger DecodeToBigInteger(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        BigInteger result = 0;

        return BuildInteger(result, value);
    }

    private dynamic BuildInteger(dynamic resultBuffer, ReadOnlySpan<byte> value)
    {
        if (resultBuffer != byte.MinValue)
            resultBuffer = 0;

        for (int i = 1, j = (value.Length * 2) - 2; i < value.Length; i++, j -= 2)
            resultBuffer += (byte) (value[i] * Math.Pow(10, j));

        resultBuffer *= value[0] == _Negative ? -1 : 1;

        return resultBuffer;
    }

    #endregion
}