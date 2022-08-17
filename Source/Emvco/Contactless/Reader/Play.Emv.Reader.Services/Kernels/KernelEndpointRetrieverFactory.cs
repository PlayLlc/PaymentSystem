using System.Linq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Contracts;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Emv.Terminal.Contracts;
using Play.Messaging;

namespace Play.Emv.Reader.Services;

internal class KernelEndpointRetrieverFactory
{
    #region Instance Members

    internal static KernelRetriever Create(
        ReaderDatabase readerDatabase, IManageTornTransactions tornTransactionsManager, IEndpointClient endpointClient,
        IManageTornTransactions tornTransactionLog, IGenerateUnpredictableNumber unpredictableNumberGenerator, ICleanTornTransactions tornTransactionCleaner,
        IReadOfflineBalance balanceReader, IValidateCombinationCapability combinationCapabilityValidator,
        IValidateCombinationCompatibility combinationCompatibilityValidator, ISelectCardholderVerificationMethod cardholderVerificationMethodSelector,
        IPerformTerminalActionAnalysis terminalActionAnalyzer, IAuthenticateTransactionSession authenticationService, ScratchPad scratchPad)
    {
        Kernel2PersistentValues persistentValues = new(readerDatabase.GetKernelConfiguration((KernelId) ShortKernelIdTypes.Kernel2));
        CertificateAuthorityDataset[] certificates = readerDatabase.GetCertificateAuthorityDatasets((KernelId) ShortKernelIdTypes.Kernel2);
        Kernel2KnownObjects kernel2KnownObjects = new();
        Kernel2Process kernel2Process = Kernel2ProcessFactory.Create(persistentValues, kernel2KnownObjects, certificates, endpointClient, tornTransactionLog,
            unpredictableNumberGenerator, tornTransactionCleaner, balanceReader, combinationCapabilityValidator, combinationCompatibilityValidator,
            cardholderVerificationMethodSelector, terminalActionAnalyzer, authenticationService, scratchPad);

        KernelProcess[] kernels = {kernel2Process};

        return new KernelRetriever(kernels.ToDictionary(a => a.GetKernelId(), b => b));
    }

    #endregion
}