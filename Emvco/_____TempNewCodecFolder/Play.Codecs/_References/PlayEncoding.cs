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
    public new static StrictAscii ASCII => new();

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     An override of the original <see cref="System.Text.Encoding.UTF8" /> that will enforce strict parsing.
    ///     Exceptions will be raised if invalid data is attempted to be parsed
    /// </summary>
    public new static StrictUtf8 UTF8 => new();

    public new static Unicode Unicode => new();
    public static Binary Binary => new();
    public static Hexadecimal Hexadecimal => new();
    public static Alphabetic Alphabetic => new();
    public static AlphaNumeric AlphaNumeric => new();
    public static AlphaNumericSpecial AlphaNumericSpecial => new();
    public static CompressedNumeric CompressedNumeric => new();
    public static Numeric Numeric => new();
    public static UnsignedInteger UnsignedBinary => new();
    public static UnsignedInteger UnsignedInteger => new();
    public static SignedInteger SignedInteger => new();
    public static AlphaSpecial AlphaSpecial => new();
    public static SignedNumeric SignedNumeric => new();
    public static NumericSpecial NumericSpecial => new();

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