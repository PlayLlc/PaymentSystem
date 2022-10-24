using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ReferenceControlParameterTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 48 };

    public ReferenceControlParameterTestTlv() : base(_DefaultContentOctets) { }

    public ReferenceControlParameterTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ReferenceControlParameter.Tag;
}
