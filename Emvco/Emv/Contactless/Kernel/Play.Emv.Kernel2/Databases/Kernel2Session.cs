﻿using Play.Emv.DataElements;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel2.Configuration;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2Session : KernelSession
{
    #region Instance Values

    private AcType _AcType = AcType.Aac;
    private OdaStatus _OdaStatus = OdaStatus.NotAvailable;
    private RrpCounter _RrpCounter = new(0);
    private bool _IsPdolDataMissing = true;

    #endregion

    #region Constructor

    public Kernel2Session(CorrelationId correlationId, KernelSessionId kernelSessionId) : base(correlationId, kernelSessionId)
    { }

    #endregion

    #region Read

    public AcType GetAcType() => _AcType;
    public OdaStatus GetOdaStatus() => _OdaStatus;
    public RrpCounter GetRrpCounter() => _RrpCounter;
    public bool IsPdolDataMissing() => _IsPdolDataMissing;

    #endregion

    #region Write

    public void SetIsPdolDataMissing(bool value) => _IsPdolDataMissing = value;
    public void Update(AcType value) => _AcType = value;
    public void Update(OdaStatus value) => _OdaStatus = value;
    public void Update(RrpCounter value) => _RrpCounter = value;

    #endregion
}