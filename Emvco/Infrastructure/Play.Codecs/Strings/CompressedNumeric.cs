using System;

using Play.Core.Extensions;

namespace Play.Codecs.Strings;

// TODO: need to move Play.Codec.CompressedNumeric logic into here
public class CompressedNumeric : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = new(nameof(CompressedNumeric));
    private const byte _PadValue = 0xF;

    #endregion

    #region Instance Members

    public PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    public override bool IsValid(ReadOnlySpan<char> value) => throw new NotImplementedException();
    public override bool IsValid(ReadOnlySpan<byte> value) => throw new NotImplementedException();

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        throw new NotImplementedException();

    public override byte[] GetBytes(ReadOnlySpan<char> value) => throw new NotImplementedException();
    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result) => throw new NotImplementedException();
    public override int GetByteCount(char[] chars, int index, int count) => throw new NotImplementedException();
    public override int GetMaxByteCount(int charCount) => throw new NotImplementedException();

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public override int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();
    public override int GetMaxCharCount(int byteCount) => throw new NotImplementedException();
    public override string GetString(ReadOnlySpan<byte> value) => throw new NotImplementedException();
    public override bool TryGetString(ReadOnlySpan<byte> value, out string result) => throw new NotImplementedException();

    public int GetNumberOfDigits(byte[] value)
    {
        int padCount = 0;

        for (int i = value.Length; i > 0; i--)
        {
            if (value[i].GetRightNibble() != _PadValue)
                break;

            padCount++;

            if (value[i].GetRightNibble() != _PadValue)
                break;

            padCount++;
        }

        return (value.Length * 2) - padCount;
    }

    #endregion
}