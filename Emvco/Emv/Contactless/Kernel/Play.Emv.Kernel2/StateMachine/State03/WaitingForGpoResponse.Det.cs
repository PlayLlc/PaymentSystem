using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    #region DET


#region Query Terminal Response
    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) 
    {
        HandleRequestOutOfSync(session, signal);

        UpdateDatabase(signal);

        return _KernelStateResolver.GetKernelState(StateId);
    }


  private void UpdateDatabase(QueryTerminalResponse signal)
    {
        _KernelDatabase.Update(signal.GetDataToSend().AsTagLengthValueArray());
    }

#endregion

    

#endregion
}