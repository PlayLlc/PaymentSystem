using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MagstripeApplicationVersionNumberReaderTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 13 };

    public MagstripeApplicationVersionNumberReaderTestTlv() : base(_DefaultContentOctets) { }

    public MagstripeApplicationVersionNumberReaderTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MagstripeApplicationVersionNumberReader.Tag;
}
