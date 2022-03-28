using Play.Emv.Ber;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class WaitingForMagStripeReadRecordResponse
    {
        #region RAPDU

        /// <summary>
        ///     Handle
        /// </summary>
        /// <param name="session"></param>
        /// <param name="signal"></param>
        /// <returns></returns>
        /// <exception cref="RequestOutOfSyncException"></exception>
        public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
        {
            HandleRequestOutOfSync(session, signal);

            if (TryHandleL1Error(session, signal))
                return _KernelStateResolver.GetKernelState(StateId);

            throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
        }

        private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
        {
            if (!signal.IsSuccessful())
                return false;

            _KernelDatabase.Update(MessageIdentifier.TryAgain);
            _KernelDatabase.Update(Status.ReadyToRead);
            _KernelDatabase.Update(new MessageHoldTime(0));
            _KernelDatabase.Update(StatusOutcome.EndApplication);
            _KernelDatabase.Update(StartOutcome.B);
            _KernelDatabase.SetUiRequestOnRestartPresent(true);
            _KernelDatabase.Update(signal.GetLevel1Error());
            _KernelDatabase.Update(MessageOnErrorIdentifier.TryAgain);
            _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

            _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

            return true;
        }

        #endregion
    }
}