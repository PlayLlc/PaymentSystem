using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Codecs.Metadata;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class SignedIntegerCodec : PlayCodec
{
    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(SignedIntegerCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(48, 57 - 48).ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(48, 57 - 48).ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

    #endregion

    #region Count

    public int GetByteCount(char[] chars, int index, int count)
    {
        if (Specs.Integer.Int8.MaxDigits <= chars.Length)
            return Specs.Integer.Int8.ByteCount;
        if (Specs.Integer.Int16.MaxDigits <= chars.Length)
            return Specs.Integer.Int16.ByteCount;
        if (Specs.Integer.Int32.MaxDigits <= chars.Length)
            return Specs.Integer.Int32.ByteCount;
        if (Specs.Integer.Int64.MaxDigits <= chars.Length)
            return Specs.Integer.Int64.ByteCount;
        if (Specs.Integer.Int8.MaxDigits <= chars.Length)
            return Specs.Integer.Int8.ByteCount;

        return Encode(chars[index..count]).Length;
    }

    public int GetMaxByteCount(int charCount)
    {
        double maximumIntegerResult = Math.Pow(10, charCount) - 1;

        double result = Math.Log(maximumIntegerResult, 2);

        return (int) ((result % 1) == 0 ? result : result + 1);
    }

    public override ushort GetByteCount<_T>(_T value) => (ushort) Unsafe.SizeOf<_T>();

    public override ushort GetByteCount<_T>(_T[] value)
    {
        Type type = typeof(_T);

        if (type.IsByte())
            return (ushort) value.Length;

        if (type.IsChar())
            return (ushort) Encode(Unsafe.As<_T[], char[]>(ref value)).Length;

        throw new InternalPlayEncodingException(this, type);
    }

    public int GetCharCount(byte[] bytes, int index, int count) => DecodeToChars(bytes[index..count]).Length;

    public nint GetCharCount(sbyte value)
    {
        const nint valueLength = 3;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(short value)
    {
        const nint valueLength = 3;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(int value)
    {
        const nint valueLength = 3;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(long value)
    {
        const nint valueLength = 3;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(BigInteger value) => GetCharCount(value.ToByteArray(), 0, value.GetByteCount());

    // There is an extra character for a negative sign '-'
    public int GetMaxCharCount(int byteCount) => ((BigInteger) Math.Pow(2, byteCount * 8)).GetNumberOfDigits() + 1;

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

    private bool IsValid(char value)
    {
        const char minCharValue = '0';
        const char maxCharValue = '9';

        if (value is >= minCharValue and <= maxCharValue)
            return true;

        return false;
    }

    private bool IsValid(byte value)
    {
        const byte minCharValue = (byte) '0';
        const byte maxCharValue = (byte) '9';

        if (value is >= minCharValue and <= maxCharValue)
            return true;

        return false;
    }

    private void Validate(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Validate(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                throw new ArgumentOutOfRangeException();
        }
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

        result = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = _ByteMap[value[i]];

        return true;
    }

    public override byte[] Encode<_T>(_T value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T, char>(ref value));

        if (!type.IsSignedInteger())
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
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T, char>(ref value));

        if (!type.IsSignedInteger())
            throw new InternalPlayEncodingException(this, type);

        if (length == Specs.Integer.Int8.ByteCount)
            return Encode(Unsafe.As<_T, sbyte>(ref value));
        if (length == Specs.Integer.Int16.ByteCount)
            return Encode(Unsafe.As<_T, short>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<_T, int>(ref value), length);
        if (length == Specs.Integer.Int32.ByteCount)
            return Encode(Unsafe.As<_T, int>(ref value));
        if (length < Specs.Integer.Int64.ByteCount)
            return Encode(Unsafe.As<_T, long>(ref value), length);
        if (length == Specs.Integer.Int64.ByteCount)
            return Encode(Unsafe.As<_T, long>(ref value));

        return Encode(Unsafe.As<_T, BigInteger>(ref value), length);
    }

    public override byte[] Encode<_T>(_T[] value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T[], char[]>(ref value));

        if (!type.IsByte())
            throw new InternalPlayEncodingException(this, type);

        return Encode(Unsafe.As<_T[], byte[]>(ref value));
    }

    public override byte[] Encode<_T>(_T[] value, int length)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T[], char[]>(ref value), length);

        if (!type.IsByte())
            throw new InternalPlayEncodingException(this, type);

        return Encode(Unsafe.As<_T[], byte[]>(ref value), length);
    }

    public byte[] Encode(sbyte value) => BitConverter.GetBytes(value);
    public byte[] Encode(short value) => BitConverter.GetBytes(value);
    public byte[] Encode(int value) => BitConverter.GetBytes(value);
    public byte[] Encode(long value) => BitConverter.GetBytes(value);

    public byte[] Encode(ReadOnlySpan<char> value)
    {
        ReadOnlySpan<char> buffer = value[0] == '-' ? value[1..] : value;
        Validate(value);
        byte[] result = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = _ByteMap[value[i]];

        return result;
    }

    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        Validate(chars[charIndex..charCount]);

        Array.ConstrainedCopy(Encode(chars), charIndex, bytes, byteIndex, charCount);

        return charCount;
    }

    public override void Encode<_T>(_T value, Span<byte> buffer, ref int offset)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
        {
            Encode(Unsafe.As<_T, char>(ref value), buffer, ref offset);

            return;
        }

        if (!type.IsNumericType())
            throw new InternalPlayEncodingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.Int8.ByteCount)
            Encode(Unsafe.As<_T, sbyte>(ref value), buffer, ref offset);
        else if (byteSize == Specs.Integer.Int16.ByteCount)
            Encode(Unsafe.As<_T, short>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.Int32.ByteCount)
            Encode(Unsafe.As<_T, int>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.Int64.ByteCount)
            Encode(Unsafe.As<_T, long>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<_T, BigInteger>(ref value), buffer, ref offset);
    }

    public override void Encode<_T>(_T value, int length, Span<byte> buffer, ref int offset)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
        {
            Encode(Unsafe.As<_T, char>(ref value), length, buffer, ref offset);

            return;
        }

        if (!type.IsNumericType())
            throw new InternalPlayEncodingException(this, type);

        if (length == Specs.Integer.Int8.ByteCount)
            Encode(Unsafe.As<_T, sbyte>(ref value));
        else if (length == Specs.Integer.Int16.ByteCount)
            Encode(Unsafe.As<_T, short>(ref value));
        else if (length == 3)
            Encode(Unsafe.As<_T, int>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.Int32.ByteCount)
            Encode(Unsafe.As<_T, uint>(ref value), buffer, ref offset);
        else if (length < Specs.Integer.Int64.ByteCount)
            Encode(Unsafe.As<_T, long>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.Int64.ByteCount)
            Encode(Unsafe.As<_T, long>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<_T, BigInteger>(ref value), length, buffer, ref offset);
    }

    public override void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(_T).IsChar())
            Encode(Unsafe.As<_T[], char[]>(ref value));
        else
            throw new InternalPlayEncodingException(this, typeof(_T));
    }

    public override void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(_T) == typeof(char))
            Encode(Unsafe.As<_T[], char[]>(ref value), length);
        else
            throw new InternalPlayEncodingException(this, typeof(_T));
    }

    public void Encode(sbyte value, Span<byte> buffer, ref int offset)
    {
        Unsafe.As<byte, sbyte>(ref buffer[offset]) = value;
        offset++;
    }

    public void Encode(short value, Span<byte> buffer, ref int offset)
    {
        Unsafe.As<byte, short>(ref buffer[offset]) = value;
        offset += 2;
    }

    public void Encode(int value, Span<byte> buffer, ref int offset)
    {
        Unsafe.As<byte, int>(ref buffer[offset]) = value;
        offset += 4;
    }

    public void Encode(long value, Span<byte> buffer, ref int offset)
    {
        Unsafe.As<byte, long>(ref buffer[offset]) = value;
        offset += 8;
    }

    public void Encode(BigInteger value, Span<byte> buffer, ref int offset)
    {
        value.ToByteArray(false).AsSpan().CopyTo(buffer[offset..]);
        offset += value.GetByteCount();
    }

    #endregion

    #region Decode To Chars

    public int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        dynamic integerValue = GetInteger(bytes[byteIndex..byteCount]);

        for (int i = charIndex; i < integerValue.GetNumberOfDigits(); i++)
        {
            chars[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return byteCount;
    }

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        Validate(value);

        dynamic integerValue = GetInteger(value);
        char[] result = new char[integerValue.GetNumberOfDigits()];

        result[0] = '-';

        for (int i = 1; i < integerValue.GetNumberOfDigits(); i++)
        {
            result[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return result;
    }

    public bool TryDecodingToChars(ReadOnlySpan<byte> value, out char[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<char>();

            return false;
        }

        dynamic integerValue = GetInteger(value);
        result = new char[integerValue.GetNumberOfDigits()];

        for (int i = 0; i < integerValue.GetNumberOfDigits(); i++)
        {
            result[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return true;
    }

    #endregion

    #region Decode To String

    public bool TryDecodingToString(ReadOnlySpan<byte> value, out string result)
    {
        if (!TryDecodingToChars(value, out char[] buffer))
        {
            result = string.Empty;

            return false;
        }

        result = new string(buffer);

        return true;
    }

    public string DecodeToString(ReadOnlySpan<byte> value) => new(DecodeToChars(value));

    #endregion

    #region Decode To Integers

    public BigInteger DecodeToBigInteger(ReadOnlySpan<byte> value) => new(value);

    #endregion

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length == Specs.Integer.Int8.ByteCount)
        {
            sbyte byteResult = DecodeToSByte(value);

            int charLength = byteResult switch
            {
                >= 0 when byteResult <= 9 => 1,
                >= 0 when byteResult <= 99 => 2,
                >= 0 => 3,
                >= -9 => 1 + 1,
                >= -99 => 2 + 1,
                _ => 3 + 1
            };

            return new DecodedResult<sbyte>(byteResult, charLength);
        }

        if (value.Length <= Specs.Integer.Int16.ByteCount)
        {
            short byteResult = DecodeToInt16(value);

            return new DecodedResult<short>(byteResult, byteResult.GetNumberOfDigits());
        }

        if (value.Length <= Specs.Integer.Int32.ByteCount)
        {
            int byteResult = DecodeToInt32(value);

            return new DecodedResult<int>(byteResult, byteResult.GetNumberOfDigits());
        }

        if (value.Length <= Specs.Integer.Int64.ByteCount)
        {
            long byteResult = DecodeToInt64(value);

            return new DecodedResult<long>(byteResult, byteResult.GetNumberOfDigits());
        }

        BigInteger bigIntegerResult = DecodeToBigInteger(value);

        return new DecodedResult<BigInteger>(bigIntegerResult, value.Length * 2);
    }

    #endregion

    #region Instance Members

    public sbyte DecodeToSByte(ReadOnlySpan<byte> value)
    {
        if (value[0] > sbyte.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value));

        return (sbyte) value[0];
    }

    public short DecodeToInt16(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.Int16.ByteCount;
        const ushort max = ushort.MaxValue;
        short result = 0;
        byte bitShift = 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return DecodeToInt16(value);
        }

        if (value.Length > byteLength)
            return DecodeToInt16(value[..byteLength]);

        for (int i = 0; i < value.Length; i++)
        {
            result |= (short) (value[i] << bitShift);
            bitShift += 8;
        }

        result |= (short) (max & (ushort) ~GetMask(value.Length));

        return result;
    }

    public int DecodeToInt32(ReadOnlySpan<byte> value)
    {
        const uint max = uint.MaxValue;
        const byte byteLength = Specs.Integer.Int32.ByteCount;
        int result = 0;
        byte bitShift = 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return DecodeToInt32(value);
        }

        if (value.Length > byteLength)
            return DecodeToInt32(value[..byteLength]);

        for (int i = 0; i < value.Length; i++)
        {
            result |= value[i] << bitShift;
            bitShift += 8;
        }

        result |= (int) (max & (uint) ~GetMask(value.Length));

        return result;
    }

    public long DecodeToInt64(ReadOnlySpan<byte> value)
    {
        const ulong max = ulong.MaxValue;
        const byte byteLength = Specs.Integer.Int64.ByteCount;
        long result = 0;
        int bitShift = 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return DecodeToInt64(value);
        }

        if (value.Length > byteLength)
            return DecodeToInt64(value[..byteLength]);

        for (int i = 0; i < value.Length; i++)
        {
            result |= (long) value[i] << bitShift;
            bitShift += 8;
        }

        result |= (long) (max & (ulong) ~GetMask(value.Length));

        return result;
    }

    private dynamic GetInteger(ReadOnlySpan<byte> value)
    {
        return value.Length switch
        {
            1 => DecodeToSByte(value),
            2 => DecodeToInt16(value),
            4 => DecodeToInt32(value),
            8 => DecodeToInt64(value),
            _ => new BigInteger(value)
        };
    }

    private dynamic GetMask(int byteCount)
    {
        const byte byteMax = byte.MaxValue;
        int result = 0;
        for (int i = 0; i < byteCount; i++)
            result |= byteMax << (i * 8);

        return result;
    }

    #endregion
}