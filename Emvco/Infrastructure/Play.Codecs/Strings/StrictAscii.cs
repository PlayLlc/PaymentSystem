using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Exceptions;

namespace Play.Codecs.Strings;

public class StrictAscii : PlayEncoding
{
    #region Static Metadata

    public static string Name = nameof(StrictAscii);
    private static readonly Encoding _ErrorDetectingEncoder = GetEncoder();

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMappers =
        Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMapper =
        Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

    private const byte _MinByteDecimal = 32;
    private const byte _MaxByteDecimal = 126;

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        foreach (char byteValue in value)
        {
            if ((byteValue < _MinByteDecimal) || (byteValue > _MaxByteDecimal))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        foreach (byte byteValue in value)
        {
            if ((byteValue < _MinByteDecimal) || (byteValue > _MaxByteDecimal))
                return false;
        }

        return true;
    }

    private void Validate(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new ArgumentOutOfRangeException();
    }

    private void Validate(ReadOnlySpan<char> value)
    {
        if (!IsValid(value))
            throw new ArgumentOutOfRangeException();
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        _ErrorDetectingEncoder.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

    public override byte[] GetBytes(string value) => GetBytes(value.AsSpan());

    /// <exception cref="EncodingException"></exception>
    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length >= StackallocThreshold)
        {
            using SpanOwner<byte> owner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = owner.Span;

            _ErrorDetectingEncoder.GetBytes(value, buffer);

            return buffer.ToArray();
        }
        else
        {
            Span<byte> buffer = stackalloc byte[value.Length];
            _ErrorDetectingEncoder.GetBytes(value, buffer);

            return buffer.ToArray();
        }
    }

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        try
        {
            result = GetBytes(value);

            return true;
        }
        catch (Exception)
        {
            result = null;

            return false;
        }
    }

    public override int GetByteCount(char[] chars, int index, int count) => _ErrorDetectingEncoder.GetByteCount(chars, index, count);
    public override int GetMaxByteCount(int charCount) => charCount;

    public sealed override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        _ErrorDetectingEncoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex);

    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        Validate(value);

        char[] result = new char[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = _CharMapper[value[i]];

        return result;
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => _ErrorDetectingEncoder.GetCharCount(bytes, index, count);
    public override int GetMaxCharCount(int byteCount) => byteCount;

    /// <exception cref="EncodingException"></exception>
    public override string GetString(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length == 0)
            throw new EncodingException(EncodingException.ByteArrayWasEmpty);

        return _ErrorDetectingEncoder.GetString(value);
    }

    public override bool TryGetString(ReadOnlySpan<byte> value, out string result)
    {
        try
        {
            CheckCore.ForEmptySequence(value, nameof(value));

            result = GetString(value);

            return true;
        }
        catch (Exception)
        {
            result = null;

            return false;
        }
    }

    private static Encoding GetEncoder() => GetEncoding("us-ascii", new EncoderExceptionFallback(), new DecoderExceptionFallback());

    #endregion
}