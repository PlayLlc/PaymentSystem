using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationCryptogramTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34, 102, 24, 15, 87, 134, 201, 16 };

    public ApplicationCryptogramTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationCryptogramTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ApplicationCryptogram.Tag;
}
