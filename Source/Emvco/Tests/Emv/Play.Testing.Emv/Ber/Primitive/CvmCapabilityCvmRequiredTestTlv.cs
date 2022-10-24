using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CvmCapabilityCvmRequiredTestTlv : TestTlv
{
    private readonly static byte[] _DefaultContentOctets = { 13 };

    public CvmCapabilityCvmRequiredTestTlv() : base(_DefaultContentOctets) { }

    public CvmCapabilityCvmRequiredTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CvmCapabilityCvmRequired.Tag;
}
