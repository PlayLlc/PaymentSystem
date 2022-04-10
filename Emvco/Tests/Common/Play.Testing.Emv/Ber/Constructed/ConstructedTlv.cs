using Play.Ber.Identifiers;
using Play.Testing.Emv.Infrastructure.Ber.Primitive;

namespace Play.Testing.Emv.Infrastructure.Ber.Constructed;

public abstract class ConstructedTlv : TestTlv
{
    #region Constructor

    protected ConstructedTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    { }

    #endregion
}