using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class AmountOtherNumericTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 13, 34, 55, 76, 91, 51 };

    public AmountOtherNumericTestTlv() : base(_DefaultContentOctets) { }

    public AmountOtherNumericTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => AmountOtherNumeric.Tag;
}
