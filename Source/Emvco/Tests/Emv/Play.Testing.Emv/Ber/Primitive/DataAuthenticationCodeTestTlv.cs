using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataAuthenticationCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34, 114 };

    public DataAuthenticationCodeTestTlv() : base(_DefaultContentOctets) { }

    public DataAuthenticationCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataAuthenticationCode.Tag;
}
