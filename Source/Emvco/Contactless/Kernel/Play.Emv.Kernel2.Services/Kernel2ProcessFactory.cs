using Play.Emv.Display.Contracts;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel2.Configuration;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel2.Services;

public class Kernel2ProcessFactory
{
    #region Instance Members

    public static Kernel2Process Create(
        ICleanTornTransactions tornTransactionCleaner, Kernel2Configuration kernel2Configuration, Kernel2PersistentValues kernel2PersistentValues,
        Kernel2KnownObjects knownObjects, IHandleTerminalRequests terminalEndpoint, IKernelEndpoint kernelEndpoint, IHandlePcdRequests pcdEndpoint,
        IGenerateUnpredictableNumber unpredictableNumberGenerator, IManageTornTransactions tornTransactionManager, CertificateAuthorityDataset[] certificates,
        IHandleDisplayRequests displayEndpoint, ScratchPad scratchPad)
    {
        KernelDatabase kernelDatabase = new(certificates, kernel2PersistentValues, knownObjects, scratchPad);

        Kernel2StateResolver kernel2StateResolver = Kernel2StateResolver.Create(tornTransactionCleaner, kernelDatabase,
            new DataExchangeKernelService(terminalEndpoint, kernelDatabase, kernelEndpoint), tornTransactionManager, terminalEndpoint, kernelEndpoint,
            pcdEndpoint, unpredictableNumberGenerator, displayEndpoint);
        Kernel2StateMachine stateMachine = new(kernel2StateResolver.GetKernelState(Idle.StateId));

        return new Kernel2Process(stateMachine);
    }

    #endregion
}