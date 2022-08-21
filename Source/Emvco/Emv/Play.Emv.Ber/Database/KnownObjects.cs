using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Core;

namespace Play.Emv.Ber;

/// <summary>
///     Represents all of the known <see cref="PrimitiveValue" /> objects within a bounded context, such as a specific
///     kernel implementation
/// </summary>
public abstract record KnownObjects : EnumObject<uint>
{
    #region Constructor

    protected KnownObjects()
    { }

    protected KnownObjects(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public abstract bool Exists(Tag value);

    #endregion
}