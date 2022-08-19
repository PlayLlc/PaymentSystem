using Play.Emv.Configuration;
using Play.Emv.Kernel.Services;
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
    private static void Main(string[] args)
    {
        TerminalConfiguration terminalConfiguration = ConfigurationMockFactory.CreateTerminalConfiguration();
        ReaderConfiguration readerConfiguration = ConfigurationMockFactory.CreateReaderConfiguration();
        SystemTraceAuditNumberConfiguration systemTraceAuditNumberConfiguration = ConfigurationMockFactory.CreateSystemTraceAuditNumberConfiguration();
        SelectionConfiguration selectionConfiguration = ConfigurationMockFactory.CreateSelectionConfiguration();
        MessageBus messageBus = new();
        IEndpointClient endpointClient = messageBus.GetEndpointClient();

        TerminalEndpoint terminalEndpoint = TerminalFactory.Create(terminalConfiguration, systemTraceAuditNumberConfiguration, endpointClient);
        MainEndpoint mainEndpoint = ReaderFactory.Create(readerConfiguration, endpointClient);
        KernelEndpoint kernelEndpoint = KernelFactory.Create(terminalConfiguration, readerConfiguration, endpointClient);
        SelectionEndpoint selectionEndpoint = SelectionEndpoint.Create(selectionConfiguration, endpointClient);
    }
}