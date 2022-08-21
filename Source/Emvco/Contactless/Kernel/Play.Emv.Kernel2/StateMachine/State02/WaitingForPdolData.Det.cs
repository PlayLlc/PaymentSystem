using System;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPdolData : KernelState
{
    #region Instance Members

    // BUG: Need to make sure you're properly implementing each DEK handler for each state

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
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

        _EndpointClient.Send(capdu);

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #region S2.1, S2.3

    /// <remarks>Book C-2 Section S2.1, S2.3</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleTimeout(KernelSession session)
    {
        if (!session.Timer.IsTimedOut())
            return false;

        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(Level3Error.TimeOut);
        _Database.Initialize(DiscretionaryData.Tag);
        _DataExchangeKernelService.Initialize(DekResponseType.DiscretionaryData);
        _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, _Database.GetErrorIndication());

        _EndpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region S2.6

    /// <remarks>Book C-2 Section S2.6</remarks>
    /// <summary>
    ///     UpdateDataExchangeSignal
    /// </summary>
    /// <param name="signal"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateDataExchangeSignal(QueryTerminalResponse signal)
    {
        _Database.Update(signal.GetDataToSend().AsPrimitiveValues());
    }

    #endregion

    #region S2.7

    /// <remarks>Book C-2 Section S2.7</remarks>
    /// <summary>
    ///     IsPdolDataMissing
    /// </summary>
    /// <param name="session"></param>
    /// <param name="pdol"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsPdolDataMissing(Kernel2Session session, out ProcessingOptionsDataObjectList pdol)
    {
        pdol = _Database.Get<ProcessingOptionsDataObjectList>(ProcessingOptionsDataObjectList.Tag);

        if (!pdol!.IsRequestedDataAvailable(_Database))
            return false;

        session.SetIsPdolDataMissing(false);

        return true;
    }

    #endregion

    #region S2.8.1 - S2.8.6

    /// <remarks>Book C-2 Section S2.8.1 - S2.8.6</remarks>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    private GetProcessingOptionsRequest CreateGetProcessingOptionsCapdu(KernelSession session, ProcessingOptionsDataObjectList pdol) =>
        !_Database.IsPresentAndNotEmpty(ProcessingOptionsDataObjectList.Tag)
            ? GetProcessingOptionsRequest.Create(session.GetTransactionSessionId())
            : GetProcessingOptionsRequest.Create(pdol.AsDataObjectListResult(_Database), session.GetTransactionSessionId());

    #endregion

    #region S2.9

    /// <remarks>Book C-2 Section S2.9</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private static void StopTimer(Kernel2Session session)
    {
        session.Timer.Stop();
    }

    #endregion

    #endregion
}