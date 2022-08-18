using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            IEndpointClient endpointClient)
        {
            ISettleTransactions settler = ConfigurationMockFactory.CreateSettler();

            return TerminalEndpoint.Create(terminalConfiguration, systemTraceAuditNumberConfiguration, settler, endpointClient);
        }
    }
}