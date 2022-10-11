using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CardRiskManagementDataObjectList1RelatedDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
    {
        13, 2, 22, 16
    };

    public CardRiskManagementDataObjectList1RelatedDataTestTlv() : base(_DefaultContentOctets) { }

    public CardRiskManagementDataObjectList1RelatedDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CardRiskManagementDataObjectList1RelatedData.Tag;
}
