using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CardDataInputCapabilityTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 174 };

    public CardDataInputCapabilityTestTlv() : base(_DefaultContentOctets) { }

    public CardDataInputCapabilityTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CardDataInputCapability.Tag;
}
