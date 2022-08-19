using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerScriptIdentifierTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {40, 81, 38, 112};

    #endregion

    #region Constructor

    public IssuerScriptIdentifierTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerScriptIdentifierTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerScriptIdentifier.Tag;
}