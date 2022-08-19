using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerActionCodeDefaultTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 32, 34, 45, 16, 144 };

    public IssuerActionCodeDefaultTestTlv() : base(_DefaultContentOctets) { }

    public IssuerActionCodeDefaultTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerActionCodeDefault.Tag;
}
