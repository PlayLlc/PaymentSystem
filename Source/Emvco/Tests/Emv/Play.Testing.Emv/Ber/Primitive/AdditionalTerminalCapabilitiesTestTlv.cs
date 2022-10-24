using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class AdditionalTerminalCapabilitiesTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 33, 13, 145, 27, 56 };

    public AdditionalTerminalCapabilitiesTestTlv() : base(_DefaultContentOctets) { }

    public AdditionalTerminalCapabilitiesTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => AdditionalTerminalCapabilities.Tag;
}
