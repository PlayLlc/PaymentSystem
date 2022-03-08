using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel2.Configuration;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel2.Services;

public class Kernel2ProcessFactory
{
    #region Instance Members

    //Kernel2Configuration kernel2Configuration,  ITlvDatabase tlvDatabase,
    //    ICertificateAuthorityDatabase certificateAuthorityDatabase
    public static Kernel2Process Create(
        ICleanTornTransactions tornTransactionCleaner,
        Kernel2Configuration kernel2Configuration,
        Kernel2PersistentValues kernel2PersistentValues,
        IHandleTerminalRequests terminalEndpoint,
        IKernelEndpoint kernelEndpoint,
        IHandlePcdRequests pcdEndpoint,
        CertificateAuthorityDataset[] certificates)
    {
        Kernel2Database kernel2Database = new(kernel2Configuration, terminalEndpoint, new Kernel2TlvDatabase(kernel2PersistentValues),
            new KernelCertificateDatabase(certificates));

        Kernel2StateResolver kernel2StateResolver = Kernel2StateResolver.Create(tornTransactionCleaner, kernel2Database,
            new DataExchangeKernelService(terminalEndpoint, kernel2Database, kernelEndpoint, pcdEndpoint), terminalEndpoint, kernelEndpoint,
            pcdEndpoint);
        Kernel2StateMachine stateMachine = new(kernel2StateResolver.GetKernelState(Idle.StateId));

        return new Kernel2Process(stateMachine);
    }

    #endregion
}