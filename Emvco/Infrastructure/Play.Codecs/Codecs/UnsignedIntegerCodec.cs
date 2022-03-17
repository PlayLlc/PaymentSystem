using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class UnsignedIntegerCodec : PlayCodec
{
    #region Instance Members

    private byte[] GetAllBytes(ulong value, byte byteCount)
    {
        Span<byte> buffer = stackalloc byte[byteCount];
        byte bitShift = 0;
        byte mostSignificantByte = value.GetMostSignificantByte();

        for (int i = 0, j = mostSignificantByte; i < mostSignificantByte; i++, j--)
        {
            buffer[j] = (byte) (value >> bitShift);
            bitShift += 8;
        }

        return buffer.ToArray();
    }

    private byte[] GetTrimmedBytes(ulong value)
    {
        byte mostSignificantByte = value.GetMostSignificantByte();
        Span<byte> buffer = stackalloc byte[mostSignificantByte];
        byte bitShift = 0;

        for (int i = mostSignificantByte - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value >> bitShift);
            bitShift += 8;
        }

        return buffer.ToArray();
    }

    #endregion

    #region Serialization

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length == Specs.Integer.UInt8.ByteCount)
            return new DecodedResult<byte>(value[0], value[0].GetNumberOfDigits());

        if (value.Length <= Specs.Integer.UInt16.ByteCount)
        {
            ushort byteResult = DecodeToUInt16(value);

            return new DecodedResult<ushort>(byteResult, byteResult.GetNumberOfDigits());
        }

        if (value.Length <= Specs.Integer.Int32.ByteCount)
        {
            uint byteResult = DecodeToUInt32(value);

            return new DecodedResult<uint>(byteResult, byteResult.GetNumberOfDigits());
        }

        if (value.Length <= Specs.Integer.Int64.ByteCount)
        {
            ulong byteResult = DecodeToUInt64(value);

            return new DecodedResult<ulong>(byteResult, byteResult.GetNumberOfDigits());
        }

        BigInteger bigIntegerResult = DecodeToBigInteger(value);

        return new DecodedResult<BigInteger>(bigIntegerResult, value.Length * 2);
    }

    #endregion

    #endregion

    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(UnsignedIntegerCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap = new Dictionary<char, byte>
    {
        {'0', 48},
        {'1', 49},
        {'2', 50},
        {'3', 51},
        {'4', 52},
        {'5', 53},
        {'6', 54},
        {'7', 55},
        {'8', 56},
        {'9', 57}
    }.ToImmutableSortedDictionary();

    private static readonly ImmutableSortedDictionary<int, char> _CharMap = new Dictionary<int, char>
    {
        {48, '0'},
        {49, '1'},
        {50, '2'},
        {51, '3'},
        {52, '4'},
        {53, '5'},
        {54, '6'},
        {55, '7'},
        {56, '8'},
        {57, '9'}
    }.ToImmutableSortedDictionary();

    #endregion

    #region Count

    public override ushort GetByteCount<_T>(_T value) => (ushort) Unsafe.SizeOf<_T>();

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override ushort GetByteCount<_T>(_T[] value)
    {
        if (!typeof(_T).IsUnsignedInteger())
            throw new CodecParsingException(this, typeof(_T));

        return (ushort) value.Length;
    }

    public int GetByteCount(char[] chars, int index, int count) => Encode(chars).Length;

    // you're smarter than you think you are - this checks out
    public int GetMaxByteCount(int charCount)
    {
        double maximumIntegerResult = Math.Pow(10, charCount) - 1;

        double result = Math.Log(maximumIntegerResult, 2);

        return (int) ((result % 1) == 0 ? result : result + 1);
    }

    public nint GetCharCount(byte value)
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

    public nint GetCharCount(ushort value)
    {
        const nint valueLength = 5;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(uint value)
    {
        const nint valueLength = 10;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(ulong value)
    {
        const nint valueLength = 20;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public int GetCharCount(BigInteger value) => ((BigInteger) Math.Pow(2, value.GetByteCount() * 8)).GetNumberOfDigits();
    public int GetMaxCharCount(int byteCount) => ((BigInteger) Math.Pow(2, byteCount * 8)).GetNumberOfDigits();

    #endregion

    #region Validation

    public override bool IsValid(ReadOnlySpan<byte> value) => true;

    public bool IsValid(ReadOnlySpan<char> value)
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
        const char minIntegerChar = '0';
        const char maxIntegerChar = '9';

        return value is >= minIntegerChar and <= maxIntegerChar;
    }

    /// <summary>
    ///     Validate
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    protected void Validate(ReadOnlySpan<char> value)
    {
        foreach (char character in value)
        {
            if (!IsValid(character))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
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

    public byte[] Encode(ushort value, bool trimEmptyBytes = false)
    {
        const byte byteCount = sizeof(ushort);

        return trimEmptyBytes ? GetTrimmedBytes(value) : GetAllBytes(value, byteCount);
    }

    public byte[] Encode(uint value, bool trimEmptyBytes = false)
    {
        const byte byteCount = sizeof(uint);

        return trimEmptyBytes ? GetTrimmedBytes(value) : GetAllBytes(value, byteCount);
    }

    public byte[] Encode(ulong value, bool trimEmptyBytes = false)
    {
        const byte byteCount = sizeof(uint);

        return trimEmptyBytes ? GetTrimmedBytes(value) : GetAllBytes(value, byteCount);
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T, char>(ref value));

        if (!type.IsUnsignedInteger())
            throw new CodecParsingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<_T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<_T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<_T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, ulong>(ref value));

        return Encode(Unsafe.As<_T, BigInteger>(ref value));
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T value, int length)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T, char>(ref value));

        if (!type.IsUnsignedInteger())
            throw new CodecParsingException(this, type);

        if (length == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<_T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<_T, ushort>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<_T, uint>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<_T, uint>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, ulong>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, ulong>(ref value));

        return Encode(Unsafe.As<_T, BigInteger>(ref value), length);
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T[] value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T[], char[]>(ref value));

        if (!type.IsByte())
            throw new CodecParsingException(this, type);

        return Encode(Unsafe.As<_T[], byte[]>(ref value));
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T[] value, int length)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T[], char[]>(ref value), length);

        if (!type.IsByte())
            throw new CodecParsingException(this, type);

        return Encode(Unsafe.As<_T[], byte[]>(ref value), length);
    }

    public byte[] Encode(BigInteger value) => value.ToByteArray();

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value)
    {
        Validate(value);
        byte[] result = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = _ByteMap[value[i]];

        return result;
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="chars"></param>
    /// <param name="charIndex"></param>
    /// <param name="charCount"></param>
    /// <param name="bytes"></param>
    /// <param name="byteIndex"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        Validate(chars[charIndex..charCount]);

        Array.ConstrainedCopy(Encode(chars), charIndex, bytes, byteIndex, charCount);

        return charCount;
    }

    public PlayEncodingId GetPlayEncodingId() => EncodingId;

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
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
            throw new CodecParsingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<_T, byte>(ref value), buffer, ref offset);
        else if (byteSize == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<_T, ushort>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<_T, uint>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<_T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<_T, BigInteger>(ref value), buffer, ref offset);
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
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
            throw new CodecParsingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<_T, byte>(ref value), length, buffer, ref offset);
        else if (byteSize == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<_T, ushort>(ref value), length, buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<_T, uint>(ref value), length, buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<_T, ulong>(ref value), length, buffer, ref offset);
        else
            Encode(Unsafe.As<_T, BigInteger>(ref value), length, buffer, ref offset);
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(_T).IsChar())
            Encode(Unsafe.As<_T[], char[]>(ref value));
        else
            throw new CodecParsingException(this, typeof(_T));
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(_T) == typeof(char))
            Encode(Unsafe.As<_T[], char[]>(ref value), length);
        else
            throw new CodecParsingException(this, typeof(_T));
    }

    public void Encode(ushort value, Span<byte> buffer, ref int offset)
    {
        Unsafe.As<byte, ushort>(ref buffer[offset]) = value;
    }

    public void Encode(uint value, Span<byte> buffer, ref int offset)
    {
        Unsafe.As<byte, uint>(ref buffer[offset]) = value;
    }

    public void Encode(ulong value, Span<byte> buffer, ref int offset)
    {
        Unsafe.As<byte, ulong>(ref buffer[offset]) = value;
    }

    public void Encode(BigInteger value, Span<byte> buffer, ref int offset)
    {
        value.ToByteArray(true).AsSpan().CopyTo(buffer[offset..]);
    }

    #endregion

    #region Decode To Chars

    public int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        // TODO: optimize this later instead of defaulting to BigInteger
        BigInteger integerValue = DecodeToBigInteger(bytes[byteIndex..byteCount]);

        for (int i = charIndex; i < integerValue.GetNumberOfDigits(); i++)
        {
            chars[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return byteCount;
    }

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        // TODO: optimize this later instead of defaulting to BigInteger
        BigInteger integerValue = DecodeToBigInteger(value);
        char[] result = new char[integerValue.GetNumberOfDigits()];

        for (int i = 0; i < integerValue.GetNumberOfDigits(); i++)
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

        // TODO: optimize this later instead of defaulting to BigInteger
        BigInteger integerValue = DecodeToBigInteger(value);
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

    public string DecodeToString(ReadOnlySpan<byte> value) => new(DecodeToChars(value));

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

    #endregion

    #region Decode To Integers

    public BigInteger DecodeToBigInteger(ReadOnlySpan<byte> value) => new(value);

    public uint DecodeToUInt32(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.UInt32.ByteCount;

        if (value.Length == 0)
            return 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return DecodeToUInt32(value);
        }

        if (value.Length > byteLength)
            return DecodeToUInt32(value[..byteLength]);

        uint result = 0;

        for (int i = 0; i < (value.Length - 1); i++)
        {
            result |= value[i];
            result <<= 8;
        }

        result |= value[^1];

        return result;
    }

    public ulong DecodeToUInt64(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.UInt64.ByteCount;

        if (value.Length == 0)
            return 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return DecodeToUInt64(value);
        }

        if (value.Length > byteLength)
            return DecodeToUInt64(value[..byteLength]);

        ulong result = 0;

        for (int i = 0; i < (value.Length - 1); i++)
        {
            result |= value[i];
            result <<= 8;
        }

        result |= value[^1];

        return result;
    }

    public ushort DecodeToUInt16(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.UInt16.ByteCount;

        if (value.Length == 0)
            return 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return DecodeToUInt16(value);
        }

        if (value.Length > byteLength)
            return DecodeToUInt16(value[..byteLength]);

        ushort result = 0;

        for (int i = 0; i < (value.Length - 1); i++)
        {
            result |= value[i];
            result <<= 8;
        }

        result |= value[^1];

        return result;
    }

    public byte DecodeToByte(ReadOnlySpan<byte> value)
    {
        if (value.Length == 0)
            return 0;

        return value[0];
    }

    #endregion
}