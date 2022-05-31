using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Exceptions;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPutDataResponseAfterGenerateAc
{
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        PutDataResponse rapdu = (PutDataResponse) signal;

        // S15.5 - S15.8
        if (TryWritingDataToCard(session.GetTransactionSessionId(), rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S15.9 - S15.13
        HandleResponse(session);

        return _KernelStateResolver.GetKernelState(Idle.StateId);
    }

    #region S15.5 - S15.8

    /// <remarks>Book C-2 Section S15.5 - S15.8</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    private bool TryWritingDataToCard(TransactionSessionId sessionId, PutDataResponse rapdu)
    {
        if (rapdu.GetStatusWords() != StatusWords._9000)
            return false;

        if (!_Database.IsPresentAndNotEmpty(TagsToWriteBeforeGeneratingApplicationCryptogram.Tag))
            return false;

        if (!_DataExchangeKernelService.TryPeek(DekResponseType.TagsToWriteBeforeGenAc, out PrimitiveValue? tagToWrite))
            return false;

        SendPutData(sessionId, tagToWrite!);

        return true;
    }

    #endregion

    #region S15.8

    /// <remarks>Book C-2 Section S15.8</remarks>
    /// <exception cref="IccProtocolException"></exception>
    private void SendPutData(TransactionSessionId sessionId, PrimitiveValue tagToWrite)
    {
        PutDataRequest capdu = PutDataRequest.Create(sessionId, tagToWrite);
        _PcdEndpoint.Request(capdu);
    }

    #endregion

    #region S15.9 - S15.13

    /// <remarks>Book C-2 Section S15.9 - S15.13</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleResponse(KernelSession session)
    {
        _Database.Update(PostGenAcPutDataStatus.Completed);

        PosCardholderInteractionInformation pcii = _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

        if (pcii.IsSecondTapNeeded())
        {
            HandleDoubleTapRequiredResponse(session);

            return;
        }

        HandleDoubleTapNotRequiredResponse(session);
    }

    #endregion

    #region S15.10 - S15.11

    /// <remarks>Book C-2 Section S15.10 - S15.11</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleDoubleTapRequiredResponse(KernelSession session)
    {
        _Database.Update(Statuses.CardReadSuccessful);
        _DisplayEndpoint.Request(new DisplayMessageRequest(_Database.GetUserInterfaceRequestData()));
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _Database.Update(Statuses.ReadyToRead);
        _Database.Update(MessageHoldTime.MinimumValue);
        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetOutcome()));
    }

    #endregion

    #region S15.12 - S15.13

    /// <remarks>Book C-2 Section S15.12 - S15.13</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleDoubleTapNotRequiredResponse(KernelSession session)
    {
        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(MessageIdentifiers.ClearDisplay);
        builder.Set(Statuses.CardReadSuccessful);
        builder.Set(MessageHoldTime.MinimumValue);
        _DisplayEndpoint.Request(new DisplayMessageRequest(builder.Complete()));

        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _Database.SetUiRequestOnOutcomePresent(true);
        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetOutcome()));
    }

    #endregion
}