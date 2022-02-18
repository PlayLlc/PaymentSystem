using System.Numerics;

using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

public class BinaryInterchangeCodec : BinaryEmvCodec, IInterchangeCodec
{
    #region Static Metadata

    public static readonly InterchangeEncodingId Identifier = IInterchangeCodec.GetEncodingId(typeof(BinaryInterchangeCodec));

    #endregion

    #region Instance Members

    public InterchangeEncodingId GetIdentifier() => Identifier;

    #endregion
}