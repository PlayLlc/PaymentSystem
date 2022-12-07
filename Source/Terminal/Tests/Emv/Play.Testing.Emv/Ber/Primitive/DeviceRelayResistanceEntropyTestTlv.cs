using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DeviceRelayResistanceEntropyTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {7, 13, 9, 11};

    #endregion

    #region Constructor

    public DeviceRelayResistanceEntropyTestTlv() : base(_DefaultContentOctets)
    { }

    public DeviceRelayResistanceEntropyTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DeviceRelayResistanceEntropy.Tag;
}