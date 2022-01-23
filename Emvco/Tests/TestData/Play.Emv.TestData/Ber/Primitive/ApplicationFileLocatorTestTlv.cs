using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class ApplicationFileLocatorTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01, 0x00};

    #endregion

    #region Constructor

    public ApplicationFileLocatorTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationFileLocatorTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return ApplicationFileLocator.Tag;
    }

    #endregion
}