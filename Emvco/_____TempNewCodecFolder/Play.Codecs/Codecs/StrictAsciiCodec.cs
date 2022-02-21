using System.Collections.Immutable;
using System.Text;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class StrictAsciiCodec
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = new(typeof(StrictAsciiCodec));
    private static readonly Encoding _ErrorDetectingEncoder = GetEncoder();

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMappers =
        Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMapper =
        Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

    private const byte _MinByteDecimal = 32;
    private const byte _MaxByteDecimal = 126;

    #endregion

    #region Instance Members

    public PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    public bool IsValid(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        foreach (char byteValue in value)
        {
            if ((byteValue < _MinByteDecimal) || (byteValue > _MaxByteDecimal))
                return false;
        }

        return true;
    }

    public bool IsValid(ReadOnlySpan<byte> value)
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

    public int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        _ErrorDetectingEncoder.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

    public byte[] GetBytes(string value) => GetBytes(value.AsSpan());

    /// <exception cref="EncodingException"></exception>
    public byte[] GetBytes(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length >= Specs.ByteArray.StackAllocateCeiling)
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

    public bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
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

    public int GetByteCount(char[] chars, int index, int count) => _ErrorDetectingEncoder.GetByteCount(chars, index, count);
    public int GetMaxByteCount(int charCount) => charCount;

    public int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
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

    public int GetCharCount(byte[] bytes, int index, int count) => _ErrorDetectingEncoder.GetCharCount(bytes, index, count);
    public int GetMaxCharCount(int byteCount) => byteCount;

    /// <exception cref="EncodingException"></exception>
    public string GetString(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length == 0)
            throw new PlayEncodingException(PlayEncodingException.ByteArrayWasEmpty);

        return _ErrorDetectingEncoder.GetString(value);
    }

    public bool TryGetString(ReadOnlySpan<byte> value, out string result)
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

    private static Encoding GetEncoder() =>
        Encoding.GetEncoding("us-ascii", new EncoderExceptionFallback(), new DecoderExceptionFallback());

    #endregion
}