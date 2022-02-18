using System.Numerics;

using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

public class CompressedNumericInterchangeCodec : CompressedNumericEmvCodec, IInterchangeCodec
{
    #region Static Metadata

    public static readonly InterchangeEncodingId Identifier = IInterchangeCodec.GetEncodingId(typeof(CompressedNumericInterchangeCodec));

    #endregion

    #region Instance Members

    public InterchangeEncodingId GetIdentifier() => Identifier;

    #endregion
}