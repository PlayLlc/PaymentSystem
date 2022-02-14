using System;
using System.Buffers;
using System.Globalization;
using System.Text;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Exceptions;

namespace Play.Codecs.Strings;

public class StrictUtf8 : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = new(nameof(StrictUtf8));
    private static readonly UTF8Encoding _ErrorDetectingEncoder = new(false, true);

    #endregion

    #region Instance Members

    public PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    /// <summary>
    ///     This is validating that the sequence of characters are in the Basic Multilingual Plane
    ///     and are not in the Unicode Surrogate range
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        foreach (Rune rune in value.EnumerateRunes())
        {
            if (!rune.IsBmp)
                return false;
            if (Rune.GetUnicodeCategory(rune) == UnicodeCategory.Surrogate)
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        int byteCount = 0;

        while (Rune.DecodeFromUtf8(value, out Rune rune, out int bytesConsumed) == OperationStatus.Done)
        {
            if (Rune.GetUnicodeCategory(rune) == UnicodeCategory.Surrogate)
                return false;
            if (!rune.IsBmp)
                return false;

            byteCount += bytesConsumed;
            value = value[bytesConsumed..];
        }

        return byteCount == value.Length;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        _ErrorDetectingEncoder.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncoderFallbackException"></exception>
    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length >= StackallocThreshold)
        {
            using SpanOwner<byte> owner = SpanOwner<byte>.Allocate(_ErrorDetectingEncoder.GetByteCount(value));
            Span<byte> buffer = owner.Span;
            _ErrorDetectingEncoder.GetBytes(value, buffer);

            return buffer.ToArray();
        }
        else
        {
            Span<byte> buffer = stackalloc byte[_ErrorDetectingEncoder.GetByteCount(value)];
            _ErrorDetectingEncoder.GetBytes(value, buffer);

            return buffer.ToArray();
        }
    }

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        try
        {
            CheckCore.ForEmptySequence(value, nameof(value));

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
    public override int GetMaxByteCount(int charCount) => _ErrorDetectingEncoder.GetMaxByteCount(charCount);

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        _ErrorDetectingEncoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex);

    public override int GetCharCount(byte[] bytes, int index, int count) => _ErrorDetectingEncoder.GetCharCount(bytes, index, count);
    public override int GetMaxCharCount(int byteCount) => _ErrorDetectingEncoder.GetMaxCharCount(byteCount);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncoderFallbackException"></exception>
    public override string GetString(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length >= StackallocThreshold)
        {
            using SpanOwner<char> owner = SpanOwner<char>.Allocate(value.Length);
            Span<char> buffer = owner.Span;
            _ErrorDetectingEncoder.GetChars(value, buffer);

            return new string(buffer);
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length];
            _ErrorDetectingEncoder.GetChars(value, buffer);

            return new string(buffer);
        }
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

    #endregion
}