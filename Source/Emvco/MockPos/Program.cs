using Play.Emv.Configuration;
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
        ISettleTransactions settler = ConfigurationMockFactory.CreateSettler();
        TerminalConfiguration terminalConfiguration = ConfigurationMockFactory.CreateTerminalConfiguration();
        SystemTraceAuditNumberConfiguration systemTraceAuditNumberConfiguration = ConfigurationMockFactory.CreateSystemTraceAuditNumberConfiguration();

        TerminalEndpoint terminalEndpoint = TerminalEndpoint.Create(terminalConfiguration, systemTraceAuditNumberConfiguration, settler, messageBus);
    }
}