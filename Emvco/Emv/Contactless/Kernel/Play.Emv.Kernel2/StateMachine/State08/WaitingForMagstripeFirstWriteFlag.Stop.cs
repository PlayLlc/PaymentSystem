﻿using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class WaitingForMagstripeFirstWriteFlag
    {
        #region STOP

        public override KernelState Handle(KernelSession session, StopKernelRequest signal)
        {
            HandleRequestOutOfSync(session, signal);

            _Database.Update(Level3Error.Stop);
            _Database.Update(StatusOutcome.EndApplication);

            // HACK: This is being called twice when a STOP signal is requested by the Kernel State itself
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetOutcome()));

            // BUG: I think the book says to clear the database and session on stop but i think our implementation might still use DEK to grab the required data before sending it to the acquirer. Check the pattern in the book and your implementation
            Clear();

            return _KernelStateResolver.GetKernelState(Idle.StateId);
        }

        #endregion
    }
}