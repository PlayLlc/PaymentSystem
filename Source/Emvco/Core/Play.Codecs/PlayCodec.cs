using System.Text;

namespace Play.Codecs;

/// <summary>
///     A simple codec base class that can be inherited to customize an encoding class specific to the encoding rules of
///     the implementing class
/// </summary>
public abstract class PlayCodec : IGetPlayCodecMetadata, IEncodeStructs, IEncodeStructsToBuffer, IDecodeToMetadata
{
    #region Instance Values

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     An override of the original <see cref="System.Text.Encoding.ASCII" /> that will enforce strict parsing.
    ///     Exceptions will be raised if invalid data is attempted to be parsed
    /// </summary>
    public static StrictAsciiCodec AsciiCodec => new();

    public static UnicodeCodec UnicodeCodec => new();
    public static BinaryCodec BinaryCodec => new();
    public static HexadecimalCodec HexadecimalCodec => new();
    public static AlphabeticCodec AlphabeticCodec => new();
    public static AlphaNumericCodec AlphaNumericCodec => new();
    public static AlphaNumericSpecialCodec AlphaNumericSpecialCodec => new();
    public static CompressedNumericCodec CompressedNumericCodec => new();
    public static NumericCodec NumericCodec => new();
    public static UnsignedIntegerCodec UnsignedIntegerCodec => new();

    //public static SignedIntegerCodec SignedIntegerCodec => new();
    public static AlphaSpecialCodec AlphaSpecialCodec => new();
    public static SignedNumericCodec SignedNumericCodec => new();
    public static NumericSpecialCodec NumericSpecialCodec => new();

    #endregion

    #region Instance Members

    #region Metadata

    public abstract PlayEncodingId GetEncodingId();

    #endregion

    #region Validation

    public abstract bool IsValid(ReadOnlySpan<byte> value);

    #endregion

    #endregion

    #region Serialization

    #region Decode To DecodedMetadata

    // HACK: We need to phase this out from the code base. This can throw runtime errors that can't  be caught before hand, and is a lot less performant than explicitly decoding and encoding the type. This is only syntactic sugar and needs to be removed
    public abstract DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion

    #endregion

    #region Count

    public abstract ushort GetByteCount<_T>(_T value) where _T : struct;
    public abstract ushort GetByteCount<_T>(_T[] value) where _T : struct;

    #endregion

    #region Encode

    public abstract byte[] Encode<_T>(_T value) where _T : struct;
    public abstract byte[] Encode<_T>(_T value, int length) where _T : struct;
    public abstract byte[] Encode<_T>(_T[] value) where _T : struct;
    public abstract byte[] Encode<_T>(_T[] value, int length) where _T : struct;
    public abstract void Encode<_T>(_T value, Span<byte> buffer, ref int offset) where _T : struct;
    public abstract void Encode<_T>(_T value, int length, Span<byte> buffer, ref int offset) where _T : struct;
    public abstract void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset) where _T : struct;
    public abstract void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset) where _T : struct;

    #endregion
}