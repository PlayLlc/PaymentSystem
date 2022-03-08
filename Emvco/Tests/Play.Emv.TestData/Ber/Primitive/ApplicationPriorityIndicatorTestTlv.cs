using Play.Ber.Identifiers;

namespace Play.Emv.TestData.Ber.Primitive;

public class ApplicationPriorityIndicatorTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x02};

    #endregion

    #region Constructor

    public ApplicationPriorityIndicatorTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationPriorityIndicatorTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => ApplicationPriorityIndicator.Tag;

    #endregion
}