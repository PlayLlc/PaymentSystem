using System.Collections.Generic;

using Play.Ber.DataObjects;

namespace Play.Emv.Configuration;

public abstract class PersistentValues
{
    #region Instance Members

    public abstract IReadOnlyCollection<TagLengthValue> GetPersistentValues();

    #endregion
}