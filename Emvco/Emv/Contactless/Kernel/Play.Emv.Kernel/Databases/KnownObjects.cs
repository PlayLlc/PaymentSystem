using Play.Ber.Identifiers;
using Play.Core;

namespace Play.Emv.Kernel.Databases;

public abstract record KnownObjects : EnumObject<Tag>
{
    #region Constructor

    protected KnownObjects()
    { }

    protected KnownObjects(Tag value) : base(value)
    { }

    #endregion

    #region Instance Members

    public abstract bool Exists(Tag value);

    #endregion
}