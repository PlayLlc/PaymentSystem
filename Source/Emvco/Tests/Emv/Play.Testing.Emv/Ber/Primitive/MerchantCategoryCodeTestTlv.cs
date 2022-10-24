using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MerchantCategoryCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 33 };

    public MerchantCategoryCodeTestTlv() : base(_DefaultContentOctets) { }

    public MerchantCategoryCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MerchantCategoryCode.Tag;
}
