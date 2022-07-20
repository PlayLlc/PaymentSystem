using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CvmListTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { };

    public CvmListTestTlv() : base(_DefaultContentOctets) { }

    public CvmListTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CvmList.Tag;
}
