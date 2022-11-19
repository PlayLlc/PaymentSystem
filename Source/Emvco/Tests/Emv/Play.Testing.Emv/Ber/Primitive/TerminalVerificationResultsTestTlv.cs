using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalVerificationResultsTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 13, 124, 55, 67, 201 };

    public TerminalVerificationResultsTestTlv() : base(_DefaultContentOctets) { }

    public TerminalVerificationResultsTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalVerificationResults.Tag;
}
