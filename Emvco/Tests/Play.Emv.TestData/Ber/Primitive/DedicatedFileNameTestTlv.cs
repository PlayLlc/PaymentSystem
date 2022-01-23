using Play.Ber.Identifiers;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.TestData.Ber.Primitive;

public class DedicatedFileNameTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] {0x80, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40};

    #endregion

    #region Constructor

    public DedicatedFileNameTestTlv() : base(_DefaultContentOctets)
    { }

    public DedicatedFileNameTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return DedicatedFileName.Tag;
    }

    #endregion
}