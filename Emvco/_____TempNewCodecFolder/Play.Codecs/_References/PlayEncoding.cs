using System.Text;

namespace Play.Codecs._References;

public abstract class PlayEncoding : Encoding
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
    public new static StrictAsciiCodec ASCII => new();

    public new static UnicodeCodec Unicode => new();
    public static BinaryCodec Binary => new();
    public static HexadecimalCodec Hexadecimal => new();
    public static AlphabeticCodec Alphabetic => new();
    public static AlphaNumericCodec AlphaNumeric => new();
    public static AlphaNumericSpecialCodec AlphaNumericSpecial => new();
    public static CompressedNumericCodec CompressedNumeric => new();
    public static NumericCodec Numeric => new();
    public static UnsignedIntegerCodec UnsignedBinary => new();
    public static UnsignedIntegerCodec UnsignedInteger => new();
    public static SignedIntegerCodec SignedInteger => new();
    public static AlphaSpecialCodec AlphaSpecial => new();
    public static SignedNumericCodec SignedNumeric => new();
    public static NumericSpecialCodec NumericSpecial => new();

    #endregion

    #region Instance Members

    public abstract byte[] GetBytes(ReadOnlySpan<char> value);
    public new abstract string GetString(ReadOnlySpan<byte> value);
    public abstract bool IsValid(ReadOnlySpan<char> value);
    public abstract bool IsValid(ReadOnlySpan<byte> value);
    public abstract bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result);
    public abstract bool TryGetString(ReadOnlySpan<byte> value, out string result);

    #endregion
}

public class Test : Encoding
{
    #region Instance Members

    public override int GetByteCount(char[] chars, int index, int count) => throw new NotImplementedException();

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        throw new NotImplementedException();

    public override int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public override int GetMaxByteCount(int charCount) => throw new NotImplementedException();
    public override int GetMaxCharCount(int byteCount) => throw new NotImplementedException();

    #endregion
}

public interface IDotNetEncoding
{
    public int GetByteCount(char[] chars, int index, int count);
    public int GetMaxByteCount(int charCount);
    public int GetMaxCharCount(int byteCount);
    public int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);
    public int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);
    public int GetCharCount(byte[] bytes, int index, int count);
}