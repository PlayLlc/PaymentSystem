using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2KernelPersistentConfiguration : KernelPersistentConfiguration
{
    #region Constructor

    public Kernel2KernelPersistentConfiguration(PrimitiveValue[] persistentValues, IResolveKnownObjectsAtRuntime runtimeCodec) : base(
        ShortKernelIdTypes.Kernel2, persistentValues)
    {
        Kernel2KnownObjects knownObjects = Kernel2KnownObjects.Empty;
        EmvBook3DefaultValues book3Defaults = new();
        Kernel2DefaultValues kernel2Defaults = new();

        _PersistentValues.AddRange(book3Defaults.GetDefaults(knownObjects));
        _PersistentValues.AddRange(kernel2Defaults.GetDefaults(knownObjects));
    }

    #endregion
}