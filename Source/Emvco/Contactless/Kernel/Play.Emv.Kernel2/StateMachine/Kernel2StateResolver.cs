using System.Collections.Generic;

using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Kernel2.Services.PrepareGenerateAc;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Emv.Terminal.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public class Kernel2StateResolver : IGetKernelState
{
    #region Instance Values

    private readonly Dictionary<StateId, KernelState> _KernelStateMap;

    #endregion

    #region Constructor

    public Kernel2StateResolver(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IEndpointClient endpointClient,
        IManageTornTransactions tornTransactionLog, IGenerateUnpredictableNumber unpredictableNumberGenerator,
        IValidateCombinationCapability combinationCapabilityValidator, IValidateCombinationCompatibility combinationCompatibilityValidator,
        ISelectCardholderVerificationMethod cardholderVerificationMethodSelector, IPerformTerminalActionAnalysis terminalActionAnalyzer,
        IAuthenticateTransactionSession authenticationService)
    {
        _KernelStateMap = new Dictionary<StateId, KernelState>();
        PrepareGenerateAcService applicationCryptogramGenerator = new(database, dataExchangeKernelService, this, endpointClient);
        OfflineBalanceReader offlineBalanceReader = new(database, dataExchangeKernelService, this, endpointClient);

        S3R1 s3R1 = new(database, dataExchangeKernelService, this, endpointClient);
        S456 s456 = new(database, dataExchangeKernelService, endpointClient, this, offlineBalanceReader, combinationCapabilityValidator,
            combinationCompatibilityValidator, cardholderVerificationMethodSelector, terminalActionAnalyzer, tornTransactionLog,
            applicationCryptogramGenerator);
        S78 s78 = new(database, dataExchangeKernelService, this, endpointClient, unpredictableNumberGenerator);
        S910 s910 = new(database, dataExchangeKernelService, this, endpointClient, authenticationService);

        KernelState[] kernelStates =
        {
            // State 1
            new Idle(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, unpredictableNumberGenerator),

            // State 2
            new WaitingForPdolData(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this),

            // State 3
            new WaitingForGpoResponse(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, unpredictableNumberGenerator, s3R1),

            // State 4
            new WaitingForEmvReadRecordResponse(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, s456),

            // State 5
            new WaitingForGetDataResponse(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, s456),

            // State 6
            new WaitingForEmvModeFirstWriteFlag(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, s456),

            // State 7
            new WaitingForMagStripeReadRecordResponse(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, s78),

            // State 8
            new WaitingForMagstripeFirstWriteFlag(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, s78),

            // State 9
            new WaitingForGenerateAcResponse1(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, offlineBalanceReader, s910),

            // State 10
            new WaitingForRecoverAcResponse(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, offlineBalanceReader,
                applicationCryptogramGenerator, s910),

            // State 11
            new WaitingForGenerateAcResponse2(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, authenticationService,
                offlineBalanceReader),

            // State 12
            new WaitingForPutDataResponseBeforeGenerateAc(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this,
                applicationCryptogramGenerator),

            // State 13
            new WaitingForCccResponse1(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this),

            // State 14
            new WaitingForCccResponse2(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this),

            // State 15
            new WaitingForPutDataResponseAfterGenerateAc(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this),

            // State 16
            new WaitingForPreGenAcBalance(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this, offlineBalanceReader),

            // State 17
            new WaitingForPostGenAcBalance(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this),

            // State R1
            new WaitingForExchangeRelayResistanceDataResponse(database, dataExchangeKernelService, endpointClient, tornTransactionLog, this,
                unpredictableNumberGenerator, s3R1)
        };

        foreach (KernelState state in kernelStates)
            _KernelStateMap.Add(state.GetStateId(), state);
    }

    #endregion

    #region Instance Members

    public KernelState GetKernelState(StateId stateId) => _KernelStateMap[stateId];

    #endregion
}