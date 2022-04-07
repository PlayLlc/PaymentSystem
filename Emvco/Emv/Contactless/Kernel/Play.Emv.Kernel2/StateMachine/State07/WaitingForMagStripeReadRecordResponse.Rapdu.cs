using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

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
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        // S7.4 - S7.6
        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // S7.9 - S7.10.2
        if (TryHandleInvalidResultCode(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // S7.11 - S7.16
        if (!TryResolveActiveRecords(session, (ReadRecordResponse) signal))
            return _KernelStateResolver.GetKernelState(StateId);

        // S7.17 - S7.19
        if (IsReadingRequired((Kernel2Session) session))
            return _KernelStateResolver.GetKernelState(StateId);

        // S7.20 - S7.21.2
        if (TryHandlingMissingMandatoryObjects(session.GetKernelSessionId()))
            return _KernelStateResolver.GetKernelState(StateId);

        // S7.22, S7.24.1 - S7.24.2
        if (!IsMagstripeDataOkay(session.GetKernelSessionId()))
            return _KernelStateResolver.GetKernelState(StateId);

        UpdateDiscretionaryTrackData();

        return _KernelStateResolver.GetKernelState(_S78.Process(this, (Kernel2Session) session));
    }

    #region S7.4 - S7.6

    /// <remarks>EMV Book C-2 Section S7.4 - S7.6</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsSuccessful())
            return false;

        try
        {
            // HACK: Move exception handling to a single exception handler
            _Database.Update(MessageIdentifiers.TryAgain);
            _Database.Update(Status.ReadyToRead);
            _Database.Update(new MessageHoldTime(0));
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcome.B);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.Update(signal.GetLevel1Error());
            _Database.Update(MessageOnErrorIdentifiers.TryAgain);
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

            return true;
        }
        catch (DataElementParsingException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
        }

        return false;
    }

    #endregion

    #region S7.9 - S7.10.2

    /// <remarks>EMV Book C-2 Section S7.9 - S7.10.2</remarks>
    private bool TryHandleInvalidResultCode(KernelSession session, QueryPcdResponse signal)
    {
        if (signal.GetStatusWords() == StatusWords._9000)
            return false;

        try
        {
            // HACK: Move exception handling to a single exception handler
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Status.NotReady);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Level2Error.StatusBytes);
            _Database.Update(signal.GetStatusWords());
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _Database.SetUiRequestOnRestartPresent(true);

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
        }

        return false;
    }

    #endregion

    #region S7.11 - S7.16

    /// <summary>
    /// TryResolveActiveRecords
    /// </summary>
    /// <param name="session"></param>
    /// <param name="rapdu"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryResolveActiveRecords(KernelSession session, ReadRecordResponse rapdu)
    {
        try
        {
            PrimitiveValue[] records = session.ResolveActiveTag(rapdu);
            _Database.Update(records);

            HandleUnpredictableNumberDataObjectList();

            _Database.Update(rapdu.GetPrimitiveDataObjects());

            return true;
        }
        catch (BerParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(session, rapdu);

            return false;
        }
        catch (CodecParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(session, rapdu);

            return false;
        }
        catch (TerminalDataException)
        {
            // TODO: Logging
            HandleBerParsingException(session, rapdu);

            return false;
        }
        catch (Exception)
        {
            // TODO: Logging
            HandleBerParsingException(session, rapdu);

            return false;
        }
    }

    #endregion

    #region S7.13.1 - S7.13.2

    /// <summary>
    /// HandleBerParsingException
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleBerParsingException(KernelSession session, QueryPcdResponse signal)
    {
        try
        {
            // HACK: Move exception handling to a single exception handler
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Status.NotReady);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Level2Error.ParsingError);
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _Database.SetUiRequestOnRestartPresent(true);
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
        }
    }

    #endregion

    #region S7.14 - S7.15

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private void HandleUnpredictableNumberDataObjectList()
    {
        if (!_Database.IsPresentAndNotEmpty(UnpredictableNumberDataObjectList.Tag))
            return;

        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded,
                                           _Database.Get<UnpredictableNumberDataObjectList>(UnpredictableNumberDataObjectList.Tag)
                                               .GetNeededData(_Database));
    }

    #endregion

    #region S7.17 - S7.19

    /// <remarks>Book C-2 Section S4.36</remarks>
    private bool IsReadingRequired(Kernel2Session session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            return false;

        _PcdEndpoint.Request(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));

        return true;
    }

    #endregion

    #region S7.20 - S7.21.2

    /// <summary>
    /// TryHandlingMissingMandatoryObjects
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandlingMissingMandatoryObjects(KernelSessionId sessionId)
    {
        try
        {
            if (!AreMandatoryDataObjectsPresent())
                return false;

            // HACK: Move exception handling to a single exception handler
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Status.NotReady);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Level2Error.CardDataMissing);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

            _KernelEndpoint.Request(new StopKernelRequest(sessionId));

            return true;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _KernelEndpoint.Request(new StopKernelRequest(sessionId));
        }

        return false;
    }

    #endregion

    #region S7.20

    /// <exception cref="TerminalDataException"></exception>
    private bool AreMandatoryDataObjectsPresent()
    {
        if (!_Database.IsPresentAndNotEmpty(Track2Data.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(PunatcTrack2.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(PositionOfCardVerificationCode3Track2.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(NumericApplicationTransactionCounterTrack2.Tag))
            return false;

        return true;
    }

    #endregion

    #region S7.22, S7.24.1 - S7.24.2

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsMagstripeDataOkay(KernelSessionId sessionId)
    {
        try
        {
            NumberOfNonZeroBits numberOfNonZeroBits = new(_Database.Get<PunatcTrack2>(PunatcTrack2.Tag),
                                                          _Database
                                                              .Get<
                                                                  NumericApplicationTransactionCounterTrack2>(NumericApplicationTransactionCounterTrack2
                                                                                                                  .Tag));

            if (!numberOfNonZeroBits.IsInRange())
            {
                HandleMagstripeDataIsInvalid(sessionId);

                return false;
            }

            if (!_Database.IsPresentAndNotEmpty(Track1Data.Tag))
                return true;

            if (!_Database.IsPresentAndNotEmpty(PositionOfCardVerificationCode3Track1.Tag))
            {
                HandleMagstripeDataIsInvalid(sessionId);

                return false;
            }

            if (!_Database.TryGet(NumericApplicationTransactionCounterTrack1.Tag,
                                  out NumericApplicationTransactionCounterTrack1? natcTrack1))
            {
                HandleMagstripeDataIsInvalid(sessionId);

                return false;
            }

            if (!_Database.TryGet(PunatcTrack1.Tag, out PunatcTrack1? punatcTrack1))
            {
                HandleMagstripeDataIsInvalid(sessionId);

                return false;
            }

            if (!numberOfNonZeroBits.IsInRange(punatcTrack1!, natcTrack1!))
            {
                HandleMagstripeDataIsInvalid(sessionId);

                return false;
            }
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _KernelEndpoint.Request(new StopKernelRequest(sessionId));
        }

        return true;
    }

    #endregion

    #region S7.24.1 - S7.24.2

    /// <summary>
    /// HandleMagstripeDataIsInvalid
    /// </summary>
    /// <param name="sessionId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleMagstripeDataIsInvalid(KernelSessionId sessionId)
    {
        try
        {
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Status.NotReady);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Level2Error.CardDataMissing);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.CreateMagstripeDiscretionaryData(_DataExchangeKernelService);
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
        }
        finally
        {
            _KernelEndpoint.Request(new StopKernelRequest(sessionId));
        }
    }

    #endregion

    #region S7.23

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private void UpdateDiscretionaryTrackData()
    {
        Track2DiscretionaryData track2DiscretionaryData = _Database.Get<Track2Data>(Track2Data.Tag).GetTrack2DiscretionaryData();
        _Database.Update(track2DiscretionaryData);

        if (!_Database.TryGet(Track1Data.Tag, out Track1Data? track1Data))
            return;

        _Database.Update(track1Data!.GetTrack1DiscretionaryData());
    }

    #endregion

    #endregion
}