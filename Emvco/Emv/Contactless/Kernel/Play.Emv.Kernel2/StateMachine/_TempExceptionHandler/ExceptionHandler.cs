using System;

using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel2.StateMachine._TempExceptionHandler;

public abstract class ExceptionHandler
{
    #region Instance Values

    protected readonly IKernelEndpoint _KernelEndpoint;
    protected readonly KernelDatabase _KernelDatabase;

    #endregion

    #region Constructor

    protected ExceptionHandler(IKernelEndpoint kernelEndpoint, KernelDatabase kernelDatabase)
    {
        _KernelEndpoint = kernelEndpoint;
        _KernelDatabase = kernelDatabase;
    }

    #endregion

    #region Instance Members

    public abstract Level1Error GetLevel1Error();
    public abstract bool TryCatchError(Action action);

    #endregion
}