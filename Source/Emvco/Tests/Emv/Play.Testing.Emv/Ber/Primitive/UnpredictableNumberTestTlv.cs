using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class UnpredictableNumberTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 44, 34, 27 };

    public UnpredictableNumberTestTlv() : base(_DefaultContentOctets) { }

    public UnpredictableNumberTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => UnpredictableNumber.Tag;
}
