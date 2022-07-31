using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ThirdPartyDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
{
        54, 44, 43, 20, 42, 4, 41, 43, 16, 20,
        55, 64, 13, 49, 23, 49, 54, 45, 44, 20,
        56, 49, 53, 41, 20, 20
    };

    public ThirdPartyDataTestTlv() : base(_DefaultContentOctets) { }

    public ThirdPartyDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ThirdPartyData.Tag;
}
