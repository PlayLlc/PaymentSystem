using Play.Ber.Codecs;

namespace Play.Interchange.Codecs;

public interface IInterchangeDataFieldCodec : IPlayCodec
{
    #region Instance Members

    protected static PlayEncodingId GetEncodingId(Type encoder) => new(encoder);

    /// <summary>
    ///     An method to get the Identifier for an instance of this class
    /// </summary>
    /// <returns></returns>
    public PlayEncodingId GetIdentifier();

    #endregion
}