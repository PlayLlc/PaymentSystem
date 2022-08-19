using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MagstripeCvmCapabilityCvmRequiredTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x38};

    #endregion

    #region Constructor

    public MagstripeCvmCapabilityCvmRequiredTestTlv() : base(_DefaultContentOctets)
    { }

    public MagstripeCvmCapabilityCvmRequiredTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => MagstripeCvmCapabilityCvmRequired.Tag;
}