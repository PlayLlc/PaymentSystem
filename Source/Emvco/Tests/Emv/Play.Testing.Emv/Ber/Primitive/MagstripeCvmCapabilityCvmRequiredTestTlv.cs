using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MagstripeCvmCapabilityCvmRequiredTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x38 };

    public MagstripeCvmCapabilityCvmRequiredTestTlv(): base(_DefaultContentOctets) { }

    public MagstripeCvmCapabilityCvmRequiredTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MagstripeCvmCapabilityCvmRequired.Tag;
}
