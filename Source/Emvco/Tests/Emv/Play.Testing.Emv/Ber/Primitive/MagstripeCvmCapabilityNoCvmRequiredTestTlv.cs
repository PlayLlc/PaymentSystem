using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MagstripeCvmCapabilityNoCvmRequiredTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x42};

    #endregion

    #region Constructor

    public MagstripeCvmCapabilityNoCvmRequiredTestTlv() : base(_DefaultContentOctets)
    { }

    public MagstripeCvmCapabilityNoCvmRequiredTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => MagstripeCvmCapabilityNoCvmRequired.Tag;
}