using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2PersistentValues : PersistentValues
{
    #region Instance Values

    private readonly PrimitiveValue[] _PersistentValues;

    #endregion

    #region Constructor

    public Kernel2PersistentValues(params PrimitiveValue[] values)
    {
        Kernel2KnownObjects knownObjects = new();
        EmvBook3DefaultValues book3Defaults = new();
        Kernel2DefaultValues kernel2Defaults = new();

        Dictionary<Tag, PrimitiveValue> persistentValues = new();
        book3Defaults.AddDefaults(knownObjects, persistentValues);
        kernel2Defaults.AddDefaults(knownObjects, persistentValues);
        UpdatePersistentConfiguration(knownObjects, persistentValues, values);

        _PersistentValues = persistentValues.Values.ToArray();
    }

    #endregion

    #region Instance Members

    protected static void UpdatePersistentConfiguration(
        Kernel2KnownObjects knownObjects, Dictionary<Tag, PrimitiveValue> persistentValues, PrimitiveValue[] configurationValues)
    {
        foreach (PrimitiveValue value in configurationValues)
        {
            // If there is a value that is not a persistent value we will ignore it
            if (!knownObjects.Exists(value.GetTag()))
                continue;

            persistentValues[value.GetTag()] = value;
        }
    }

    public override IReadOnlyCollection<PrimitiveValue> GetPersistentValues() => _PersistentValues;

    #endregion
}