using System;

using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Ber.DataObjects;

public record Empty : PrimitiveValue
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = HexadecimalCodec.EncodingId;

    #endregion

    #region Instance Values

    private readonly Tag _Tag;

    #endregion

    #region Constructor

    public Empty(Tag tag)
    {
        _Tag = tag;
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => _Tag;
    public override ushort GetValueByteCount(BerCodec codec) => 0;

    #endregion

    #region Serialization

    public override Empty Decode(TagLengthValue value) => new(value.GetTag());
    public override byte[] EncodeValue(BerCodec codec) => Array.Empty<byte>();
    public override byte[] EncodeValue(BerCodec codec, int length) => Array.Empty<byte>();
    public override byte[] EncodeTagLengthValue(BerCodec codec, int length) => EncodeTagLengthValue();

    /// <exception cref="Exceptions.BerParsingException"></exception>
    private byte[] EncodeTagLengthValue()
    {
        byte[] result = new byte[2 + _Tag.GetByteCount()];

        _Tag.Serialize().AsSpan().CopyTo(result);

        result[^2] = 0;
        result[^3] = 0;

        return result;
    }

    public new byte[] EncodeTagLengthValue(BerCodec codec) => EncodeTagLengthValue();

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