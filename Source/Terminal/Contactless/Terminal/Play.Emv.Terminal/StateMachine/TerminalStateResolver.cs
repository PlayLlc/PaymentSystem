using System.Collections.Generic;

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

        #region Constructor

        public TerminalStateResolver(
            TerminalConfiguration terminalConfiguration, DataExchangeTerminalService dataExchangeTerminalService, IEndpointClient endpointClient,
            ISettleTransactions settleTransactions)
        {
            _TerminalStateMap = new Dictionary<StateId, TerminalState>();

            TerminalState[] terminalStates = {new Idle(terminalConfiguration, dataExchangeTerminalService, endpointClient, this, settleTransactions)};

            foreach (TerminalState state in terminalStates)
                _TerminalStateMap.Add(state.GetStateId(), state);
        }

        #endregion

        #region Instance Members

        public TerminalState GetKernelState(StateId stateId) => _TerminalStateMap[stateId];

        #endregion
    }
}