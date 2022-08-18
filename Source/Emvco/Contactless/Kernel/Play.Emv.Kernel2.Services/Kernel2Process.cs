using System.Threading;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Security;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services;

public class Kernel2Process : KernelProcess
{
    #region Constructor

    private Kernel2Process(Kernel2StateMachine kernelStateMachine) : base(kernelStateMachine, new CancellationTokenSource())
    { }

    #endregion

    public static Kernel2Process Create(
        CertificateAuthorityDataset[] certificates, IEndpointClient endpointClient, IManageTornTransactions tornTransactionLog,
        IGenerateUnpredictableNumber unpredictableNumberGenerator, IValidateCombinationCapability combinationCapabilityValidator,
        IValidateCombinationCompatibility combinationCompatibilityValidator, ISelectCardholderVerificationMethod cardholderVerificationMethodSelector,
        IPerformTerminalActionAnalysis terminalActionAnalyzer, IAuthenticateTransactionSession authenticationService, ScratchPad scratchPad)
    {
        KernelDatabase database = new(certificates, new Kernel2PersistentValues(), new Kernel2KnownObjects(), scratchPad);
        DataExchangeKernelService dataExchangeKernelService = new(endpointClient, database);
        Kernel2StateResolver kernel2StateResolver = new(database, dataExchangeKernelService, endpointClient, tornTransactionLog, unpredictableNumberGenerator,
            combinationCapabilityValidator, combinationCompatibilityValidator, cardholderVerificationMethodSelector, terminalActionAnalyzer,
            authenticationService);
        Kernel2StateMachine stateMachine = new(kernel2StateResolver.GetKernelState(Idle.StateId));

        return new Kernel2Process(stateMachine);
    }

    #region Instance Members

    public override KernelId GetKernelId() => ShortKernelIdTypes.Kernel2;

    public override void Enqueue(StopKernelRequest message)
    {
        // BUG: Possible race condition between these 3 statements
        Clear();
        base.Enqueue(message);
        base.Enqueue(new CleanKernelRequest(message.GetKernelSessionId()));
    }

    #endregion
}