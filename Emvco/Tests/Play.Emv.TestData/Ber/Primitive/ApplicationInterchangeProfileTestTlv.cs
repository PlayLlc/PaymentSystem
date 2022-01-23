using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class ApplicationInterchangeProfileTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] {0x1C, 0x00};

    #endregion

    #region Constructor

    public ApplicationInterchangeProfileTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationInterchangeProfileTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return ApplicationInterchangeProfile.Tag;
    }

    #endregion
}