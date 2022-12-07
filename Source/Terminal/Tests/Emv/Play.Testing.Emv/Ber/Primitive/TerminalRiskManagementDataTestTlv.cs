using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalRiskManagementDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 13, 64, 109, 25, 47, 108, 201, 91 };

    public TerminalRiskManagementDataTestTlv() : base(_DefaultContentOctets) { }

    public TerminalRiskManagementDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalRiskManagementData.Tag;
}
