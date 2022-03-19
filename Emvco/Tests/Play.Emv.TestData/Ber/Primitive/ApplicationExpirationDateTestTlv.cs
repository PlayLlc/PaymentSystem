using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class ApplicationExpirationDateTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0X12, 0X21, 0X22};

    #endregion

    #region Constructor

    public ApplicationExpirationDateTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationExpirationDateTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => ApplicationExpirationDate.Tag;

    #endregion
}