using System;
using System.Text;

using Play.Codecs.Integers;
using Play.Codecs.Strings;

namespace Play.Codecs;

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

    public static StrictAscii StrictAscii => new();
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