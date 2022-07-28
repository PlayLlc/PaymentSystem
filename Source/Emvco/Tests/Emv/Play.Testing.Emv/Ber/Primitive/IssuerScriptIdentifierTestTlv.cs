using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerScriptIdentifierTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 40, 81, 38, 112 };

    public IssuerScriptIdentifierTestTlv() : base(_DefaultContentOctets) { }

    public IssuerScriptIdentifierTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerScriptIdentifier.Tag;
}
