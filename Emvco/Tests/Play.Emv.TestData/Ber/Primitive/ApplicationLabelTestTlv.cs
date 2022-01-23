using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class ApplicationLabelTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] {0x56, 0x49, 0x53, 0x41, 0x20, 0x50, 0x52, 0x45, 0x50, 0x41, 0x49, 0x44};

    #endregion

    #region Constructor

    public ApplicationLabelTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationLabelTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return ApplicationLabel.Tag;
    }

    #endregion
}