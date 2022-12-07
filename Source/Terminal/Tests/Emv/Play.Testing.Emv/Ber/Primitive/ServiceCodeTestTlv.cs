using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ServiceCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 9, 34 };

    public ServiceCodeTestTlv() : base(_DefaultContentOctets) { }

    public ServiceCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ServiceCode.Tag;
}
