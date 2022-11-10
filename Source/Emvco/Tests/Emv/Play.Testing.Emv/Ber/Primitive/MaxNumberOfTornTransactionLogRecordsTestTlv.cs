using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MaxNumberOfTornTransactionLogRecordsTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 4 };

    public MaxNumberOfTornTransactionLogRecordsTestTlv() : base(_DefaultContentOctets) { }

    public MaxNumberOfTornTransactionLogRecordsTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MaxNumberOfTornTransactionLogRecords.Tag;
}
