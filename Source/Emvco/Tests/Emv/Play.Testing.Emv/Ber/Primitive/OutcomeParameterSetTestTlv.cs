using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public sealed class OutcomeParameterSetTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 14, 23, 101, 28, 34, 56, 33, 44 };

    public OutcomeParameterSetTestTlv() : base(_DefaultContentOctets) { }

    public OutcomeParameterSetTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => OutcomeParameterSet.Tag;
}
