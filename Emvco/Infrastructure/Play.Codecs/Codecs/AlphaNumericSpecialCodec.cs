using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Specifications;



namespace Play.Codecs;

public class AlphaNumericSpecialCodec : PlayCodec
{
    #region Serialization

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        char[] valueResult = DecodeToChars(value);

        return new DecodedResult<char[]>(valueResult, valueResult.Length);
    }

    #endregion

    #endregion

    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(AlphaNumericSpecialCodec));

    // 32 - 126
    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

    #endregion

    #region Count

    public int GetByteCount(ReadOnlySpan<char> value) => value.Length;
    public int GetByteCount(ReadOnlySpan<byte> value) => value.Length;
    public int GetByteCount(char[] chars, int index, int count) => count;
    public int GetMaxByteCount(int charCount) => charCount;
    public override ushort GetByteCount<T>(T value) where T : struct => throw new NotImplementedException();

    public override ushort GetByteCount<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new NotImplementedException();
    }

    public int GetCharCount(byte[] bytes, int index, int count) => count;
    public int GetMaxCharCount(int byteCount) => byteCount;

    #endregion

    #region Validation

    public bool IsValid(ReadOnlySpan<char> value)
    {
        foreach (char character in value)
        {
            if (!_ByteMap.ContainsKey(character))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        foreach (byte character in value)
        {
            if (!_CharMap.ContainsKey(character))
                return false;
        }

        return true;
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

        result = Encode(value);

        return true;
    }

    public byte[] Encode(ReadOnlySpan<char> value)
    {
        if (value.Length > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            Span<byte> buffer = stackalloc byte[value.Length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
    }

    /// <exception cref="Exceptions._Temp.CodecParsingException"></exception>
    public override byte[] Encode<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode<char>(Unsafe.As<T[], char[]>(ref value));

        throw new CodecParsingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T[] value, int length) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value), length);

        throw new CodecParsingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T value) where T : struct =>
        throw new CodecParsingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T value, int length) where T : struct =>
        throw new CodecParsingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new CodecParsingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
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
        throw new CodecParsingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
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
        if (typeof(T) == typeof(char))
            Encode<char>(Unsafe.As<T[], char[]>(ref value), buffer, ref offset);
        else
        {
            throw new CodecParsingException(
                $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
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
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value), length, buffer, ref offset);
        else
        {
            throw new CodecParsingException(
                $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        Encode(chars[charIndex..(charIndex + charCount)]).AsSpan().CopyTo(bytes[byteIndex..]);

        return charCount;
    }

    public void Encode(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset)
    {
        for (int i = 0; i < value.Length; i++)
            buffer[offset++] = _ByteMap[value[i]];
    }

    #endregion

    #region Decode To Chars

    public int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        DecodeToChars(bytes[byteIndex..(byteIndex + byteCount)]).AsSpan().CopyTo(chars[charIndex..]);

        return byteCount;
    }

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        if (value.Length > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(value.Length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _CharMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length];

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
}