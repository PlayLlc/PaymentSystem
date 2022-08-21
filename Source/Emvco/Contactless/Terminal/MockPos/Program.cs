using System.Text.Json;

using MockPos.Configuration;
using MockPos.Services;

using Play.Emv.Configuration;
using Play.Emv.Display.Configuration;
using Play.Emv.Display.Services;
using Play.Emv.Kernel.Services;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Services;
using Play.Emv.Reader;
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
        //// Configuration
        //TerminalConfiguration terminalConfiguration = ConfigurationMockFactory.CreateTerminalConfiguration();
        //ReaderConfiguration readerConfiguration = ConfigurationMockFactory.CreateReaderConfiguration();
        //SystemTraceAuditNumberConfiguration systemTraceAuditNumberConfiguration = ConfigurationMockFactory.CreateSystemTraceAuditNumberConfiguration();
        //SelectionConfiguration selectionConfiguration = ConfigurationMockFactory.CreateSelectionConfiguration();
        //DisplayConfiguration displayConfiguration = ConfigurationMockFactory.CreateDisplayConfiguration();
        //PcdConfiguration pcdConfiguration = ConfigurationMockFactory.CreatePcdProtocolConfiguration();

        //// Services
        //MessageBus messageBus = new();
        //IEndpointClient endpointClient = messageBus.GetEndpointClient();
        //DisplayServiceMock displayServiceMock = new();
        //PcdServiceMock pcdServiceMock = new();

        //// Endpoint Processes
        //TerminalEndpoint terminalEndpoint = TerminalFactory.Create(terminalConfiguration, systemTraceAuditNumberConfiguration, endpointClient);
        //MainEndpoint mainEndpoint = ReaderFactory.Create(readerConfiguration, endpointClient);
        //KernelEndpoint kernelEndpoint = KernelFactory.Create(terminalConfiguration, readerConfiguration, endpointClient);
        //SelectionEndpoint selectionEndpoint = SelectionEndpoint.Create(selectionConfiguration, endpointClient);
        //DisplayEndpoint displayEndpoint = DisplayEndpoint.Create(displayConfiguration, displayServiceMock, displayServiceMock, endpointClient);
        //ProximityCouplingDeviceEndpoint pcdEndpoint = ProximityCouplingDeviceEndpoint.Create(pcdConfiguration, pcdServiceMock, endpointClient);

        GetDto();
        Console.WriteLine("HI");
    }

    private static void GetDto()
    {
        PosConfigurationDto? a =
            JsonSerializer.Deserialize<PosConfigurationDto>(
                File.ReadAllText(@"C:\Source\PaymentSystem\Source\Emvco\Contactless\Terminal\MockPos\TestJson.json"));

        Console.WriteLine(a);
        Console.ReadKey();
    }

    #endregion
}