using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

/// <summary>
///     An encoder for encoding and decoding alphabetic and numeric ASCII characters
/// </summary>
/// <remarks>
///     Strict parsing is enforced. Exceptions will be raised if invalid data is attempted to be parsed
/// </remarks>
public class AlphaNumericInterchangeDataFieldCodec : AlphaNumericEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    public static readonly InterchangeEncodingId Identifier =
        IInterchangeDataFieldCodec.GetEncodingId(typeof(AlphaNumericInterchangeDataFieldCodec));

    #endregion

    #region Instance Members

    public InterchangeEncodingId GetIdentifier() => Identifier;

    #endregion
}