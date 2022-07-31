using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageSummaryStatusTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x7E };

    public DataStorageSummaryStatusTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageSummaryStatusTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageSummaryStatus.Tag;
}
