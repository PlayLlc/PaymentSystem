using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PreGenAcPutDataStatusTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34 };

    public PreGenAcPutDataStatusTestTlv() : base(_DefaultContentOctets) { }

    public PreGenAcPutDataStatusTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PreGenAcPutDataStatus.Tag;
}
