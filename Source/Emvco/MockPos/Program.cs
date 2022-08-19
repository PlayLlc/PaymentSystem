using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Configuration;
using Play.Emv.Display.Services;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel2.Services;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Pcd.Services;
using Play.Emv.Reader;
using Play.Emv.Reader.Services;
using Play.Emv.Security;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Services;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Session;
using Play.Emv.Terminal.StateMachine;
using Play.Messaging;

namespace MockPos;

internal class Program
{
    #region Instance Members

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

        DisplayEndpoint displayEndpoint = DisplayEndpoint.Create(null, null, null, null);
        ProximityCouplingDeviceEndpoint pcdEndpoint = ProximityCouplingDeviceEndpoint.Create(null, null, null);
    }

    #endregion
}