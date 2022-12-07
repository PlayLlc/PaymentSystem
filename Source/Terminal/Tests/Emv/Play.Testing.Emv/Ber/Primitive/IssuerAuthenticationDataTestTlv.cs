using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerAuthenticationDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 123, 222, 15, 16, 39, 190, 34, 212, 138, 47 };

    public IssuerAuthenticationDataTestTlv() : base(_DefaultContentOctets) { }

    public IssuerAuthenticationDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerAuthenticationData.Tag;
}
