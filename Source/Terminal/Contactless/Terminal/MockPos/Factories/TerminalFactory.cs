using Play.Emv.Configuration;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Session;
using Play.Messaging;

namespace MockPos.Factories
{
    internal class TerminalFactory
    {
        #region Instance Members

        public static TerminalEndpoint Create(TerminalConfiguration terminalConfiguration, IEndpointClient endpointClient) =>
            TerminalEndpoint.Create(terminalConfiguration, endpointClient);

        #endregion
    }
}