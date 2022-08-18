using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Configuration;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services.Selection;
using Play.Emv.Kernel2.Services;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Reader;
using Play.Emv.Reader.Services;
using Play.Emv.Security;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Session;
using Play.Emv.Terminal.StateMachine;
using Play.Messaging;

namespace MockPos;

internal class Program
{
    private static void Main(string[] args)
    {
        MessageBus messageBus = new();
        TerminalConfiguration terminalConfiguration = ConfigurationMockFactory.CreateTerminalConfiguration();
        ReaderConfiguration readerConfiguration = ConfigurationMockFactory.CreateReaderConfiguration();
        SystemTraceAuditNumberConfiguration systemTraceAuditNumberConfiguration = ConfigurationMockFactory.CreateSystemTraceAuditNumberConfiguration();

        TerminalEndpoint terminalEndpoint = TerminalFactory.Create(terminalConfiguration, systemTraceAuditNumberConfiguration, messageBus);
        MainEndpoint mainEndpoint = ReaderFactory.Create(readerConfiguration, messageBus);
        KernelEndpoint kernelEndpoint = KernelFactory.Create(terminalConfiguration, readerConfiguration, messageBus);
    }
}