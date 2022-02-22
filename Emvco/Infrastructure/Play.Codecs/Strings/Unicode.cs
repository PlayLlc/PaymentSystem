﻿using System;
using System.Text;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Codecs.Strings;

public class UnicodeCodec : PlayEncoding
{
    #region Static Metadata

    private static readonly Encoding _UnicodeCodec = UnicodeCodec;
    public static readonly PlayEncodingId EncodingId = new(typeof(UnicodeCodec));

    #endregion

    #region Instance Members

    public override int GetByteCount(char[] chars, int index, int count) => _UnicodeCodec.GetByteCount(chars, index, count);

    public override int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        _UnicodeCodec.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

    public override int GetCharCount(byte[] bytes, int index, int count) => _UnicodeCodec.GetCharCount(bytes, index, count);

    public override int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        _UnicodeCodec.GetChars(bytes, byteIndex, byteCount, chars, charIndex);

    public override int GetMaxByteCount(int charCount) => _UnicodeCodec.GetMaxByteCount(charCount);
    public override int GetMaxCharCount(int byteCount) => _UnicodeCodec.GetMaxCharCount(byteCount);

    public override byte[] Encode(ReadOnlySpan<char> value)
    {
        int byteCount = _UnicodeCodec.GetByteCount(value);

        if (byteCount > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteCount);
            Span<byte> buffer = spanOwner.Span;
            _UnicodeCodec.GetBytes(value, buffer);

            return buffer.ToArray();
        }
        else
        {
            Span<byte> buffer = stackalloc byte[byteCount];
            _UnicodeCodec.GetBytes(value, buffer);

            return buffer.ToArray();
        }
    }

    public override int Encode(ReadOnlySpan<char> value, Span<byte> buffer) => _UnicodeCodec.GetBytes(value, buffer);
    public override string DecodeToString(ReadOnlySpan<byte> value) => _UnicodeCodec.GetString(value);
    public override bool IsValid(ReadOnlySpan<char> value) => true;
    private bool IsValid(byte value) => Rune.IsValid(value);

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!Rune.IsValid(value[i]))
                return false;
        }

        return true;
    }

    public override bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        result = Encode(value);

        return true;
    }

    public override bool TryDecodingToString(ReadOnlySpan<byte> value, out string result)
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