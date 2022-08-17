using System.Linq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Contracts;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel2.Configuration;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Reader.Services;

internal class KernelEndpointRetrieverFactory
{
    #region Instance Members

    internal static KernelRetriever Create(
        ReaderDatabase readerDatabase, ICleanTornTransactions tornTransactionCleaner, IHandleTerminalRequests terminalEndpoint, IKernelEndpoint kernelEndpoint,
        IHandlePcdRequests pcdEndpoint, IGenerateUnpredictableNumber unpredictableNumberGenerator, IManageTornTransactions tornTransactionsManager,
        IHandleDisplayRequests displayEndpoint, ScratchPad scratchPad)
    {
        KernelProcess[] kernels =
        {
            Kernel2ProcessFactory.Create(tornTransactionCleaner,

                // HACK - We shouldn't need to use a concrete type to pull these values from the reader database
                new Kernel2PersistentValues(readerDatabase.GetKernelConfiguration((KernelId) ShortKernelIdTypes.Kernel2)), Kernel2KnownObjects.Empty,
                terminalEndpoint, kernelEndpoint, pcdEndpoint, unpredictableNumberGenerator, tornTransactionsManager,
                readerDatabase.GetCertificateAuthorityDatasets(new KernelId(ShortKernelIdTypes.Kernel2)), displayEndpoint, scratchPad)
        };

        return new KernelRetriever(kernels.ToDictionary(a => a.GetKernelId(), b => b));
    }

    #endregion
}