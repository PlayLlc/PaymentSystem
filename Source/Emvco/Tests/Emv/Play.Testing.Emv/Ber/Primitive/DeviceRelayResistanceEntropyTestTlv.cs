using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DeviceRelayResistanceEntropyTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 7, 13, 9, 11 }; 

    public DeviceRelayResistanceEntropyTestTlv() : base(_DefaultContentOctets) { }

    public DeviceRelayResistanceEntropyTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DeviceRelayResistanceEntropy.Tag;
}
