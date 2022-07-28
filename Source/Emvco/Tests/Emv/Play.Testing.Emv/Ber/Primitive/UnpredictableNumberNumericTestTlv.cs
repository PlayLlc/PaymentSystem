using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class UnpredictableNumberNumericTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 13, 22, 77, 31 };

    public UnpredictableNumberNumericTestTlv() : base(_DefaultContentOctets) { }

    public UnpredictableNumberNumericTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => UnpredictableNumberNumeric.Tag;
}
