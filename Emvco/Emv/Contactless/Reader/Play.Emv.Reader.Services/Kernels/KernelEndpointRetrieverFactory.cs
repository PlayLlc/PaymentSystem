﻿using System.Linq;

using Play.Emv.DataElements;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel2.Configuration;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Reader.Services;

internal class KernelEndpointRetrieverFactory
{
    #region Instance Members

    internal static KernelRetriever Create(
        ReaderDatabase readerDatabase,
        ICleanTornTransactions tornTransactionCleaner,
        IHandleTerminalRequests terminalEndpoint,
        IKernelEndpoint kernelEndpoint,
        IHandlePcdRequests pcdEndpoint)
    {
        KernelProcess[] kernels =
        {
            Kernel2ProcessFactory.Create(tornTransactionCleaner,
                                         (Kernel2Configuration) readerDatabase.GetKernelConfiguration(ShortKernelIdTypes.Kernel2),

                                         // HACK - We shouldn't need to use a concrete type to pull these values from the reader database
                                         new Kernel2PersistentValues(new DatabaseValues(readerDatabase
                                                                                            .GetPersistentKernelValues(ShortKernelIdTypes
                                                                                                .Kernel2))), terminalEndpoint,
                                         kernelEndpoint, pcdEndpoint,

                                         //readerDatabase.GetPersistentKernelValues(ShortKernelId.Kernel2),
                                         readerDatabase.GetCertificateAuthorityDatasets(ShortKernelIdTypes.Kernel2))
        };

        return new KernelRetriever(kernels.ToDictionary(a => a.GetKernelId(), b => b));
    }

    #endregion
}