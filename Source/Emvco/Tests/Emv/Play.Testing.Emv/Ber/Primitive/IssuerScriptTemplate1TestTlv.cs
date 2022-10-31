using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerScriptTemplate1TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38, 40, 81, 38 };

    public IssuerScriptTemplate1TestTlv() : base(_DefaultContentOctets) { }

    public IssuerScriptTemplate1TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerScriptTemplate1.Tag;
}
