using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class AlphabeticCodec : PlayCodec
{
    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(AlphabeticCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMapper = Enumerable.Range(65, (90 - 65) + 1).Concat(Enumerable.Range(97, (122 - 97) + 1))
        .ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMapper = Enumerable.Range(65, (90 - 65) + 1).Concat(Enumerable.Range(97, (122 - 97) + 1))
        .ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

    #endregion

    #region Count

    public int GetByteCount(ReadOnlySpan<char> value) => value.Length;
    public override ushort GetByteCount<T>(T value) where T : struct => throw new NotImplementedException();

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override ushort GetByteCount<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new CodecParsingException("This exception should never be reached");
    }

    public int GetCharCount(ReadOnlySpan<char> value) => value.Length;
    public int GetCharCount(ReadOnlySpan<byte> value) => value.Length;
    public int GetMaxCharCount(int byteCount) => byteCount * 2;

    #endregion

    #region Validation

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public bool IsValid(byte value)
    {
        const byte bigA = (byte) 'A';
        const byte littleA = (byte) 'a';
        const byte bigZ = (byte) 'Z';
        const byte littleZ = (byte) 'z';

        return value is >= bigA and <= bigZ or >= littleA and <= littleZ;
    }

    public bool IsValid(char value)
    {
        const char bigA = 'A';
        const char littleA = 'a';
        const char bigZ = 'Z';
        const char littleZ = 'z';

        return value is >= bigA and <= bigZ || value is >= littleA and <= littleZ;
    }

    #endregion

    #region Encode

    /// <exception cref="CodecParsingException">Ignore.</exception>
    public bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        if (IsValid(value))
        {
            result = Encode(value);

            return true;
        }

        result = Array.Empty<byte>();

        return false;
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T value) where T : struct => throw new NotImplementedException();

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public override byte[] Encode<T>(T value, int length) where T : struct => throw new NotImplementedException();

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new NotImplementedException();
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override byte[] Encode<T>(T[] value, int length) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new NotImplementedException();
    }

    public byte[] Encode(string value)
    {
        if (value == string.Empty)
            return Array.Empty<byte>();

        return Encode(value.AsSpan());
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

        byte[] byteArray = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
        {
            if (!_ByteMapper.ContainsKey(value[i]))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);

            byteArray[i] = _ByteMapper[value[i]];
        }

        return byteArray;
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        Validate(value);

        if (length > value.Length)
        {
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(AlphabeticCodec)} could not {nameof(Encode)} the value because the length was out of range"));
        }

        byte[] byteArray = new byte[length];

        for (int i = 0; i < length; i++)
        {
            if (!_ByteMapper.ContainsKey(value[i]))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);

            byteArray[i] = _ByteMapper[value[i]];
        }

        return byteArray;
    }

    public byte[] Encode(ReadOnlySpan<Nibble> value)
    {
        if ((value.Length % 2) != 0)
        {
            throw new CodecParsingException(
                $"the {nameof(AlphaNumericSpecialCodec)} could not {nameof(Encode)} the argument because the value was not a multiple of 2");
        }

        return value.AsByteArray();
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public override void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), buffer, ref offset);

        throw new CodecParsingException(this, typeof(T));
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length, buffer, ref offset);
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        Validate(value);

        if (length > value.Length)
            throw new InvalidOperationException();

        for (int i = 0; i < length; i++)
        {
            if (!_ByteMapper.ContainsKey(value[i]))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);

            buffer[offset++] = _ByteMapper[value[i]];
        }
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

        for (int i = 0; i < value.Length; i++)
        {
            if (!_ByteMapper.ContainsKey(value[i]))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);

            buffer[offset++] = _ByteMapper[value[i]];
        }
    }

    #endregion

    #region Decode To Chars

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        char[] result = new char[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = _CharMapper[value[i]];

        return result;
    }

    /// <summary>
    ///     GetChar
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public char DecodeToChar(byte value)
    {
        Validate(value);

        return _CharMapper[value];
    }

    #endregion

    #region Decode To String

    /// <exception cref="CodecParsingException"></exception>
    public string DecodeToString(ReadOnlySpan<byte> value)
    {
        Validate(value);

        if (value.Length >= Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> owner = SpanOwner<char>.Allocate(value.Length);
            Span<char> buffer = owner.Span;

            for (int i = 0; i <= (value.Length - 1); i++)
                buffer[i] = _CharMapper[value[i]];

            return new string(buffer);
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length];
            for (int i = 0; i <= (value.Length - 1); i++)
                buffer[i] = _CharMapper[value[i]];

            return new string(buffer);
        }
    }

    /// <exception cref="CodecParsingException">Ignore.</exception>
    public bool TryDecodingToString(ReadOnlySpan<byte> value, out string result)
    {
        result = string.Empty;

        if (IsValid(value))
        {
            result = DecodeToString(value);

            return true;
        }

        return false;
    }

    #endregion

    #region Decode To Integers

    /// <summary>
    ///     GetByte
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public byte DecodeToByte(char value)
    {
        Validate(value); //

        return (byte) value;
    }

    #endregion

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value) => new DecodedResult<char[]>(AlphabeticCodec.DecodeToChars(value), value.Length);

    #endregion

    #region Instance Members

    /// <summary>
    ///     Validate
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="CodecParsingException"></exception>
    private void Validate(byte value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
    }

    /// <exception cref="CodecParsingException"></exception>
    private void Validate(char value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
    }

    /// <exception cref="CodecParsingException"></exception>
    protected void Validate(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        for (int i = 0; i <= (value.Length - 1); i++)
        {
            if (!IsValid(value[i]))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
        }
    }

    /// <exception cref="CodecParsingException"></exception>
    private void Validate(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        for (int i = 0; i <= (value.Length - 1); i++)
        {
            if (!IsValid(value[i]))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
        }
    }

    #endregion
}