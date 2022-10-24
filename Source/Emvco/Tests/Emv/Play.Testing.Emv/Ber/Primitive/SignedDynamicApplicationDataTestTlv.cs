using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class SignedDynamicApplicationDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
    33, 134, 55, 16, 78, 101, 34, 67, 28, 101, 34, 78, 201 };

    public SignedDynamicApplicationDataTestTlv() : base(_DefaultContentOctets) { }

    public SignedDynamicApplicationDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => SignedDynamicApplicationData.Tag;
}
