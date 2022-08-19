using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerScriptTemplate2TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38 };

    public IssuerScriptTemplate2TestTlv() : base(_DefaultContentOctets) { }

    public IssuerScriptTemplate2TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerScriptTemplate2.Tag;
}
