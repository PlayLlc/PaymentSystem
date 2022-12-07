using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DeviceEstimatedTransmissionTimeForRelayResistanceRapduTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {9, 11};

    #endregion

    #region Constructor

    public DeviceEstimatedTransmissionTimeForRelayResistanceRapduTestTlv() : base(_DefaultContentOctets)
    { }

    public DeviceEstimatedTransmissionTimeForRelayResistanceRapduTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag;
}