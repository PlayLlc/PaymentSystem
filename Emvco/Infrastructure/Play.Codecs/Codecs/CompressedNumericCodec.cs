using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class CompressedNumericCodec : PlayCodec
{
    #region Instance Members

    private static int GetPadCount(ReadOnlySpan<char> value)
    {
        int offset = 0;

        for (; offset < value.Length;)
        {
            if (value[offset] != _PaddingCharKey)
                break;

            offset++;
        }

        return offset;
    }

    public static int GetPadCount(ReadOnlySpan<byte> value)
    {
        int offset = value.Length;

        for (; offset > 0;)
        {
            if (value[offset] != _PaddedByte)
                break;

            offset -= 2;
        }

        if (value[offset].AreBitsSet(_PaddedRightNibble))
            offset++;

        return offset;
    }

    private int Pad(Span<byte> buffer)
    {
        int padCount = GetPadCount(buffer);
        buffer[^(padCount / 2)..].Fill(_PaddedByte);
        if ((padCount % 2) != 0)
            buffer[^((padCount / 2) + 1)] |= _PaddedRightNibble;

        return padCount;
    }

    #endregion

    #region Serialization

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        BigInteger maximumIntegerResult = (BigInteger) Math.Pow(2, value.Length * 8);

        if (maximumIntegerResult <= byte.MaxValue)
        {
            byte result = DecodeToByte(value[0]);

            return new DecodedResult<byte>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= ushort.MaxValue)
        {
            ushort result = DecodeToUInt16(value);

            return new DecodedResult<ushort>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= uint.MaxValue)
        {
            uint result = DecodeToUInt32(value);

            return new DecodedResult<uint>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= ulong.MaxValue)
        {
            ulong result = DecodeToUInt64(value);

            return new DecodedResult<ulong>(result, result.GetNumberOfDigits());
        }
        else
        {
            BigInteger result = DecodeToBigInteger(value);

            return new DecodedResult<BigInteger>(result, result.GetNumberOfDigits());
        }
    }

    #endregion

    #endregion

    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(CompressedNumericCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    private const char _PaddingCharKey = 'F';
    private const byte _PaddedLeftNibble = 0xF0;
    private const byte _PaddedRightNibble = 0xF;
    private const byte _PaddedByte = 0xFF;

    #endregion

    #region Count

    // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
    public override ushort GetByteCount<T>(T value) where T : struct => checked((ushort) Unsafe.SizeOf<T>());

    public override ushort GetByteCount<T>(T[] value) where T : struct
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        if (typeof(T) == typeof(byte))
            return (ushort) ((ushort) value.Length * 2);

        if (typeof(T) == typeof(char))
            return (ushort) (((ushort) value.Length / 2) + (value.Length % 2));

        throw new NotImplementedException();
    }

    public int GetByteCount(char[] chars, int index, int count) => GetMaxByteCount(count);
    public int GetMaxByteCount(int charCount) => charCount / 2;
    public int GetCharCount(byte[] bytes, int index, int count) => GetMaxCharCount(count);
    public int GetMaxCharCount(int byteCount) => byteCount * 2;

    #endregion

    #region Validation

    public bool IsValid(ReadOnlySpan<char> value)
    {
        int padCount = GetPadCount(value);

        for (int i = padCount; i < value.Length; i++)
        {
            if (!_ByteMap.ContainsKey(value[i]))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        int padCount = GetPadCount(value);

        for (int i = 0, j = 0; j < ((value.Length * 2) - padCount); i += j % 2, j++)
        {
            if ((padCount % 2) != 0)
            {
                if (!_CharMap.ContainsKey(value[i].GetMaskedValue(_PaddedLeftNibble)))
                    return false;
            }
            else
            {
                if (!_CharMap.ContainsKey(value[i].GetMaskedValue(_PaddedRightNibble)))
                    return false;
            }
        }

        return true;
    }

    public bool IsValid(byte value)
    {
        if (value == _PaddedByte)
            return true;

        if (!value.AreBitsSet(_PaddedLeftNibble))
        {
            if (!_CharMap.ContainsKey((byte) (value >> 4)))
                return false;
        }

        if (!_CharMap.ContainsKey(value.GetMaskedValue(_PaddedLeftNibble)))
            return false;

        return true;
    }

    public bool IsValid(char value) => _ByteMap.ContainsKey(value);

    #endregion

    #region Encode

    /// <summary>
    ///     TryEncoding
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        // wrong
        result = AlphaNumericCodec.Encode(value);

        return true;
    }

    public byte[] Encode(string value)
    {
        if ((value.Length % 2) != 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        int length = value.Length / 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[length];

            for (int i = 0, j = 0; i < length; i++, j += 2)
                buffer[i] = DecodeToByte(value[j], value[j + 1]);

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < length; i++, j += 2)
                buffer[i] = DecodeToByte(value[j], value[j + 1]);

            return buffer.ToArray();
        }
    }

    // DEPRECATING: This method will eventually be deprecated in favor of strongly typed arguments
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        if (!typeof(T).IsNumericType())
            throw new CodecParsingException(this, typeof(T));

        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    // DEPRECATING: This method will eventually be deprecated in favor of strongly typed arguments
    /// <exception cref="Exception"></exception>
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

    // DEPRECATING: This method will eventually be deprecated in favor of strongly typed arguments
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value));

        throw new NotImplementedException();
    }

    // DEPRECATING: This method will eventually be deprecated in favor of strongly typed arguments
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value), length);

        throw new NotImplementedException();
    }

    public byte[] Encode(ReadOnlySpan<char> value) => Encode(value, value.Length);

    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        int byteSize = (value.Length / 2) + (value.Length % 2);

        if (byteSize > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
            Span<byte> result = spanOwner.Span;

            int padCount = Pad(result);

            for (int i = 0, j = 0; j < ((value.Length * 2) - padCount); i += j % 2, j++)
            {
                try
                {
                    if ((padCount % 2) == 0)
                        result[i] |= (byte) (_ByteMap[value[j]] << 4);
                    else
                        result[i] |= _ByteMap[value[j]];
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new CodecParsingException(
                        $"The value could not be encoded by {nameof(CompressedNumericCodec)} because there was an invalid character",
                        exception);
                }
            }

            return result.ToArray();
        }
        else
        {
            Span<byte> result = stackalloc byte[byteSize];

            int padCount = Pad(result);

            for (int i = 0, j = 0; j < ((value.Length * 2) - padCount); i += j % 2, j++)
            {
                try
                {
                    if ((padCount % 2) == 0)
                        result[i] |= (byte) (_ByteMap[value[j]] << 4);
                    else
                        result[i] |= _ByteMap[value[j]];
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new CodecParsingException(
                        $"The value could not be encoded by {nameof(CompressedNumericCodec)} because there was an invalid character",
                        exception);
                }
            }

            return result.ToArray();
        }
    }

    public byte[] Encode(byte value)
    {
        return new[] {value};
    }

    public byte[] Encode(ushort value)
    {
        const byte byteSize = Specs.Integer.UInt16.ByteCount;
        int padCount = value.GetNumberOfDigits() - (byteSize * 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(uint value)
    {
        const byte byteSize = Specs.Integer.UInt32.ByteCount;
        int padCount = value.GetNumberOfDigits() - (byteSize * 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public byte[] Encode(uint value, int length)
    {
        const byte byteSize = Specs.Integer.UInt32.ByteCount;

        if (length > (byteSize * 2))
            throw new Exception();

        int padCount = (byteSize * 2) - value.GetNumberOfDigits();

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(ulong value)
    {
        const byte byteSize = Specs.Integer.UInt64.ByteCount;
        int padCount = value.GetNumberOfDigits() - (byteSize * 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public byte[] Encode(ulong value, int length)
    {
        const byte byteSize = Specs.Integer.UInt64.ByteCount;

        if (length > (byteSize * 2))
            throw new Exception();

        int padCount = (byteSize * 2) - value.GetNumberOfDigits();

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(BigInteger value)
    {
        int numberOfDigits = value.GetNumberOfDigits();
        int byteSize = (numberOfDigits / 2) + (numberOfDigits % 2);
        int padCount = numberOfDigits % 2;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / (BigInteger) Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(BigInteger value, int length)
    {
        int byteSize = (length / 2) + (length % 2);
        int maxNumberOfDigits = byteSize * 2;
        int padCount = maxNumberOfDigits - (maxNumberOfDigits - length);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = maxNumberOfDigits - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / (BigInteger) Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    // //////////////////////////////END

    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = Encode(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value));
        else
            throw new NotImplementedException();
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value), length);
        else
            throw new NotImplementedException();
    }

    // //////////////////////////////START

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<T, byte>(ref value), buffer, ref offset);
        else if (byteSize == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<T, BigInteger>(ref value), buffer, ref offset);
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        if (length == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<T, byte>(ref value));
        else if (length == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<T, ushort>(ref value));
        else if (length == 3)
            Encode(Unsafe.As<T, uint>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<T, uint>(ref value), buffer, ref offset);
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
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        int byteSize = (length / 2) + (length % 2);

        int padCount = Pad(buffer[offset..]);

        for (int i = 0, j = 0; j < ((length * 2) - padCount); i += j % 2, j++)
        {
            try
            {
                if ((padCount % 2) == 0)
                    buffer[i] |= (byte) (_ByteMap[value[j]] << 4);
                else
                    buffer[i] |= _ByteMap[value[j]];
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new CodecParsingException(
                    $"The value could not be encoded by {nameof(CompressedNumericCodec)} because there was an invalid character",
                    exception);
            }
        }

        offset += byteSize;
    }

    #endregion

    #region Decode To Chars

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                DecodeToChars(value[i], buffer, j);

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                DecodeToChars(value[i], buffer, j);

            return buffer.ToArray();
        }
    }

    private void DecodeToChars(byte value, Span<char> buffer, int offset)
    {
        buffer[offset++] = _CharMap[(byte) (value / 10)];
        buffer[offset] = _CharMap[(byte) (value % 10)];
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

    public byte DecodeToByte(byte value)
    {
        int leftNibble = value >> 4;
        byte rightNibble = (byte) (value & ~0xF0);

        return (byte) ((leftNibble * 10) + rightNibble);
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint DecodeToUInt32(ReadOnlySpan<byte> value)
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
    public ulong DecodeToUInt64(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        ulong result = 0;

        return (ulong) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ushort DecodeToUInt16(ReadOnlySpan<byte> value)
    {
        ushort result = 0;

        return (ushort) BuildInteger(result, value);
    }

    private static byte DecodeToByte(char leftChar, char rightChar)
    {
        byte result = _ByteMap[leftChar];
        result *= 10;
        result += _ByteMap[rightChar];

        return result;
    }

    private dynamic BuildInteger(dynamic resultBuffer, ReadOnlySpan<byte> value)
    {
        if (resultBuffer != byte.MinValue)
            resultBuffer = 0;

        for (int i = 0, j = (value.Length * 2) - 2; i < value.Length; i++, j -= 2)
            resultBuffer += DecodeToByte(value[i]) * Math.Pow(10, j);

        return resultBuffer;
    }

    #endregion
}