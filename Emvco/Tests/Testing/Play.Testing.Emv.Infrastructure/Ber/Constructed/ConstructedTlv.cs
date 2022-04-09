using Play.Ber.Identifiers;
using Play.Emv.TestData.Ber.Primitive;

namespace Play.Emv.TestData.Ber.Constructed;

public abstract class ConstructedTlv : TestTlv
{
    #region Constructor

    protected ConstructedTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    { }

    #endregion
}