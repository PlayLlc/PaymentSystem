using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MerchantCustomDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 1, 2, 3, 4, 5, 6, 7, 8, 9,
    10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20};

    public MerchantCustomDataTestTlv() : base(_DefaultContentOctets) { }

    public MerchantCustomDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MerchantCustomData.Tag;
}
