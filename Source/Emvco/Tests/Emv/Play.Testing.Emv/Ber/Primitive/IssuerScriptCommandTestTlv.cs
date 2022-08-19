using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerScriptCommandTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38 };

    public IssuerScriptCommandTestTlv() : base(_DefaultContentOctets) { }

    public IssuerScriptCommandTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerScriptCommand.Tag;
}
