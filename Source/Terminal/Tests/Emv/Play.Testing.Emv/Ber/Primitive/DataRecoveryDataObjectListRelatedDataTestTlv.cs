using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataRecoveryDataObjectListRelatedDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x02, 0x03, 0x00, 14, 27, 132 };

    public DataRecoveryDataObjectListRelatedDataTestTlv() : base(_DefaultContentOctets) { }

    public DataRecoveryDataObjectListRelatedDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataRecoveryDataObjectListRelatedData.Tag;
}
