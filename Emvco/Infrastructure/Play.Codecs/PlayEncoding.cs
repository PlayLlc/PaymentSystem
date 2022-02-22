using System;

using Play.Codecs.Integers;
using Play.Codecs.Strings;

namespace Play.Codecs;

public abstract class PlayCodec
{
    #region Static Metadata

    protected const int StackallocThreshold = 256;

    #endregion

    #region Instance Values

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     An override of the original <see cref="System.Text.Encoding.ASCII" /> that will enforce strict parsing.
    ///     Exceptions will be raised if invalid data is attempted to be parsed
    /// </summary>
    public static StrictAsciiCodec AsciiCodec => new();

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     An override of the original <see cref="System.Text.Encoding.UTF8" /> that will enforce strict parsing.
    ///     Exceptions will be raised if invalid data is attempted to be parsed
    /// </summary>
    public static StrictUtf8Codec Utf8Codec => new();

    public static UnicodeCodec UnicodeCodec => new();
    public static StrictAsciiCodec StrictAsciiCodec => new();
    public static BinaryCodec BinaryCodec => new();
    public static HexadecimalCodec HexadecimalCodec => new();
    public static AlphabeticCodec AlphabeticCodec => new();
    public static AlphaNumericCodec AlphaNumericCodec => new();
    public static AlphaNumericSpecialCodec AlphaNumericSpecialCodec => new();
    public static CompressedNumericCodec CompressedNumericCodec => new();
    public static NumericCodec NumericCodec => new();
    public static UnsignedIntegerCodec UnsignedBinaryCodec => new();
    public static UnsignedIntegerCodec UnsignedIntegerCodec => new();
    public static SignedIntegerCodec SignedIntegerCodec => new();

    #endregion

    #region Instance Members

    public abstract byte[] Encode(ReadOnlySpan<char> value);
    public abstract int GetByteCount(char[] chars, int index, int count);
    public abstract int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);
    public abstract int GetCharCount(byte[] bytes, int index, int count);
    public abstract int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);
    public abstract int GetMaxByteCount(int charCount);
    public abstract int GetMaxCharCount(int byteCount);
    public abstract string DecodeToString(ReadOnlySpan<byte> value);
    public abstract bool IsValid(ReadOnlySpan<char> value);
    public abstract bool IsValid(ReadOnlySpan<byte> value);
    public abstract bool TryEncoding(ReadOnlySpan<char> value, out byte[] result);
    public abstract bool TryDecodingToString(ReadOnlySpan<byte> value, out string result);

    #endregion
}