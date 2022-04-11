using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Core;

namespace Play.Emv.Kernel.Databases;

/// <summary>
///     Represents all of the known <see cref="PrimitiveValue" /> objects within a bounded context, such as a specific
///     kernel implementation
/// </summary>
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