using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.Lengths;

namespace Play.Ber.DataObjects;

public abstract class ConstructedValue : IEncodeBerDataObjects, IRetrieveConstructedValueMetadata, IEqualityComparer<ConstructedValue>,
    IEquatable<ConstructedValue>
{
    #region Instance Members

    // TODO: Okay, so I even confused myself. So encoding Primitives returns encoded content octets, but
    // TODO: encoded constructed returns the entire raw TLV.... I'm an idiot
    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public abstract Tag GetTag();

    /// <summary>
    ///     The byte count of the Tag, Length and Value fields for this Constructed TLV object
    /// </summary>
    /// <param name="codec"></param>
    /// <exception cref="BerException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
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

    #region Equality

    public abstract bool Equals(ConstructedValue? x, ConstructedValue? y);
    public abstract bool Equals(ConstructedValue? other);
    public abstract int GetHashCode(ConstructedValue obj);

    #endregion
}