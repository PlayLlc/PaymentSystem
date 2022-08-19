using Play.Emv.Configuration;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Session;
using Play.Emv.Terminal.StateMachine;
using Play.Messaging;

namespace MockPos
{
    internal class TerminalFactory
    {
        public static TerminalEndpoint Create(
            TerminalConfiguration terminalConfiguration, SystemTraceAuditNumberConfiguration systemTraceAuditNumberConfiguration,
            IEndpointClient endpointClient) =>
            TerminalEndpoint.Create(terminalConfiguration, systemTraceAuditNumberConfiguration, endpointClient);
    }
}