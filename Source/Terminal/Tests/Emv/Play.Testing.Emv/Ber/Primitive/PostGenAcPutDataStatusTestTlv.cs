using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PostGenAcPutDataStatusTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 37 };

    public PostGenAcPutDataStatusTestTlv() : base(_DefaultContentOctets) { }

    public PostGenAcPutDataStatusTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PostGenAcPutDataStatus.Tag;
}
