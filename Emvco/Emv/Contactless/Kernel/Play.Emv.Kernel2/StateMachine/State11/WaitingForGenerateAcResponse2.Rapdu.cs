using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse2
{
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        RecoverAcResponse rapdu = (RecoverAcResponse) signal;
        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (kernel2Session.TryGetTornEntry(out TornEntry? result))
        {
            throw new TerminalDataException(
                $"The {nameof(AuthHandler)} could not {nameof(WaitingForGenerateAcResponse2)} because the expected {nameof(TornEntry)} could not be retrieved from the {nameof(Kernel2Session)}");
        }

        if (!_Database.TryGet(result!, out TornRecord? tempTornRecord))
        {
            throw new TerminalDataException(
                $"The {nameof(AuthHandler)} could not {nameof(WaitingForGenerateAcResponse2)} because the expected temporary {nameof(TornRecord)} could not be retrieved from the {nameof(TornTransactionLog)}");
        }

        // S11.1, S11.11 - S11.17
        if (TryHandlingL1Error(kernel2Session, rapdu, tempTornRecord!))
            return _KernelStateResolver.GetKernelState(StateId);

        // S11.5 - S11.10
        if (TryHandlingL2Error(kernel2Session, rapdu, tempTornRecord!))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandlingMissingCardData(kernel2Session, tempTornRecord!))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandlingCardDataError(kernel2Session, tempTornRecord!))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandlingCardDataError(kernel2Session, tempTornRecord!))
            return _KernelStateResolver.GetKernelState(StateId);

        // S11.22 - S11.24
        HandleBalanceReading(kernel2Session, rapdu);

        return _KernelStateResolver.GetKernelState(ProcessCardholderAuthenticationMethod(kernel2Session, rapdu, tempTornRecord!));
    }

    #region S11.1, S11.11 - S11.17

    /// <remarks>Book C-2 Section S11.1, S11.11 - S11.17</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private bool TryHandlingL1Error(Kernel2Session session, RecoverAcResponse rapdu, TornRecord tempTornRecord)
    {
        if (!rapdu.IsLevel1ErrorPresent())
            return false;

        if (!session.TryGetTornEntry(out TornEntry? tornEntry))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornEntry)} could not be retrieved from the {nameof(Kernel2Session)}");
        }

        HandleIdsWriteFlagSet(tornEntry!);

        PrepareNewTornRecord();
        HandleLevel1Response(session.GetKernelSessionId(), rapdu);

        return true;
    }

    #endregion

    #region S11.11 - S11.12

    /// <remarks>Book C-2 Section S11.11 - S11.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleIdsWriteFlagSet(TornEntry tornEntry)
    {
        if (!_TornTransactionLog.TryGet(tornEntry!, out TornRecord? tornTempRecord))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornRecord)} could not be retrieved from the {nameof(TornTransactionLog)}");
        }

        if (tornTempRecord!.TryGetRecordItem(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
            _TornTransactionLog.Remove(_DataExchangeKernelService, tornEntry); // S11.12 

        if (!((IntegratedDataStorageStatus) idsStatus!).IsWriteSet())
            _TornTransactionLog.Remove(_DataExchangeKernelService, tornEntry); // S11.12
    }

    #endregion

    #region S11.13, S11.15

    /// <remarks>Book C-2 Section S11.13, S11.15</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private void PrepareNewTornRecord()
    {
        if (!_Database.TryGet(DataRecoveryDataObjectList.Tag, out DataRecoveryDataObjectList? drDol))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not  create a new {nameof(TornRecord)} because the  {nameof(DataRecoveryDataObjectList)} could not be retrieved from the {nameof(KernelDatabase)}");
        }

        DataRecoveryDataObjectListRelatedData drDolRelatedData = drDol!.AsRelatedData(_Database);

        _Database.Update(drDolRelatedData);

        _TornTransactionLog.Add(TornRecord.Create(_Database), _Database);
    }

    #endregion

    #region S11.16 - S11.17

    /// <remarks>Book C-2 Section S11.16 - S11.17</remarks>
    private void HandleLevel1Response(KernelSessionId sessionId, RecoverAcResponse rapdu)
    {
        try
        {
            _Database.Update(MessageIdentifiers.TryAgain);
            _Database.Update(Statuses.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcomes.B);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.Update(rapdu.GetLevel1Error());
            _Database.Update(MessageOnErrorIdentifiers.TryAgain);
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
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
            _KernelEndpoint.Request(new StopKernelRequest(sessionId));
        }
    }

    #endregion

    #region S11.5 - S11.10

    /// <remarks>Book C-2 Section S11.5 - S11.10</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingL2Error(Kernel2Session session, RecoverAcResponse rapdu, TornRecord tempTornRecord)
    {
        // S11.5
        RemoveTornEntryFrom(session);

        // S11.6
        if (rapdu.IsLevel2ErrorPresent())
        {
            // S11.7
            HandleLStatusBytesError(session.GetKernelSessionId(), tempTornRecord);

            return true;
        }

        if (TryHandlingBerParsingError(session.GetKernelSessionId(), rapdu, tempTornRecord))
            return true;

        return false;
    }

    #endregion

    #region S11.5

    /// <remarks>Book C-2 Section S11.5</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void RemoveTornEntryFrom(Kernel2Session session)
    {
        if (!session.TryGetTornEntry(out TornEntry? tornEntry))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornEntry)} could not be retrieved from the {nameof(Kernel2Session)}");
        }

        _TornTransactionLog.Remove(_DataExchangeKernelService, tornEntry!);
    }

    #endregion

    #region S11.7

    /// <remarks>Book C-2 Section S11.7</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleLStatusBytesError(KernelSessionId sessionId, TornRecord tempTornRecord)
    {
        _Database.Update(Level2Error.StatusBytes);
        _ResponseHandler.ProcessInvalidDataResponse(sessionId, tempTornRecord);
    }

    #endregion

    #region S11.8 - S11.10

    /// <remarks>Book C-2 Section S11.8 - S11.10</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private bool TryHandlingBerParsingError(KernelSessionId sessionId, RecoverAcResponse rapdu, TornRecord tempTornRecord)
    {
        try
        {
            _Database.Update(rapdu.GetPrimitiveDataObjects());

            return false;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingError(sessionId, tempTornRecord);

            return true;
        }
        catch (BerParsingException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingError(sessionId, tempTornRecord);

            return true;
        }
        catch (CodecParsingException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingError(sessionId, tempTornRecord);

            return true;
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            HandleBerParsingError(sessionId, tempTornRecord);

            return true;
        }
    }

    #endregion

    #region S11.10

    /// <remarks>Book C-2 Section S11.10</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleBerParsingError(KernelSessionId sessionId, TornRecord tempTornRecord)
    {
        _Database.Update(Level2Error.ParsingError);
        _ResponseHandler.ProcessInvalidDataResponse(sessionId, tempTornRecord);
    }

    #endregion

    #region S11.18 - S11.19

    /// <remarks>Book C-2 Section S11.18 - S11.19</remarks>
    private bool TryHandlingMissingCardData(Kernel2Session session, TornRecord tempTornRecord)
    {
        if (_Database.IsPresentAndNotEmpty(ApplicationTransactionCounter.Tag))
            return false;

        _ResponseHandler.ProcessInvalidDataResponse(session.GetKernelSessionId(), tempTornRecord);

        return true;
    }

    #endregion

    #region S11.20 - S11.21

    /// <remarks>Book C-2 Section S11.20 - S11.21</remarks>
    private bool TryHandlingCardDataError(Kernel2Session session, TornRecord tempTornRecord)
    {
        if (!IsCryptogramInformationDataValid())
            return false;

        _Database.Update(Level2Error.CardDataError);
        _ResponseHandler.ProcessInvalidDataResponse(session.GetKernelSessionId(), tempTornRecord);

        return true;
    }

    #endregion

    #region S11.20

    /// <remarks>Book C-2 Section S11.20</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private bool IsCryptogramInformationDataValid()
    {
        if (_Database.TryGet(CryptogramInformationData.Tag, out CryptogramInformationData? cid))
            return false;

        if (!cid!.IsValid(_Database))
            return false;

        return true;
    }

    #endregion

    #region S11.22 - S11.24

    /// <remarks>Book C-2 Section S11.22 - S11.24</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleBalanceReading(Kernel2Session session, RecoverAcResponse rapdu)
    {
        _ = _BalanceReader.Process(this, session, rapdu);

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.TagsToWriteAfterGenAc))
            return;

        SetDisplayMessage();
    }

    #endregion

    #region S11.24

    /// <remarks>Book C-2 Section S11.24</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void SetDisplayMessage()
    {
        _Database.Update(MessageIdentifiers.ClearDisplay);
        _Database.Update(Statuses.CardReadSuccessful);
        _Database.Update(MessageHoldTime.MinimumValue);
        _DisplayEndpoint.Request(new DisplayMessageRequest(_Database.GetUserInterfaceRequestData()));
    }

    #endregion

    #region S11.25

    /// <remarks>Book C-2 Section S11.25</remarks>
    private StateId ProcessCardholderAuthenticationMethod(Kernel2Session session, RecoverAcResponse rapdu, TornRecord tempTornRecord)
    {
        if (_Database.IsPresentAndNotEmpty(SignedDynamicApplicationData.Tag))
            return _AuthHandler.ProcessWithCda(session, rapdu, tempTornRecord);

        return _AuthHandler.ProcessWithoutCda(session, tempTornRecord);
    }

    #endregion
}