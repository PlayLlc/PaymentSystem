using System;

using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Ber.DataObjects;

public record Empty : PrimitiveValue 
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = HexadecimalCodec.EncodingId;
    private readonly Tag _Tag;

    #endregion

    #region Instance Members

    public Empty(Tag tag)
    {
        _Tag = tag;

    } 
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => _Tag;
    public override ushort GetValueByteCount(BerCodec codec) => 0;

    #endregion

    #region Serialization

    public override byte[] EncodeValue(BerCodec codec) => Array.Empty<byte>();
    public override byte[] EncodeValue(BerCodec codec, int length) => Array.Empty<byte>();
    public new byte[] EncodeTagLengthValue(BerCodec codec, int length)
    {
        return EncodeTagLengthValue();
    }

    private byte[] EncodeTagLengthValue()
    {
        byte[] result = new byte[2 + _Tag.GetByteCount()];

        _Tag.Serialize().AsSpan().CopyTo(result);

        result[^2] = 0;
        result[^3] = 0;

        return result;
    }
    public new byte[] EncodeTagLengthValue(BerCodec codec)
    {
        return EncodeTagLengthValue();
    }

    #endregion

    #region Equality

    public bool Equals(Null? x, Null? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(Null obj) => obj.GetHashCode();

    #endregion
}