using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Contracts;

public class KernelPersistentConfiguration
{
    #region Instance Values

    private readonly KernelId _KernelId;
    private readonly TagLengthValue[] _PersistentConfiguration;

    #endregion

    #region Constructor

    public KernelPersistentConfiguration(KernelId kernelId, TagLengthValue[] persistentConfiguration)
    {
        _KernelId = kernelId;
        _PersistentConfiguration = persistentConfiguration;
    }

    #endregion

    #region Instance Members

    public KernelId GetKernelId() => _KernelId;
    public TagLengthValue[] GetPersistentValues() => _PersistentConfiguration;

    #endregion
}