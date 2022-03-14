using System;

using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public class CommonProcessingS456 : CommonProcessing
{
    #region Instance Values

    protected readonly KernelDatabase _KernelDatabase;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;
    private readonly IHandlePcdRequests _PcdEndpoint;

    public CommonProcessingS456(KernelDatabase kernelDatabase)
    {
        _KernelDatabase = kernelDatabase;
    }

    protected override StateId[] _ValidStateIds { get; } = new StateId[]
    {
        WaitingForEmvReadRecordResponse.StateId, WaitingForGetDataResponse.StateId, WaitingForEmvModeFirstWriteFlag.StateId
    };

    #endregion

    #region Instance Members

    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    public override KernelState Process(IGetKernelStateId kernelStateId, Kernel2Session session)
    {
        HandleRequestOutOfSync(kernelStateId.GetStateId());

        throw new NotImplementedException();
    }

    #endregion
}