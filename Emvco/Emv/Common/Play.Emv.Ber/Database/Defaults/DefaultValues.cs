using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Ber;

/// <summary>
///     Represents the default values for any <see cref="PrimitiveValue" /> objects that are required within a bounded
///     <see cref="KnownObjects" /> context
/// </summary>
public abstract class DefaultValues
{
    #region Instance Members

    public abstract void AddDefaults(KnownObjects knownObjects, Dictionary<Tag, PrimitiveValue> defaultValueMap);

    #endregion
}