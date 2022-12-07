using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class KernelIdTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12 };

    public KernelIdTestTlv() : base(_DefaultContentOctets) { }

    public KernelIdTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => KernelId.Tag;
}
