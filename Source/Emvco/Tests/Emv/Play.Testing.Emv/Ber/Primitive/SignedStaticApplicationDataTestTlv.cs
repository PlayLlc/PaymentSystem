using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class SignedStaticApplicationDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
    33, 134, 55, 16, 78, 101, 34, 67, 28, 101, 34, 78, 201 };

    public SignedStaticApplicationDataTestTlv() : base(_DefaultContentOctets) { }

    public SignedStaticApplicationDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => SignedStaticApplicationData.Tag;
}
