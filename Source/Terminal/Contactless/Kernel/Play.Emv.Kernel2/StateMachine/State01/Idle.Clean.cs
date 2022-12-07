using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine;

public partial class Idle : KernelState
{
    #region Instance Members

    #region CLEAN

    public override KernelState Handle(CleanKernelRequest signal)
    {
        // CHECK: I don't think we're supposed to empty the torn transaction log here. Double check what this logic should be doing and when on a signal
        //_KernelCleaner.Clean();

        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Initialize(DiscretionaryData.Tag);
        _DataExchangeKernelService.Initialize(DekResponseType.DiscretionaryData);
        _TornTransactionLog.CleanOldRecords(_DataExchangeKernelService, DekResponseType.DiscretionaryData);

        _EndpointClient.Send(new OutKernelResponse(signal.GetCorrelationId(), signal.GetKernelSessionId(), _Database.GetTransaction()));

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion

    #endregion
}