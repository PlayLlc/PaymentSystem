using System.Collections.Generic;

using Play.Ber.DataObjects;

namespace Play.Emv.Kernel.Contracts;

public abstract class PersistentValues
{
    #region Instance Members

    public abstract IReadOnlyCollection<PrimitiveValue> GetPersistentValues();

    #endregion
}