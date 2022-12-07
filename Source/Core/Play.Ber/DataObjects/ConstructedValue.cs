using System;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Lengths;
using Play.Ber.Tags;

namespace Play.Ber.DataObjects;

public abstract record ConstructedValue : IEncodeBerDataObjects, IRetrieveConstructedValueMetadata
{
    #region Serialization

    /// <summary>
    ///     Encodes the Value field of the constructed TLV this object represents
    /// </summary>
    /// <param name="codec"></param>
    /// <returns></returns>
    public abstract byte[] EncodeValue(BerCodec codec);

    /// <summary>
    ///     Encodes the Tag-Length-Value fields of the constructed TLV this object represents
    /// </summary>
    /// <param name="codec"></param>
    /// <returns></returns>
    public abstract byte[] EncodeTagLengthValue(BerCodec codec);

    #endregion

    #region Instance Members

    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public abstract Tag GetTag();

    /// <summary>
    ///     The byte count of the Tag, Length and Value fields for this Constructed TLV object
    /// </summary>
    /// <param name="codec"></param>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public uint GetTagLengthValueByteCount(BerCodec codec)
    {
        ushort valueOctetCount = GetValueByteCount(codec);

        return checked((uint) Tag.GetByteCount(this) + new Length(valueOctetCount).GetByteCount() + GetValueByteCount(codec));
    }

    /// <summary>
    ///     The byte count of the Value field for this Constructed TLV object
    /// </summary>
    /// <param name="codec"></param>
    public abstract ushort GetValueByteCount(BerCodec codec);

    #endregion
}