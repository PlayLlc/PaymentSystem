﻿using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderPinOffline
{
    #region Instance Members

    public CvmCode Process(KernelDatabase database);

    #endregion
}