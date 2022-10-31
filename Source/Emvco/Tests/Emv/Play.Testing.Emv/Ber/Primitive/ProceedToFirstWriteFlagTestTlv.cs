using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ProceedToFirstWriteFlagTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 134 };

    public ProceedToFirstWriteFlagTestTlv() : base(_DefaultContentOctets) { }

    public ProceedToFirstWriteFlagTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ProceedToFirstWriteFlag.Tag;
}
