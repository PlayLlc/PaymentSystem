using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalTransactionQualifiersTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { };

    public TerminalTransactionQualifiersTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalTransactionQualifiers.Tag;
}
