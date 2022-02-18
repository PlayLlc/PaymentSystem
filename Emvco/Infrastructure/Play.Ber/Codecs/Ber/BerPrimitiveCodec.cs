using System;

using Play.Ber.InternalFactories;

namespace Play.Ber.Codecs;

public abstract class BerPrimitiveCodec : Codec
{
    #region Instance Members

    protected static BerEncodingId GetBerEncodingId(Type encoder) => new(encoder);

    /// <summary>
    ///     An method to get the Identifier for an instance of this class
    /// </summary>
    /// <returns></returns>
    public abstract BerEncodingId GetIdentifier();

    #endregion

    #region Serialization

    public abstract DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion
}