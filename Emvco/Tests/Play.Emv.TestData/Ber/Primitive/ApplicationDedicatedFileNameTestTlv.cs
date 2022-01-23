using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class ApplicationDedicatedFileNameTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40};

    #endregion

    #region Constructor

    public ApplicationDedicatedFileNameTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationDedicatedFileNameTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => ApplicationDedicatedFileName.Tag;

    #endregion
}