﻿using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services;

public interface IValidateCombinationCapability
{
    #region Instance Members

    public void Process(KernelDatabase database);

    #endregion
}