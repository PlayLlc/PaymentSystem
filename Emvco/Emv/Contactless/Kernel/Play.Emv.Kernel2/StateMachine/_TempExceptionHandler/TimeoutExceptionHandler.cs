using System;

using Play.Emv.Ber.Enums;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel2.StateMachine._TempExceptionHandler;

public class TimeoutExceptionHandler : ExceptionHandler
{
    #region Constructor

    public TimeoutExceptionHandler(IKernelEndpoint kernelEndpoint, KernelDatabase kernelDatabase) : base(kernelEndpoint, kernelDatabase)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() => Level1Error.TimeOutError;
    public override bool TryCatchError(Action action) => throw new NotImplementedException();

    #endregion
}