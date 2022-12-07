using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class SecurityCapabilityTestTlv : TestTlv
{
    private readonly static byte[] _DefaultContentOctets = { 38 };

    public SecurityCapabilityTestTlv() : base(_DefaultContentOctets) { }

    public SecurityCapabilityTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => SecurityCapability.Tag;
}
