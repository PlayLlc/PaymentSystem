using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ErrorIndicationTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 34, 28, 56, 64, 73 };

    public ErrorIndicationTestTlv() : base(_DefaultContentOctets) { }

    public ErrorIndicationTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ErrorIndication.Tag;
}
