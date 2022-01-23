using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class TransactionDateTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] {0x14, 0x06, 0x17};

    #endregion

    #region Constructor

    public TransactionDateTestTlv() : base(_DefaultContentOctets)
    { }

    public TransactionDateTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return TransactionDate.Tag;
    }

    #endregion
}