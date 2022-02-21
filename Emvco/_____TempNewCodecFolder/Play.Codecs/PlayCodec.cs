using Play.Codecs.Metadata;

namespace Play.Codecs;

/// <summary>
///     A simple codec base class that can be inherited to customize an encoding class specific to the encoding rules of
///     the implementing class
/// </summary>
public abstract class PlayCodec : IGetPlayCodecMetadata, IEncodeStructs, IEncodeStructsToBuffer, IDecodeToMetadata
{
    #region Metadata

    public abstract PlayEncodingId GetEncodingId();

    #endregion

    #region Instance Values

    public static UnsignedIntegerCodec UnsignedIntegerCodec => new();
    public static AlphaNumericCodec AlphaNumericCodec => new();

    #endregion

    #region Count

    public abstract ushort GetByteCount<_T>(_T value) where _T : struct;
    public abstract ushort GetByteCount<_T>(_T[] value) where _T : struct;

    #endregion

    #region Validation

    public abstract bool IsValid(ReadOnlySpan<byte> value);

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

    #region Decode To DecodedMetadata

    public abstract DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion
}