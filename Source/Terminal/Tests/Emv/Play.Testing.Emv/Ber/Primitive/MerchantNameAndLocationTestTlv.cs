using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MerchantNameAndLocationTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x61, 0x62, 0x63, 0x64, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
        0x20, 0x20, 0x20, 0x20, 0x20
    };

    #endregion

    #region Constructor

    public MerchantNameAndLocationTestTlv() : base(_DefaultContentOctets)
    { }

    public MerchantNameAndLocationTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => MerchantNameAndLocation.Tag;

    #endregion
}