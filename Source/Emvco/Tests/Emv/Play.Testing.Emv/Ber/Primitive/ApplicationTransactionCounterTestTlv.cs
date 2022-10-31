using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationTransactionCounterTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 21, 12 };

    public ApplicationTransactionCounterTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationTransactionCounterTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ApplicationTransactionCounter.Tag;
}

