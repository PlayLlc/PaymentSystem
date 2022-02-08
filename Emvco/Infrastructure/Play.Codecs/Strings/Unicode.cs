using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Codecs.Strings;

public class Unicode : PlayEncoding
{
    #region Static Metadata

    private static readonly Encoding _UnicodeCodec = Unicode;

    #endregion

    #region Instance Members

    public override int GetByteCount(char[] chars, int index, int count) => _UnicodeCodec.GetByteCount(chars, index, count);

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        _UnicodeCodec.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

    public override int GetCharCount(byte[] bytes, int index, int count) => _UnicodeCodec.GetCharCount(bytes, index, count);

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        _UnicodeCodec.GetChars(bytes, byteIndex, byteCount, chars, charIndex);

    public override int GetMaxByteCount(int charCount) => _UnicodeCodec.GetMaxByteCount(charCount);
    public override int GetMaxCharCount(int byteCount) => _UnicodeCodec.GetMaxCharCount(byteCount);

    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        var byteCount = _UnicodeCodec.GetByteCount(value);

        if (byteCount > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteCount);
            var buffer = spanOwner.Span;
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

    public override int GetBytes(ReadOnlySpan<char> value, Span<byte> buffer) => _UnicodeCodec.GetBytes(value, buffer);
    public override string GetString(ReadOnlySpan<byte> value) => _UnicodeCodec.GetString(value);
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

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        result = GetBytes(value);

        return true;
    }

    public override bool TryGetString(ReadOnlySpan<byte> value, out string result)
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