using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CaPublicKeyIndexTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34 };

    public CaPublicKeyIndexTestTlv() : base(_DefaultContentOctets) { }

    public CaPublicKeyIndexTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CaPublicKeyIndex.Tag;
}
