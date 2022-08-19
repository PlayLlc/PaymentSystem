using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CvmCapabilityNoCvmRequiredTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x8C };

    public CvmCapabilityNoCvmRequiredTestTlv() : base(_DefaultContentOctets) { }

    public CvmCapabilityNoCvmRequiredTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CvmCapabilityNoCvmRequired.Tag;
}
