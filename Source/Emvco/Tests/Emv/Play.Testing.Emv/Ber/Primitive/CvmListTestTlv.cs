using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CvmListTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x08, 0x32, 0x3c, 0x4d, 0x16, 0x10, 0x8, 0x2, 0x34, 0x8F,
        0x45, 0x7C
    };

    #endregion

    #region Constructor

    public CvmListTestTlv() : base(_DefaultContentOctets)
    { }

    public CvmListTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => CvmList.Tag;
}