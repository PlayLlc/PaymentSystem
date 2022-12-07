using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalIdentificationTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
        { (byte) 'a', (byte) 'A', (byte) 'z', (byte) 'Z', (byte) '0', (byte) '9', (byte) '1', (byte) 'c' };

    public TerminalIdentificationTestTlv() : base(_DefaultContentOctets) { }

    public TerminalIdentificationTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalIdentification.Tag;
}
