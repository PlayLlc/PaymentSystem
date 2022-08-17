using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Configuration;
using Play.Emv.Identifiers;
using Play.Emv.Terminal.DataExchange;
using Play.Messaging;

namespace Play.Emv.Terminal.StateMachine
{
    public class TerminalStateResolver : IGetTerminalState
    {
        #region Instance Values

        private readonly Dictionary<StateId, TerminalState> _TerminalStateMap;

        #endregion

        public TerminalState GetKernelState(StateId stateId) => _TerminalStateMap[stateId];

        public static TerminalStateResolver Create(
            TerminalConfiguration terminalConfiguration, DataExchangeTerminalService dataExchangeTerminalService, IEndpointClient endpointClient,
            ISettlementReconciliationService settlementReconciliationService)
        {
            TerminalStateResolver terminalStateResolver = new();

            TerminalState[] terminalStates =
            {
                new Idle(terminalConfiguration, dataExchangeTerminalService, endpointClient, terminalStateResolver, settlementReconciliationService)
            };

            foreach (TerminalState state in terminalStates)
                terminalStateResolver._TerminalStateMap.Add(state.GetStateId(), state);

            return terminalStateResolver;
        }
    }
}