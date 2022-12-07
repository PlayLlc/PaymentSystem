using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class KernelConfigurationTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 15 };

    public KernelConfigurationTestTlv() : base(_DefaultContentOctets) { }

    public KernelConfigurationTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => KernelConfiguration.Tag;
}
