using System;

using Play.Emv.DataElements.Emv;
using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPdolData : KernelState
{
    #region DET

    #region QueryTerminalResponse

    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (HandleTimeout(session))
            return _KernelStateResolver.GetKernelState(StateId);

        UpdateDataExchangeSignal(signal);

        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (IsPdolDataMissing(kernel2Session, out ProcessingOptionsDataObjectList pdol))
            return _KernelStateResolver.GetKernelState(StateId);

        // TODO: GOTO S2.8.1
        GetProcessingOptionsRequest capdu = CreateGetProcessingOptionsCapdu(session, pdol);
        StopTimer(kernel2Session);

        _PcdEndpoint.Request(capdu);

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #region S2.6

    private void UpdateDataExchangeSignal(QueryTerminalResponse signal)
    {
        _KernelDatabase.UpdateRange(signal.GetDataToSend().AsTagLengthValueArray());
    }

    #endregion

    #region S2.7

    private bool IsPdolDataMissing(Kernel2Session session, out ProcessingOptionsDataObjectList pdol)
    {
        pdol = ProcessingOptionsDataObjectList.Decode(_KernelDatabase.Get(ProcessingOptionsDataObjectList.Tag).EncodeValue().AsSpan());

        if (!pdol!.IsRequestedDataAvailable(_KernelDatabase))
            return false;

        ((Kernel2Session) session).SetIsPdolDataMissing(false);

        return true;
    }

    #endregion

    #region S2.8.1 - S2.8.6

    public GetProcessingOptionsRequest CreateGetProcessingOptionsCapdu(KernelSession session, ProcessingOptionsDataObjectList pdol) =>
        !_KernelDatabase.IsPresentAndNotEmpty(ProcessingOptionsDataObjectList.Tag)
            ? GetProcessingOptionsRequest.Create(session.GetTransactionSessionId())
            : GetProcessingOptionsRequest.Create(pdol.AsDataObjectListResult(_KernelDatabase), session.GetTransactionSessionId());

    #endregion

    #region S2.9

    public void StopTimer(Kernel2Session session)
    {
        session.StopTimeout();
    }

    #endregion

    #endregion

    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}