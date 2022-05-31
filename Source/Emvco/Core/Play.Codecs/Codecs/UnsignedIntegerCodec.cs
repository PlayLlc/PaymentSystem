using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Core.Extensions.Types;
using Play.Core.Specifications;

namespace Play.Codecs;

public class UnsignedIntegerCodec : PlayCodec
{
    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(UnsignedIntegerCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        new Dictionary<char, byte>
        {
            {'0', 0},
            {'2', 2},
            {'1', 1},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9}
        }.ToImmutableSortedDictionary();

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        new Dictionary<byte, char>
        {
            {0, '0'},
            {1, '1'},
            {2, '2'},
            {3, '3'},
            {4, '4'},
            {5, '5'},
            {6, '6'},
            {7, '7'},
            {8, '8'},
            {9, '9'}
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
        if (!typeof(_T).IsByte())
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

    #endregion

    #region Encode

    // DEPRECATING: This method will eventually be deprecated in favor of a method that passes in a Span<T> buffer
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

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public byte[] Encode(ulong value, bool trimEmptyBytes = false)
    {
        return LocalEncode(value, trimEmptyBytes ? value.GetMostSignificantByte() : Specs.Integer.UInt64.ByteCount);

        static byte[] LocalEncode(ulong valueToTrim, byte length)
        {
            Span<byte> buffer = stackalloc byte[length];
            byte bitShift = 0;

            for (int i = length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (valueToTrim >> bitShift);
                bitShift += 8;
            }

            return buffer.ToArray();
        }
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public byte[] Encode(ushort value, bool trimEmptyBytes = false)
    {
        return LocalEncode(value, trimEmptyBytes ? value.GetMostSignificantByte() : Specs.Integer.UInt16.ByteCount);

        static byte[] LocalEncode(ushort valueToTrim, byte length)
        {
            Span<byte> buffer = stackalloc byte[length];
            byte bitShift = 0;

            for (int i = length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (valueToTrim >> bitShift);
                bitShift += 8;
            }

            return buffer.ToArray();
        }
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public byte[] Encode(uint value, bool trimEmptyBytes = false)
    {
        return LocalEncode(value, trimEmptyBytes ? value.GetMostSignificantByte() : Specs.Integer.UInt32.ByteCount);

        static byte[] LocalEncode(uint valueToTrim, byte length)
        {
            Span<byte> buffer = stackalloc byte[length];
            byte bitShift = 0;

            for (int i = length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (valueToTrim >> bitShift);
                bitShift += 8;
            }

            return buffer.ToArray();
        }
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public byte[] Encode(BigInteger value) => value.ToByteArray();

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints. We will also include a Span<byte> in the argument as a buffer as opposed to returning a new byte[]
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

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints. We will also include a Span<byte> in the argument as a buffer as opposed to returning a new byte[]
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

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints. We will also include a Span<byte> in the argument as a buffer as opposed to returning a new byte[]
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

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints. We will also include a Span<byte> in the argument as a buffer as opposed to returning a new byte[]
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

    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value)
    {
        // HACK: This isn't the most efficient if we're initializing an extra BigInteger buffer. Figure out the correct way to do this without using the BigInteger as a crutch

        Validate(value);
        BigInteger buffer = 0;

        for (int i = 0; i < (value.Length - 1); i++)
        {
            buffer += _ByteMap[value[i]];
            buffer *= 10;
        }

        buffer += _ByteMap[value[^1]];

        return buffer.ToByteArray();
    }

    public void Encode(ushort value, Span<byte> buffer, bool trimEmptyBytes = false)
    {
        LocalEncode(value, buffer, trimEmptyBytes ? value.GetMostSignificantByte() : Specs.Integer.UInt16.ByteCount);

        static void LocalEncode(ushort valueToTrim, Span<byte> buffer, byte length)
        {
            byte bitShift = 0;

            for (int i = length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (valueToTrim >> bitShift);
                bitShift += 8;
            }
        }
    }

    public void Encode(uint value, Span<byte> buffer, bool trimEmptyBytes = false)
    {
        LocalEncode(value, buffer, trimEmptyBytes ? value.GetMostSignificantByte() : Specs.Integer.UInt32.ByteCount);

        static void LocalEncode(uint valueToTrim, Span<byte> buffer, byte length)
        {
            byte bitShift = 0;

            for (int i = length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (valueToTrim >> bitShift);
                bitShift += 8;
            }
        }
    }

    public void Encode(ulong value, Span<byte> buffer, bool trimEmptyBytes = false)
    {
        LocalEncode(value, buffer, trimEmptyBytes ? value.GetMostSignificantByte() : Specs.Integer.UInt64.ByteCount);

        static void LocalEncode(ulong valueToTrim, Span<byte> buffer, byte length)
        {
            byte bitShift = 0;

            for (int i = length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (valueToTrim >> bitShift);
                bitShift += 8;
            }
        }
    }

    public void Encode(BigInteger value, Span<byte> buffer) => value.ToByteArray().AsSpan().CopyTo(buffer);

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints. We will also include a Span<byte> in the argument as a buffer as opposed to returning a new byte[]
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

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints. We will also include a Span<byte> in the argument as a buffer as opposed to returning a new byte[]
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

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints. We will also include a Span<byte> in the argument as a buffer as opposed to returning a new byte[]
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(_T).IsChar())
            Encode(Unsafe.As<_T[], char[]>(ref value));
        else
            throw new CodecParsingException(this, typeof(_T));
    }

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints. We will also include a Span<byte> in the argument as a buffer as opposed to returning a new byte[]
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(_T) == typeof(char))
            Encode(Unsafe.As<_T[], char[]>(ref value), length);
        else
            throw new CodecParsingException(this, typeof(_T));
    }

    #endregion

    #region Decode To Chars

    public int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        // HACK: This isn't the most efficient if we're initializing an extra BigInteger buffer. Figure out the correct way to do this without using the BigInteger as a crutch

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
        // HACK: This isn't the most efficient if we're initializing an extra BigInteger buffer. Figure out the correct way to do this without using the BigInteger as a crutch

        BigInteger integerValue = DecodeToBigInteger(value);
        char[] result = new char[integerValue.GetNumberOfDigits()];

        for (int i = result.Length - 1; i >= 0; i--)
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

        // HACK: This isn't the most efficient if we're initializing an extra BigInteger buffer. Figure out the correct way to do this without using the BigInteger as a crutch

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

    public byte DecodeToByte(ReadOnlySpan<byte> value)
    {
        if (value.Length == 0)
            return 0;

        return value[0];
    }

    public uint DecodeToUInt32(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.UInt32.ByteCount;
        Span<byte> buffer = stackalloc byte[byteLength];

        if (value.Length == 0)
            return 0;

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

    #endregion

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

    #region Instance Members

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
}