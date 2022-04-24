using Play.Ber.Identifiers;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public abstract class ConstructedTlv : TestTlv
{
    #region Constructor

    protected ConstructedTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    { }

    protected ConstructedTlv(byte[] value) : base(value)
    { }

    #endregion
}