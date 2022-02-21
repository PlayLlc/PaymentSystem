using System.Text;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Codecs;

public class UnicodeCodec
{
    #region Static Metadata

    private static readonly Encoding _UnicodeCodec = Encoding.Unicode;
    public static readonly PlayEncodingId PlayEncodingId = new(nameof(UnicodeCodec));

    #endregion

    #region Instance Members

    public PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    public int GetByteCount(char[] chars, int index, int count) => _UnicodeCodec.GetByteCount(chars, index, count);

    public int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        _UnicodeCodec.GetBytes(chars, charIndex, charCount, bytes, byteIndex);

    public int GetCharCount(byte[] bytes, int index, int count) => _UnicodeCodec.GetCharCount(bytes, index, count);

    public int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        _UnicodeCodec.GetChars(bytes, byteIndex, byteCount, chars, charIndex);

    public int GetMaxByteCount(int charCount) => _UnicodeCodec.GetMaxByteCount(charCount);
    public int GetMaxCharCount(int byteCount) => _UnicodeCodec.GetMaxCharCount(byteCount);

    public byte[] GetBytes(ReadOnlySpan<char> value)
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

    public int GetBytes(ReadOnlySpan<char> value, Span<byte> buffer) => _UnicodeCodec.GetBytes(value, buffer);
    public string GetString(ReadOnlySpan<byte> value) => _UnicodeCodec.GetString(value);
    public bool IsValid(ReadOnlySpan<char> value) => true;
    private bool IsValid(byte value) => Rune.IsValid(value);

    public bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!Rune.IsValid(value[i]))
                return false;
        }

        return true;
    }

    public bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        result = GetBytes(value);

        return true;
    }

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