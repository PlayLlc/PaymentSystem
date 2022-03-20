using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Ber.Lengths;
using Play.Codecs;
using Play.Emv.Ber.DataElements;

namespace Play.Ber.DataObjects;

public abstract record PrimitiveValue : IEqualityComparer<PrimitiveValue>, IEncodeBerDataObjects, IRetrievePrimitiveValueMetadata,
    IDecodeDataElement
{
    #region Instance Members

    public TagLengthValue AsTagLengthValue(BerCodec codec)
    {
        Tag tag = GetTag();
        byte[]? contentOctets = EncodeValue(codec);

        return new TagLengthValue(GetTag(), EncodeValue(codec));
    }

    public abstract PlayEncodingId GetEncodingId();
    public abstract Tag GetTag();

    public uint GetTagLengthValueByteCount(BerCodec codec) =>
        checked((uint) Tag.GetByteCount(this) + Length.GetByteCount(this, codec) + GetValueByteCount(codec));

    /// <summary>
    ///     Gets the Tag-Length-Value byte count of this object
    /// </summary>
    /// <param name="codec"></param>
    /// <param name="length">The length of this object's Value field</param>
    /// <returns></returns>
    public uint GetTagLengthValueByteCount(BerCodec codec, int length) =>
        checked((uint) Tag.GetByteCount(this) + Length.GetByteCount(this, codec) + GetValueByteCount(codec));

    public abstract ushort GetValueByteCount(BerCodec codec);

    #endregion

    #region Serialization

    public abstract PrimitiveValue Decode(TagLengthValue value);

    /// <summary>
    ///     Encodes this object as a Tag-Length-Value
    /// </summary>
    /// <param name="codec"></param>
    /// <returns></returns>
    public byte[] EncodeTagLengthValue(BerCodec codec) => codec.EncodeTagLengthValue(this);

    /// <summary>
    ///     Encodes this object as a Tag-Length-Value
    /// </summary>
    /// <param name="codec"></param>
    /// <param name="length">This parameter determines the length of the TLV Value field</param>
    /// <returns></returns>
    public byte[] EncodeTagLengthValue(BerCodec codec, int length) => codec.EncodeTagLengthValue(this, length);

    public abstract byte[] EncodeValue(BerCodec codec);
    public abstract byte[] EncodeValue(BerCodec codec, int length);

    #endregion

    #region Equality

    public bool Equals(PrimitiveValue? x, PrimitiveValue? y)
    {
        if (x is null)
            return y is null;

        return y is not null && x.Equals(y);
    }

    public int GetHashCode(PrimitiveValue obj) => obj.GetHashCode();

    #endregion
}