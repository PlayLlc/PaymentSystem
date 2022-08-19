using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationTransactionCounterTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {21, 12};

    #endregion

    #region Constructor

    public ApplicationTransactionCounterTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationTransactionCounterTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationTransactionCounter.Tag;
}