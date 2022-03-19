using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class MerchantIdentifierTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x42, 0x43, 0x54, 0x45, 0x53, 0x54, 0x20, 0x31,
        0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38
    };

    #endregion

    #region Constructor

    public MerchantIdentifierTestTlv() : base(_DefaultContentOctets)
    { }

    public MerchantIdentifierTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => MerchantIdentifier.Tag;

    #endregion
}