using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CvmCapabilityNoCvmRequiredTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x8C};

    #endregion

    #region Constructor

    public CvmCapabilityNoCvmRequiredTestTlv() : base(_DefaultContentOctets)
    { }

    public CvmCapabilityNoCvmRequiredTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => CvmCapabilityNoCvmRequired.Tag;
}