using System.Collections.Generic;

using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public class Kernel2StateResolver : IGetKernelState
{
    #region Instance Values

    private readonly Dictionary<StateId, KernelState> _KernelStateMap;

    #endregion

    #region Constructor

    private Kernel2StateResolver()
    {
        _KernelStateMap = new Dictionary<StateId, KernelState>();
    }

    #endregion

    #region Instance Members

    public static Kernel2StateResolver Create(
        ICleanTornTransactions tornTransactionCleaner, KernelDatabase kernelDatabase, DataExchangeKernelService dataExchangeKernelService,
        IManageTornTransactions tornTransactionManager, IHandleTerminalRequests terminalEndpoint, IKernelEndpoint kernelEndpoint,
        IHandlePcdRequests pcdEndpoint, IGenerateUnpredictableNumber unpredictableNumberGenerator)
    {
        Kernel2StateResolver kernelStateResolver = new();

        KernelState[] kernelStates =
        {
            new Idle(kernelDatabase, dataExchangeKernelService, kernelEndpoint, tornTransactionManager, kernelStateResolver,
                     pcdEndpoint, terminalEndpoint, tornTransactionCleaner, unpredictableNumberGenerator)
        };

        foreach (KernelState state in kernelStates)
            kernelStateResolver._KernelStateMap.Add(state.GetStateId(), state);

        return kernelStateResolver;
    }

    public KernelState GetKernelState(StateId stateId) => _KernelStateMap[stateId];

    #endregion
}