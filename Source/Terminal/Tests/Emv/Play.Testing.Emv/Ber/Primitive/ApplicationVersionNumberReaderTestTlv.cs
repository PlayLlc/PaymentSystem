using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationVersionNumberReaderTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34, 75 };

    public ApplicationVersionNumberReaderTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationVersionNumberReaderTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ApplicationVersionNumberReader.Tag;
}
