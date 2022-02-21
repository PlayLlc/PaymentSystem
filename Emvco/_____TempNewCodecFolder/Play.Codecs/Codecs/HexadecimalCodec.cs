using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Codecs.Metadata;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class HexadecimalCodec : PlayCodec
{
    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(HexadecimalCodec));

    #endregion

    #region Count

    public int GetByteCount(char[] chars, int index, int count)
    {
        if ((index + count) > chars.Length)
            throw new ArgumentOutOfRangeException();

        if ((count % 2) != 0)
            throw new ArgumentOutOfRangeException();

        return count / 2;
    }

    public int GetMaxByteCount(int charCount)
    {
        if (charCount.TryGetRemainder(2, out int resultWithoutRemainder) == 0)
            return resultWithoutRemainder;

        return resultWithoutRemainder + 1;
    }

    public override ushort GetByteCount<_T>(_T value) => throw new NotImplementedException();
    public override ushort GetByteCount<_T>(_T[] value) => throw new NotImplementedException();

    public int GetCharCount(byte[] bytes, int index, int count)
    {
        if (index > bytes.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        if ((index + count) > bytes.Length)
            throw new ArgumentOutOfRangeException();

        return (index + count) * 2;
    }

    public int GetMaxCharCount(int byteCount) => byteCount * 2;

    #endregion

    #region Validation

    /// <summary>
    ///     Checks if the value contains valid hexadecimal bytes
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        try
        {
            CheckCore.ForEmptySequence(value, nameof(value));

            return true;
        }
        catch (Exception)
        {
            // TODO: log and catch less ambiguous exceptions
            return false;
        }
    }

    /// <summary>
    ///     Checks if the value contains valid Hexadecimal characters
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsValid(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        for (int i = 0; i < (value.Length - 1); i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public static bool IsValid(char c)
    {
        if ((c > 0xFF) || (c < 0))
            return false;

        return Lookup.CharToHex[c] != 0xFF;
    }

    #endregion

    #region Encode

    /// <summary>
    ///     Encodes a sequence of characters into a byte array
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public bool TryEncode(ReadOnlySpan<char> value, out byte[] result)
    {
        try
        {
            CheckCore.ForEmptySequence(value, nameof(value));

            result = Encode(value);

            return true;
        }
        catch (PlayEncodingException)
        {
            result = new byte[0];

            return false;
        }
    }

    /// <summary>
    ///     Encodes a sequence of characters into a byte array
    /// </summary>
    /// <param name="value"></param>
    /// < returns></returns>
    /// < exception cref="PlayEncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        bool lengthIsEven = (value.Length % 2) == 0;
        byte[] result = new byte[lengthIsEven ? value.Length / 2 : (value.Length + 1) / 2];
        int resultIndex = 0;
        int stringIndex = 0;

        if (!lengthIsEven)
            result[resultIndex++] = (byte) (0x00 | DecodeToByte(value[stringIndex++]));

        for (; resultIndex < result.Length; stringIndex += 2)
            result[resultIndex++] = (byte) ((DecodeToByte(value[stringIndex]) << 4) | DecodeToByte(value[stringIndex + 1]));

        return result;
    }

    public byte[] Encode(byte value) => UnsignedIntegerCodec.Encode(value);
    public byte[] Encode(ushort value) => UnsignedIntegerCodec.Encode(value);
    public byte[] Encode(uint value) => UnsignedIntegerCodec.Encode(value);
    public byte[] Encode(ulong value) => UnsignedIntegerCodec.Encode(value);
    public byte[] Encode(BigInteger value) => value.ToByteArray();

    public override byte[] Encode<_T>(_T value)
    {
        if (typeof(_T) == typeof(char))
            return Encode(Unsafe.As<_T, char>(ref value));

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<_T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteSize)
            return Encode(Unsafe.As<_T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<_T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, ulong>(ref value));

        return Encode(Unsafe.As<_T, BigInteger>(ref value));
    }

    public override byte[] Encode<_T>(_T value, int length)
    {
        if (typeof(_T) == typeof(char))
            return Encode(Unsafe.As<_T, char>(ref value));
        if (length == Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<_T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteSize)
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

    public override byte[] Encode<_T>(_T[] value)
    {
        if (typeof(_T) == typeof(char))
            return Encode(Unsafe.As<_T[], char[]>(ref value));
        if (typeof(_T) == typeof(byte))
            return Encode(Unsafe.As<_T[], byte[]>(ref value));
    }

    public override byte[] Encode<_T>(_T[] value, int length) => throw new NotImplementedException();

    /// <exception cref="PlayEncodingException"></exception>
    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        CheckCore.ForEmptySequence(chars, nameof(chars));

        if ((charCount + charIndex) > chars.Length)
            throw new ArgumentOutOfRangeException();

        if (byteIndex > (bytes.Length - 1))
            throw new ArgumentOutOfRangeException();

        int indexRangeEnd = charIndex + charCount;
        int indexRangeLength = indexRangeEnd - charIndex;
        int bytesEncoded = indexRangeLength / 2;

        if (bytes.Length < (byteIndex + bytesEncoded))
            throw new ArgumentOutOfRangeException(nameof(bytes), "The byte buffer passed to the argument is smaller than expected");

        if ((indexRangeLength % 2) != 0)
            throw new ArgumentOutOfRangeException();

        for (int i = charIndex, j = byteIndex; i <= indexRangeEnd; i += 2, j++)
            bytes[j] = (byte) ((DecodeToByte(chars[i]) << 4) | DecodeToByte(chars[i + 1]));

        return bytesEncoded;
    }

    public void Encode(byte value, Span<byte> buffer, ref int offset)
    {
        buffer[offset++] = value;
    }

    public void Encode(ushort value, Span<byte> buffer, ref int offset)
    {
        UnsignedIntegerCodec.Encode(value, buffer, ref offset);
    }

    public void Encode(uint value, Span<byte> buffer, ref int offset)
    {
        UnsignedIntegerCodec.Encode(value, buffer, ref offset);
    }

    public void Encode(ulong value, Span<byte> buffer, ref int offset)
    {
        UnsignedIntegerCodec.Encode(value, buffer, ref offset);
    }

    public void Encode(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        bool lengthIsEven = (value.Length % 2) == 0;
        nint length = lengthIsEven ? value.Length / 2 : (value.Length + 1) / 2;
        int stringIndex = 0;

        if (!lengthIsEven)
            buffer[offset++] = (byte) (0x00 | DecodeToByte(value[stringIndex++]));

        for (; offset < (offset + length); stringIndex += 2)
            buffer[offset++] = (byte) ((DecodeToByte(value[stringIndex]) << 4) | DecodeToByte(value[stringIndex + 1]));
    }

    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        bool lengthIsEven = (value.Length % 2) == 0;
        int stringIndex = 0;

        if (!lengthIsEven)
            buffer[offset++] = (byte) (0x00 | DecodeToByte(value[stringIndex++]));

        for (; offset < (offset + length); stringIndex += 2)
            buffer[offset++] = (byte) ((DecodeToByte(value[stringIndex]) << 4) | DecodeToByte(value[stringIndex + 1]));
    }

    public override void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    public override void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Decode To Chars

    /// <summary>
    ///     GetChars
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="byteIndex"></param>
    /// <param name="byteCount"></param>
    /// <param name="chars"></param>
    /// <param name="charIndex"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        if (byteIndex > bytes.Length)
            throw new ArgumentOutOfRangeException();

        if ((byteIndex + byteCount) > bytes.Length)
            throw new ArgumentOutOfRangeException();

        if (charIndex > chars.Length)
            throw new ArgumentOutOfRangeException();

        if ((charIndex + (byteCount * 2)) > (chars.Length - charIndex))
            throw new ArgumentOutOfRangeException();

        Array.ConstrainedCopy(bytes, byteIndex, DecodeToChars(bytes[byteIndex..byteCount]), 0, byteCount);

        return byteCount;
    }

    /// <summary>
    ///     Decodes a sequence of bytes into a Hexadecimal string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length >= Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> owner = SpanOwner<char>.Allocate(value.Length * 2);
            Span<char> buffer = owner.Span;
            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                ToCharsBuffer(value[i], buffer, j);

            return buffer.ToArray();
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length * 2];
            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                ToCharsBuffer(value[i], buffer, j);

            return buffer.ToArray();
        }
    }

    private static void ToCharsBuffer(byte value, Span<char> buffer, int startingIndex = 0)
    {
        uint difference = (((value & 0xF0U) << 4) + (value & 0x0FU)) - 0x8989U;
        uint packedResult = (((uint) -(int) difference & 0x7070U) >> 4) + difference + 0xB9B9U;

        buffer[startingIndex + 1] = (char) (packedResult & 0xFF);
        buffer[startingIndex] = (char) (packedResult >> 8);
    }

    #endregion

    #region Decode To String

    /// <summary>
    ///     Decodes a sequence of bytes into a Hexadecimal string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="PlayEncodingException"></exception>
    public string DecodeToString(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length >= Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> owner = SpanOwner<char>.Allocate(value.Length * 2);
            Span<char> buffer = owner.Span;
            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                ToCharsBuffer(value[i], buffer, j);

            return new string(buffer.ToArray());
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length * 2];
            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                ToCharsBuffer(value[i], buffer, j);

            return new string(buffer.ToArray());
        }
    }

    /// <summary>
    ///     Decodes a sequence of bytes into a Hexadecimal string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public bool TryDecodingToString(ReadOnlySpan<byte> value, out string? result)
    {
        try
        {
            CheckCore.ForEmptySequence(value, nameof(value));

            result = DecodeToString(value);

            return true;
        }
        catch (PlayEncodingException)
        {
            result = null;

            return false;
        }
    }

    #endregion

    #region Decode To Integers

    public BigInteger DecodeToBigInteger(ReadOnlySpan<char> value) => UnsignedIntegerCodec.DecodeToBigInteger(Encode(value));

    /// <summary>
    ///     Returns an unsigned integer from the Hexadecimal string provided
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint DecodeToUInt32(ReadOnlySpan<char> value) => UnsignedIntegerCodec.DecodeToUInt32(Encode(value));

    public ulong DecodeToUInt64(ReadOnlySpan<char> value) => UnsignedIntegerCodec.DecodeToUInt64(Encode(value));

    /// <summary>
    ///     Returns an unsigned integer from the Hexadecimal string provided
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ushort DecodeToUInt16(ReadOnlySpan<char> value) => UnsignedIntegerCodec.DecodeToUInt16(Encode(value));

    /// <summary>
    ///     Encodes a valid Hexadecimal character into its equivalent byte value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    private static byte DecodeToByte(char value)
    {
        if ((value > 0xFF) || (value < 0))
            throw new PlayEncodingException(PlayEncodingException.ByteWasOutOfRangeOfHexadecimalCharacter);

        byte result = Lookup.CharToHex[value];

        return result == 0xFF ? throw new PlayEncodingException(PlayEncodingException.ByteWasOutOfRangeOfHexadecimalCharacter) : result;
    }

    #endregion

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value) => throw new NotImplementedException();

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        ReadOnlySpan<byte> trimmedValue = value.TrimStart((byte) 0);

        if (value.Length == Specs.Integer.UInt8.ByteSize)
        {
            byte byteResult = DecodeToByte(trimmedValue[0]);

            return new DecodedResult<byte>(byteResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt16.ByteSize)
        {
            ushort shortResult = DecodeToUInt16(trimmedValue);

            return new DecodedResult<ushort>(shortResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt32.ByteCount)
        {
            uint intResult = DecodeToUInt32(trimmedValue);

            return new DecodedResult<uint>(intResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt64.ByteCount)
        {
            ulong longResult = DecodeToUInt64(trimmedValue);

            return new DecodedResult<ulong>(longResult, value.Length * 2);
        }

        BigInteger bigIntegerResult = DecodeToBigInteger(trimmedValue);

        return new DecodedResult<BigInteger>(bigIntegerResult, value.Length * 2);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Returns an unsigned integer from the Hexadecimal string provided
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ulong DecodeToUIn64(ReadOnlySpan<char> value) => UnsignedIntegerCodec.DecodeToUInt64(Encode(value));

    #endregion

    private static class Lookup
    {
        #region Instance Values

        public static ReadOnlySpan<char> CharDictionary =>
            new[]
            {
                '0', '1', '2', '3', '4', '5', '6', '7',
                '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
            };

        /// <summary>Map from an ASCII char to its hex value, e.g. arr['B'] == 11. 0xFF means it's not a hex digit.</summary>
        public static ReadOnlySpan<byte> CharToHex =>
            new byte[]
            {
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 15
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 31
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 47
                0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7,
                0x8, 0x9, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 63
                0xFF, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 79
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 95
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 111
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 127
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 143
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 159
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 175
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 191
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 207
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 223
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 239
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF // 255
            };

        #endregion
    }
}