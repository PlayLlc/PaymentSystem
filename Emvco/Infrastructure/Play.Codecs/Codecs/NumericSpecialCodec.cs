using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class NumericSpecialCodec : PlayCodec
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = new(typeof(NumericSpecialCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap = Enumerable.Range(0, 10).Concat(Enumerable.Range(32, 47 - 32))
        .Concat(Enumerable.Range(58, 64 - 58)).Concat(Enumerable.Range(91, 96 - 91)).Concat(Enumerable.Range(123, 126 - 123))
        .ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap = Enumerable.Range(0, 10).Concat(Enumerable.Range(32, 47 - 32))
        .Concat(Enumerable.Range(58, 64 - 58)).Concat(Enumerable.Range(91, 96 - 91)).Concat(Enumerable.Range(123, 126 - 123))
        .ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;

    public bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!_ByteMap.Keys.Contains(value[i]))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!_CharMap.Keys.Contains(value[i]))
                return false;
        }

        return true;
    }

    public override ushort GetByteCount<_T>(_T value) => (ushort) Unsafe.SizeOf<_T>();

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InternalPlayEncodingException"></exception>
    public override ushort GetByteCount<_T>(_T[] value)
    {
        Type type = typeof(_T);

        if (type.IsByte())
            return (ushort) value.Length;
        if (type.IsChar())
            return (ushort) value.Length;

        throw new InternalPlayEncodingException(this, type);
    }

    /// <exception cref="InternalPlayEncodingException"></exception>
    public override byte[] Encode<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode<char>(Unsafe.As<T[], char[]>(ref value));

        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <exception cref="InternalPlayEncodingException"></exception>
    public override byte[] Encode<T>(T[] value, int length) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value), length);

        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InternalPlayEncodingException"></exception>
    public override byte[] Encode<T>(T value) where T : struct =>
        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="InternalPlayEncodingException"></exception>
    public override byte[] Encode<T>(T value, int length) where T : struct =>
        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="InternalPlayEncodingException"></exception>
    public override void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="InternalPlayEncodingException"></exception>
    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="InternalPlayEncodingException"></exception>
    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode<char>(Unsafe.As<T[], char[]>(ref value), buffer, ref offset);
        else
        {
            throw new InternalPlayEncodingException(
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
    /// <exception cref="InternalPlayEncodingException"></exception>
    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value), length, buffer, ref offset);
        else
        {
            throw new InternalPlayEncodingException(
                $"The {nameof(AlphaNumericSpecialCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public bool IsValid(char value)
    {
        if (_ByteMap.Keys.Contains(value))
            return true;

        return false;
    }

    public byte[] Encode(string value)
    {
        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[value.Length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
    }

    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = Encode(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    public byte[] Encode(ReadOnlySpan<char> value) => Encode(value, value.Length);

    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == length)
            return Encode(value);

        if (value.Length > length)
            return Encode(value)[^length..];

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> tempBuffer = stackalloc char[value.Length];
            value.CopyTo(tempBuffer);

            return Encode(tempBuffer);
        }
        else
        {
            using SpanOwner<char> spanCharOwner = SpanOwner<char>.Allocate(length);
            Span<char> tempBuffer = spanCharOwner.Span;
            value.CopyTo(tempBuffer);

            return Encode(tempBuffer);
        }
    }

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

    public int GetByteCount(char[] chars, int index, int count) => count;
    public int GetMaxByteCount(int charCount) => charCount;

    public int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                buffer[i++] = _CharMap[(byte) value[i]];

            return buffer.ToArray();
        }
        else
        {
            SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                buffer[i++] = _CharMap[(byte) value[i]];

            return buffer.ToArray();
        }
    }

    public int GetCharCount(byte[] bytes, int index, int count) => GetMaxCharCount(count);
    public int GetMaxCharCount(int byteCount) => byteCount * 2;
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

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        char[] valueResult = DecodeToChars(value);

        return new DecodedResult<char[]>(valueResult, valueResult.Length);
    }

    #endregion
}