using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Codecs.Strings;

namespace Play.Emv.Codecs;

public class NumericSpecialEmvCodec : IPlayCodec
{
    #region Static Metadata

    private static readonly NumericSpecial _Codec = PlayEncoding.NumericSpecial;

    #endregion

    #region Instance Members

    public ushort GetByteCount<T>(T value) where T : struct => throw new NotImplementedException();
    public ushort GetByteCount<T>(T[] value) where T : struct => throw new NotImplementedException();
    public bool IsValid(ReadOnlySpan<byte> value) => throw new NotImplementedException();
    public byte[] Encode<T>(T value) where T : struct => throw new NotImplementedException();
    public byte[] Encode<T>(T value, int length) where T : struct => throw new NotImplementedException();
    public byte[] Encode<T>(T[] value) where T : struct => throw new NotImplementedException();
    public byte[] Encode<T>(T[] value, int length) where T : struct => throw new NotImplementedException();

    public void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Serialization

    public DecodedMetadata Decode(ReadOnlySpan<byte> value) => throw new NotImplementedException();

    #endregion
}