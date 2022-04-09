using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Infrastructure.Ber.Primitive;

public class TransactionDateTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x14, 0x06, 0x17};

    #endregion

    #region Constructor

    public TransactionDateTestTlv() : base(_DefaultContentOctets)
    { }

    public TransactionDateTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => TransactionDate.Tag;

    #endregion
}