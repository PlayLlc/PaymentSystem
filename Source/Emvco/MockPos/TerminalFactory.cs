using Play.Emv.Configuration;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Session;
using Play.Messaging;

namespace MockPos
{
    internal class TerminalFactory
    {
        #region Instance Members

        public static TerminalEndpoint Create(
            TerminalConfiguration terminalConfiguration, SystemTraceAuditNumberConfiguration systemTraceAuditNumberConfiguration,
            IEndpointClient endpointClient) =>
            TerminalEndpoint.Create(terminalConfiguration, systemTraceAuditNumberConfiguration, endpointClient);

        #endregion
    }
}