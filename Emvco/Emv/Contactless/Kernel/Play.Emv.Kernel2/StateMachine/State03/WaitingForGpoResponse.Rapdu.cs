using System;

using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandleInvalidResultCode(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryPersistingRapdu(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandleMissingCardData(session))
            return _KernelStateResolver.GetKernelState(StateId);

        if (IsEmvModeSupported())
            return HandleEmvMode();

        if (TryHandlingMagstripeNotSupported())
            return HandleMagstripeMode();
    }

    #region S3.4 - S3.5

    /// <remarks>Book C-2 Section S3.4 - S3.5 </remarks>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsSuccessful())
            return false;

        session.StopTimeout();

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

    #region S3.8 - S3.9.2

    /// <remarks>Book C-2 Section S3.8 - S3.9.2 </remarks>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    private bool TryHandleInvalidResultCode(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.StatusBytes);
        _KernelDatabase.Update(signal.GetStatusWords());
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region S3.10 - S3.12

    private bool TryPersistingRapdu(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        try
        {
            _KernelDatabase.Update(((GetDataResponse) signal).GetTagLengthValueResult());
            _DataExchangeKernelService.Resolve((GetDataResponse) signal);

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Logging

            HandleBerParsingException(session, signal);

            return false;
        }
        catch (Exception)
        {
            // TODO: Logging

            HandleBerParsingException(session, signal);

            return false;
        }
    }

    private void HandleBerParsingException(KernelSession session, QueryPcdResponse signal)
    {
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Level2Error.ParsingError);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #region S3.13

    /// <remarks>Emv Book C-2 Section S3.13 </remarks>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    private bool TryHandleMissingCardData(KernelSession session)
    {
        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationFileLocator.Tag))
        {
            HandleMissingCardData(session);

            return true;
        }

        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationInterchangeProfile.Tag))
        {
            HandleMissingCardData(session);

            return true;
        }

        return false;
    }

    #endregion

    public bool IsEmvModeSupported()
    { }

    public KernelState HandleEmvMode()
    { }

    public bool TryHandlingMagstripeNotSupported()
    { }

    public KernelState HandleMagstripeMode()
    { }

    #region S3.90.1 - S3.90.2

    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    /// <remarks>Emv Book C-2 Section S3.90.1 - S3.90.2 </remarks>
    private void HandleMissingCardData(KernelSession session)
    {
        _KernelDatabase.Update(Level2Error.CardDataMissing);
        _KernelDatabase.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.Update(Status.NotReady);
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _DataExchangeKernelService.Enqueue(DekResponseType.DiscretionaryData, _KernelDatabase.GetErrorIndication());
        _KernelDatabase.SetUiRequestOnRestartPresent(true);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion
}