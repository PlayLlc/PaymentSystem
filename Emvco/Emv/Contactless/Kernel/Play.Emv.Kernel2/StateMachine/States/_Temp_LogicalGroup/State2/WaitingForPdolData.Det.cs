using System;

using Play.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup;

public partial class WaitingForPdolData : KernelState
{
    #region DET

    #region QueryTerminalResponse

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleTimeout(session))
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

    /// <summary>
    ///     UpdateDataExchangeSignal
    /// </summary>
    /// <param name="signal"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void UpdateDataExchangeSignal(QueryTerminalResponse signal)
    {
        _KernelDatabase.Update(signal.GetDataToSend().AsTagLengthValueArray());
    }

    #endregion

    #region S2.7

    /// <summary>
    ///     IsPdolDataMissing
    /// </summary>
    /// <param name="session"></param>
    /// <param name="pdol"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
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

    #endregion
}