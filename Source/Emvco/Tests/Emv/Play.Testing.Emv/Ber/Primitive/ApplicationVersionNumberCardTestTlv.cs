using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationVersionNumberCardTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 21, 12 };

    public ApplicationVersionNumberCardTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationVersionNumberCardTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ApplicationVersionNumberCard.Tag;
}
