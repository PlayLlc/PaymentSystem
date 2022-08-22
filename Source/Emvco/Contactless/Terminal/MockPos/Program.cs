using System.Text.Json;

using MockPos.Dtos;
using MockPos.Factories;
using MockPos.Services;

using Play.Emv.Configuration;
using Play.Emv.Display.Configuration;
using Play.Emv.Display.Services;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel2;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Services;
using Play.Emv.Reader;
using Play.Emv.Reader.Configuration;
using Play.Emv.Reader.Services;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Services;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Session;
using Play.Messaging;

namespace MockPos;

internal class Program
{
    #region Instance Members

    private static void Main(string[] args)
    {
        // Configuration

        PosConfigurationDto posConfigurationDto = GetDto(); 

        TerminalConfiguration terminalConfiguration = posConfigurationDto.GetTerminalConfiguration();
        TransactionProfiles transactionProfiles = posConfigurationDto.GetTransactionProfiles();
        SelectionConfiguration selectionConfiguration = new(transactionProfiles, null);
        CertificateAuthorityDatasets certificateAuthorityDatasets = posConfigurationDto.GetCertificateAuthorityDatasets();
        KernelPersistentConfigurations kernelPersistentConfigurations = posConfigurationDto.GetKernelPersistent(new EmvRuntimeCodec());
        ReaderPersistentConfiguration readerPersistentConfiguration = posConfigurationDto.
        ReaderDatabase readerConfiguration = new ReaderDatabase(kernelPersistentConfigurations, selectionConfiguration.TransactionProfiles, certificateAuthorityDatasets, new ReaderPersistentConfiguration())
        DisplayConfiguration displayConfiguration = posConfigurationDto.GetDisplayConfiguration();
        PcdConfiguration pcdConfiguration = posConfigurationDto.GetPcdConfiguration();

        // Services
        MessageBus messageBus = new();
        IEndpointClient endpointClient = messageBus.GetEndpointClient();
        DisplayServiceMock displayServiceMock = new();
        PcdServiceMock pcdServiceMock = new();

        // Endpoint Processes
        TerminalEndpoint terminalEndpoint = TerminalFactory.Create(terminalConfiguration, systemTraceAuditNumberConfiguration, endpointClient);
        MainEndpoint mainEndpoint = ReaderFactory.Create(readerConfiguration, endpointClient);
        KernelEndpoint kernelEndpoint = KernelFactory.Create(terminalConfiguration, readerConfiguration, endpointClient);
        SelectionEndpoint selectionEndpoint = SelectionEndpoint.Create(selectionConfiguration, endpointClient);

        DisplayEndpoint displayEndpoint = DisplayEndpoint.Create(displayConfiguration, displayServiceMock, displayServiceMock, endpointClient);
        ProximityCouplingDeviceEndpoint pcdEndpoint = ProximityCouplingDeviceEndpoint.Create(pcdConfiguration, pcdServiceMock, endpointClient);





        GetDto();
        Console.WriteLine("HI");
    }

    private static PosConfigurationDto GetDto()
    {
        return
            JsonSerializer.Deserialize<PosConfigurationDto>(
                File.ReadAllText(@"C:\Source\PaymentSystem\Source\Emvco\Contactless\Terminal\MockPos\TestJson.json"))!; 
    }

    #endregion
}