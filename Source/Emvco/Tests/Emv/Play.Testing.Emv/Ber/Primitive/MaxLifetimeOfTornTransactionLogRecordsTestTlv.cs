using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MaxLifetimeOfTornTransactionLogRecordsTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 24, 46 };

    public MaxLifetimeOfTornTransactionLogRecordsTestTlv() : base(_DefaultContentOctets) { }

    public MaxLifetimeOfTornTransactionLogRecordsTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MaxLifetimeOfTornTransactionLogRecords.Tag;
}
