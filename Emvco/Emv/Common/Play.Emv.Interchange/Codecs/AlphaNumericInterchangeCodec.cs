using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

/// <summary>
///     An encoder for encoding and decoding alphabetic and numeric ASCII characters
/// </summary>
/// <remarks>
///     Strict parsing is enforced. Exceptions will be raised if invalid data is attempted to be parsed
/// </remarks>
public class AlphaNumericInterchangeCodec : AlphaNumericEmvCodec, IInterchangeCodec
{
    #region Static Metadata

    public static readonly InterchangeEncodingId Identifier = IInterchangeCodec.GetEncodingId(typeof(AlphaNumericInterchangeCodec));

    #endregion

    #region Instance Members

    public InterchangeEncodingId GetIdentifier() => Identifier;

    #endregion
}