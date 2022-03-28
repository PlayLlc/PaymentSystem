using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class WaitingForMagStripeReadRecordResponse
    {
        #region DET

        // BUG: Need to make sure you're properly implementing each DEK handler for each state

        /// <exception cref="RequestOutOfSyncException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
        {
            HandleRequestOutOfSync(session, signal);

            // S7.1
            UpdateDatabase(signal);

            return _KernelStateResolver.GetKernelState(StateId);
        }

        #region S7.1

        /// <exception cref="TerminalDataException"></exception>
        private void UpdateDatabase(QueryTerminalResponse signal)
        {
            _Database.Update(signal.GetDataToSend().AsPrimitiveValues());
        }

        #endregion

        #endregion
    }
}