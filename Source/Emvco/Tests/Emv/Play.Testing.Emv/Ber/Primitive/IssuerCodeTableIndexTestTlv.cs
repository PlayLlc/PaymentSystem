using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerCodeTableIndexTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 47 };

    public IssuerCodeTableIndexTestTlv() : base(_DefaultContentOctets) { }

    public IssuerCodeTableIndexTestTlv(byte[] contentOctets) : base(contentOctets)
    {

    }

    public override Tag GetTag() => IssuerCodeTableIndex.Tag;
}
