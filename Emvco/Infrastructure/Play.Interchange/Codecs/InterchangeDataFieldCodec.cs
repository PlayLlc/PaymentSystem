using Play.Ber.Codecs;

namespace Play.Interchange.Codecs;

public abstract class InterchangeDataFieldCodec : Codec
{
    #region Instance Members

    protected static InterchangeEncodingId GetEncodingId(Type encoder) => new(encoder);

    /// <summary>
    ///     An method to get the Identifier for an instance of this class
    /// </summary>
    /// <returns></returns>
    public abstract InterchangeEncodingId GetIdentifier();

    #endregion
}