using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerActionCodeOnlineTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 144, 14, 181, 51, 91 };

    public IssuerActionCodeOnlineTestTlv() : base(_DefaultContentOctets) { }

    public IssuerActionCodeOnlineTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerActionCodeOnline.Tag;
}
