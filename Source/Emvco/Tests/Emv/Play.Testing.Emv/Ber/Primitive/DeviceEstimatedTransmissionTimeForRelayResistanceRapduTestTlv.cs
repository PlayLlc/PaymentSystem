using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DeviceEstimatedTransmissionTimeForRelayResistanceRapduTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 9, 11 };

    public DeviceEstimatedTransmissionTimeForRelayResistanceRapduTestTlv() : base(_DefaultContentOctets) { }

    public DeviceEstimatedTransmissionTimeForRelayResistanceRapduTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag;
}
