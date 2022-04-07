using System;

using Play.Emv.Ber;
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
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse1
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
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        GenerateApplicationCryptogramResponse rapdu = (GenerateApplicationCryptogramResponse) signal;

        if (TryHandleL1Error(session.GetKernelSessionId(), signal))
            return _KernelStateResolver.GetKernelState(StateId);

        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (TryHandleLevel2StatusByteError(kernel2Session, rapdu, out StateId? stateIdForStatusByteErrorFlow))
            return _KernelStateResolver.GetKernelState(stateIdForStatusByteErrorFlow!.Value);

        if (TryHandleLevel2ParsingError(kernel2Session, rapdu, out StateId? stateIdForParsingErrorFlow))
            return _KernelStateResolver.GetKernelState(stateIdForParsingErrorFlow!.Value);

        if (TryHandleMissingMandatoryDataObjects(kernel2Session, rapdu, out StateId? stateIdForMissingMandatoryDataObjectsFlow))
            return _KernelStateResolver.GetKernelState(stateIdForMissingMandatoryDataObjectsFlow!.Value);

        if (TryHandleInvalidCryptogramInformationData(kernel2Session, rapdu, out StateId? stateIdForInvalidCryptogramInformationDataFlow))
            return _KernelStateResolver.GetKernelState(stateIdForInvalidCryptogramInformationDataFlow!.Value);

        return _KernelStateResolver.GetKernelState(HandleAuthentication(kernel2Session, rapdu));
    }

    #region S9.5 - S9.15 - L1RSP

    /// <exception cref="TerminalDataException"></exception>
    public bool TryHandleL1Error(KernelSessionId sessionId, QueryPcdResponse signal)
    {
        if (signal.IsSuccessful())
            return false;

        if (!IsTransactionRecoverySupported())
            HandleL1Error(sessionId, signal);
        else
            HandleTornTransaction(sessionId, signal);

        return true;
    }

    #endregion

    #region S9.5

    /// <exception cref="TerminalDataException"></exception>
    private bool IsTransactionRecoverySupported()
    {
        if (!_Database.IsIdsAndTtrImplemented())
            return false;
        if (!_Database.IsPresentAndNotEmpty(MaxNumberOfTornTransactionLogRecords.Tag))
            return false;
        if (!_Database.IsPresentAndNotEmpty(DataRecoveryDataObjectList.Tag))
            return false;

        return true;
    }

    #endregion

    #region S9.6 - S9.10

    /// <exception cref="TerminalDataException"></exception>
    private void HandleL1Error(KernelSessionId sessionId, QueryPcdResponse signal)
    {
        if (!_Database.IsIdsAndTtrImplemented() || !_Database.IsIntegratedDataStorageWriteFlagSet())
            HandleL1ErrorTryAgain(sessionId, signal);
        else
            HandleL1ErrorTryAnotherCard(sessionId, signal);
    }

    #endregion

    #region S9.7 - S9.8

    /// <exception cref="TerminalDataException"></exception>
    private void HandleL1ErrorTryAnotherCard(KernelSessionId sessionId, QueryPcdResponse signal)
    {
        try
        {
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Status.NotReady);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
            _Database.Update(signal.GetLevel1Error());
            _Database.SetIsDataRecordPresent(true);
            _Database.CreateEmvDataRecord(_DataExchangeKernelService);
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _Database.SetUiRequestOnRestartPresent(true);
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

    #region S9.9 - S9.10, S9.14 - S9.15

    /// <exception cref="TerminalDataException"></exception>
    private void HandleL1ErrorTryAgain(KernelSessionId sessionId, QueryPcdResponse signal)
    {
        try
        {
            // HACK: Move exception handling to a single exception handler
            _Database.Update(MessageIdentifiers.TryAgain);
            _Database.Update(Status.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);

            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcome.B);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.Update(signal.GetLevel1Error());
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

    #region S9.11 - S9.15

    /// <exception cref="TerminalDataException"></exception>
    private void HandleTornTransaction(KernelSessionId sessionId, QueryPcdResponse signal)
    {
        DataRecoveryDataObjectListRelatedData? drdol = _Database.Get<DataRecoveryDataObjectList>(DataRecoveryDataObjectList.Tag)
            .AsRelatedData(_Database);
        if (_TornTransactionManager.TryAddAndDisplace(_Database, out TornRecord? displacedRecord))
            _Database.Update(displacedRecord!);

        HandleL1ErrorTryAgain(sessionId, signal);
    }

    #endregion

    #region S916 - S917

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandleLevel2StatusByteError(Kernel2Session session, GenerateApplicationCryptogramResponse rapdu, out StateId? stateId)
    {
        if (rapdu.GetStatusWords() == StatusWords._9000)
        {
            stateId = null;

            return false;
        }

        SetLevel2StatusByteError(rapdu);

        stateId = _S910.Process(this, session, rapdu);

        return true;
    }

    #endregion

    #region S917

    /// <exception cref="TerminalDataException"></exception>
    private void SetLevel2StatusByteError(GenerateApplicationCryptogramResponse rapdu)
    {
        _Database.Update(Level2Error.StatusBytes);
        _Database.Update(rapdu.GetStatusWords());
    }

    #endregion

    #region S918 - S920

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandleLevel2ParsingError(Kernel2Session session, GenerateApplicationCryptogramResponse rapdu, out StateId? stateId)
    {
        try
        {
            _Database.Update(rapdu.GetPrimitiveDataObjects(_Database));
            stateId = null;

            return false;
        }
        catch (TerminalDataException)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            stateId = HandleLevel2ParsingError(session, rapdu);

            return true;
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
            stateId = HandleLevel2ParsingError(session, rapdu);

            return true;
        }
    }

    #endregion

    #region S920

    /// <exception cref="TerminalDataException"></exception>
    private StateId HandleLevel2ParsingError(Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
    {
        _Database.Update(Level2Error.ParsingError);

        return _S910.Process(this, session, rapdu);
    }

    #endregion

    #region S921 - S922

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandleMissingMandatoryDataObjects(
        Kernel2Session session, GenerateApplicationCryptogramResponse rapdu, out StateId? stateId)
    {
        if (!_Database.IsPresentAndNotEmpty(ApplicationTransactionCounter.Tag))
        {
            stateId = HandleMissingMandatoryDataObjects(session, rapdu);

            return true;
        }

        if (!_Database.IsPresentAndNotEmpty(CryptogramInformationData.Tag))
        {
            stateId = HandleMissingMandatoryDataObjects(session, rapdu);

            return true;
        }

        stateId = null;

        return false;
    }

    #endregion

    #region S922

    /// <exception cref="TerminalDataException"></exception>
    private StateId HandleMissingMandatoryDataObjects(Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
    {
        _Database.Update(Level2Error.CardDataMissing);

        return _S910.Process(this, session, rapdu);
    }

    #endregion

    #region S923 - S924

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandleInvalidCryptogramInformationData(
        Kernel2Session session, GenerateApplicationCryptogramResponse rapdu, out StateId? stateId)
    {
        CryptogramInformationData cid = _Database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

        if (!cid.IsValid(_Database))
        {
            stateId = HandleInvalidCryptogramInformationData(session, rapdu);

            return true;
        }

        stateId = null;

        return false;
    }

    #endregion

    #region S924

    private StateId HandleInvalidCryptogramInformationData(Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
    {
        _Database.Update(Level2Error.CardDataError);

        return _S910.Process(this, session, rapdu);
    }

    #endregion

    #region S925 - S928

    /// <exception cref="TerminalDataException"></exception>
    private StateId HandleAuthentication(Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
    {
        _BalanceReader.Process(this, session, rapdu);

        if (!IsPosGenAcWriteNeeded())
            SetDisplayMessage();

        // S928 is executed in the common object S910
        return _S910.Process(this, session, rapdu);
    }

    #endregion

    #region S926

    /// <exception cref="TerminalDataException"></exception>
    private bool IsPosGenAcWriteNeeded() => _DataExchangeKernelService.IsEmpty(DekResponseType.TagsToWriteAfterGenAc);

    #endregion

    #region S927

    /// <exception cref="TerminalDataException"></exception>
    private void SetDisplayMessage()
    {
        _Database.Update(MessageIdentifiers.ClearDisplay);
        _Database.Update(Status.CardReadSuccessful);
        _Database.Update(MessageHoldTime.MinimumValue);

        _DisplayEndpoint.Request(new DisplayMessageRequest(_Database.GetUserInterfaceRequestData()));
    }

    #endregion

    #region S928

    // S928 is executed within the S910 common object

    #endregion

    #endregion
}