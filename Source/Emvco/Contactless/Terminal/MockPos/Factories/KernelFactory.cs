using Play.Emv.Ber.Enums;
using Play.Emv.Configuration;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel2.Services;
using Play.Emv.Reader;
using Play.Emv.Security;
using Play.Messaging;

namespace MockPos.Factories
{
    internal class KernelFactory
    {
        #region Instance Members

        private static KernelProcess[] CreateKernelProcesses(
            TerminalConfiguration terminalConfiguration, ReaderDatabase readerDatabase, IEndpointClient endpointClient)
        {
            List<KernelProcess> kernelProcesses = new();

            CertificateAuthorityDataset[] certificates = readerDatabase.GetCertificateAuthorityDatasets(ShortKernelIdTypes.Kernel2);
            TornTransactionLog tornTransactionLog = new(terminalConfiguration.GetMaxNumberOfTornTransactionLogRecords(),
                terminalConfiguration.GeMaxLifetimeOfTornTransactionLogRecords());
            UnpredictableNumberGenerator unpredictableNumberGenerator = new();
            CombinationCompatibilityValidator combinationCompatibilityValidator = new();
            CombinationCapabilityValidator combinationCapabilityValidator = new();
            CardholderVerificationMethodSelector cardholderVerificationMethodSelector = new();
            TerminalActionAnalysisService terminalActionAnalysisService = new();
            SecurityAuthenticationService securityAuthenticationService = SecurityAuthenticationService.Create();
            ScratchPad scratchPad = new(terminalConfiguration.GetMaxNumberOfTornTransactionLogRecords(),
                terminalConfiguration.GeMaxLifetimeOfTornTransactionLogRecords());

            KernelProcess kernel2Process = Kernel2Process.Create(certificates, endpointClient, tornTransactionLog, unpredictableNumberGenerator,
                combinationCompatibilityValidator, combinationCapabilityValidator, cardholderVerificationMethodSelector, terminalActionAnalysisService,
                securityAuthenticationService, scratchPad);

            kernelProcesses.Add(kernel2Process);

            return kernelProcesses.ToArray();
        }

        public static KernelEndpoint Create(TerminalConfiguration terminalConfiguration, ReaderDatabase readerDatabase, IEndpointClient endpointClient)
        {
            KernelProcess[] kernelProcesses = CreateKernelProcesses(terminalConfiguration, readerDatabase, endpointClient);

            return KernelEndpoint.Create(kernelProcesses, endpointClient);
        }

        #endregion
    }
}