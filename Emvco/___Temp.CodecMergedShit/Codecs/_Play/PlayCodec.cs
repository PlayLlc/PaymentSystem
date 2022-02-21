using ___Temp.CodecMergedShit.Metadata;

namespace ___Temp.CodecMergedShit;

/// <summary>
///     A simple codec base class that can be inherited to customize an encoding class specific to the encoding rules of
///     the implementing class
/// </summary>
public abstract class PlayCodec : IGetPlayCodecMetadata, IEncodeStructs, IEncodeStructsToBuffer, IDecodeToMetadata
{
    #region Instance Members

    public abstract PlayEncodingId GetEncodingId();
    public abstract ushort GetByteCount<_T>(_T value) where _T : struct;
    public abstract ushort GetByteCount<_T>(_T[] value) where _T : struct;
    public abstract void Encode<_T>(_T value, Span<byte> buffer, ref int offset) where _T : struct;
    public abstract void Encode<_T>(_T value, int length, Span<byte> buffer, ref int offset) where _T : struct;
    public abstract void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset) where _T : struct;
    public abstract void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset) where _T : struct;
    public abstract byte[] Encode<_T>(_T value) where _T : struct;
    public abstract byte[] Encode<_T>(_T value, int length) where _T : struct;
    public abstract byte[] Encode<_T>(_T[] value) where _T : struct;
    public abstract byte[] Encode<_T>(_T[] value, int length) where _T : struct;
    public abstract bool IsValid(ReadOnlySpan<byte> value);

    #endregion

    #region Serialization

    public abstract DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion
}