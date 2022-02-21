using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using ___Temp.CodecMergedShit.Metadata;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Specifications;
using Play.Emv.Codecs;
using Play.Emv.Codecs.Exceptions;

namespace ___Temp.CodecMergedShit.Codecs.Strings;

public class AlphaNumericSpecialCodec : PlayCodec
{
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

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value));

        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value, int length) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value), length);

        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    public override byte[] Encode<T>(T value) where T : struct =>
        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    public override byte[] Encode<T>(T value, int length) where T : struct =>
        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new InternalPlayEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value), buffer, ref offset);
        else
        {
            throw new InternalPlayEncodingException(
                $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value), length, buffer, ref offset);
        else
        {
            throw new InternalPlayEncodingException(
                $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    #endregion

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        char[] valueResult = GetChars(value);

        return new DecodedResult<char[]>(valueResult, valueResult.Length);
    }

    #endregion

    #region Instance Members

    public int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        GetBytes(chars[charIndex..(charIndex + charCount)]).AsSpan().CopyTo(bytes[byteIndex..]);

        return charCount;
    }

    public byte[] GetBytes(ReadOnlySpan<char> value)
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

    public void GetBytes(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset)
    {
        for (int i = 0; i < value.Length; i++)
            buffer[offset++] = _ByteMap[value[i]];
    }

    public bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = GetBytes(value);

        return true;
    }

    public int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        GetChars(bytes[byteIndex..(byteIndex + byteCount)]).AsSpan().CopyTo(chars[charIndex..]);

        return byteCount;
    }

    public char[] GetChars(ReadOnlySpan<byte> value)
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

    public string GetString(ReadOnlySpan<byte> value) => new(GetChars(value));

    public bool TryGetString(ReadOnlySpan<byte> value, out string result)
    {
        if (!IsValid(value))
        {
            result = string.Empty;

            return false;
        }

        result = GetString(value);

        return true;
    }

    #endregion
}