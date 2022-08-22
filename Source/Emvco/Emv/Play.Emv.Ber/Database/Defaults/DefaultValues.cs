using Play.Ber.DataObjects;
using Play.Ber.Tags;

namespace Play.Emv.Ber;

/// <summary>
///     Represents the default values for any <see cref="PrimitiveValue" /> objects that are required within a bounded
///     <see cref="KnownObjects" /> context
/// </summary>
public abstract class DefaultValues
{
    #region Instance Members

    public abstract IEnumerable<PrimitiveValue> GetDefaults(KnownObjects knownObjects);

    #endregion
}