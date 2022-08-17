using Play.Emv.Display.Contracts;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Emv.Terminal.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services;

public class Kernel2ProcessFactory
{
    #region Instance Members

    public static Kernel2Process Create(
        Kernel2PersistentValues kernel2PersistentValues, Kernel2KnownObjects knownObjects, CertificateAuthorityDataset[] certificates,
        IEndpointClient endpointClient, IManageTornTransactions tornTransactionLog, IGenerateUnpredictableNumber unpredictableNumberGenerator,
        ICleanTornTransactions tornTransactionCleaner, IReadOfflineBalance balanceReader, IValidateCombinationCapability combinationCapabilityValidator,
        IValidateCombinationCompatibility combinationCompatibilityValidator, ISelectCardholderVerificationMethod cardholderVerificationMethodSelector,
        IPerformTerminalActionAnalysis terminalActionAnalyzer, IAuthenticateTransactionSession authenticationService, ScratchPad scratchPad)
    {
        KernelDatabase database = new(certificates, kernel2PersistentValues, knownObjects, scratchPad);
        DataExchangeKernelService dataExchangeKernelService = new(endpointClient, database);

        Kernel2StateResolver kernel2StateResolver = new(database, dataExchangeKernelService, endpointClient, tornTransactionLog, unpredictableNumberGenerator,
            tornTransactionCleaner, balanceReader, combinationCapabilityValidator, combinationCompatibilityValidator, cardholderVerificationMethodSelector,
            terminalActionAnalyzer, authenticationService);
        Kernel2StateMachine stateMachine = new(kernel2StateResolver.GetKernelState(Idle.StateId));

        return new Kernel2Process(stateMachine);
    }

    #endregion
}