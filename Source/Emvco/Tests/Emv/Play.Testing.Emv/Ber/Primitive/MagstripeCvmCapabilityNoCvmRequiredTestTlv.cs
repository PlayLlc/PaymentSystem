using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MagstripeCvmCapabilityNoCvmRequiredTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x42 };

    public MagstripeCvmCapabilityNoCvmRequiredTestTlv() : base(_DefaultContentOctets) { }

    public MagstripeCvmCapabilityNoCvmRequiredTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MagstripeCvmCapabilityNoCvmRequired.Tag;
}
