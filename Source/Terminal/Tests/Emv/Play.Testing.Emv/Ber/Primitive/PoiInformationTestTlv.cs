using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PoiInformationTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
    {
        17, 134, 25, 26, 65, 174, 208, 109, 38, 47, 27,
        34, 18, 27, 103, 78, 64, 33, 12, 50
    };

    public PoiInformationTestTlv() : base(_DefaultContentOctets) { }

    public PoiInformationTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PoiInformation.Tag;
}
