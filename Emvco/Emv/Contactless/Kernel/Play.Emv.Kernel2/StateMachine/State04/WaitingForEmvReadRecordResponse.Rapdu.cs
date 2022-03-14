using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForEmvReadRecordResponse : KernelState
{
    #region Instance Values

    private readonly CommonProcessingS456 _S456;

    #endregion

    #region RAPDU

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="Play.Emv.Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        Kernel2Session kernel2Session = (Kernel2Session) session;
        ReadRecordResponse rapdu = (ReadRecordResponse) signal;
        bool isRecordSigned = false;

        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandleInvalidResultCode(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        isRecordSigned = IsOfflineDataAuthenticationRecordPresent(session);

        if (!TryResolveActiveRecords(session, rapdu, out Tag[] resolvedRecords))
            return _KernelStateResolver.GetKernelState(StateId);

        UpdateDataNeeded(kernel2Session, rapdu, resolvedRecords, isRecordSigned);

        AttemptToUpdateStaticDataToBeAuthenticated(kernel2Session, rapdu, isRecordSigned);

        if (!IsReadingRequired(kernel2Session))
            OptimizeRead(session);

        AttemptNextCommand(session);

        return _S456.Process(this, kernel2Session);
    }

    #region S4.4 - S4.6

    /// <remarks>Book C-2 Section S4.4 - S4.6</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsSuccessful())
            return false;

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

    #region S4.9 & S4.10.1 - S4.10.2

    /// <remarks>Book C-2 Section S4.9 & S4.10.1 - S4.10.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    #region S4.11 - S4.13

    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>Book C-2 Section S4.11 - S4.13</remarks>
    private bool IsOfflineDataAuthenticationRecordPresent(KernelSession session)
    {
        if (!session.TryPeekActiveTag(out RecordRange result))
        {
            throw new
                TerminalDataException($"The state {nameof(WaitingForEmvModeFirstWriteFlag)} expected the {nameof(KernelSession)} to return a {nameof(RecordRange)} because the {nameof(ApplicationFileLocator)} indicated more files need to be read");
        }

        return result.GetOfflineDataAuthenticationLength() > 0;
    }

    #endregion

    #region S4.14, S4.24 - S4.25, S5.27.1 - S5.27.2

    /// <remarks>Book C-2 Section S4.14, S4.24 - S4.25, S5.27.1 - S5.27.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    public bool TryResolveActiveRecords(KernelSession session, ReadRecordResponse rapdu, out Tag[] resolvedRecords)
    {
        try
        {
            TagLengthValue[] records = session.ResolveActiveTag(rapdu);

            _KernelDatabase.Update(records);

            resolvedRecords = records.Select(a => a.GetTag()).ToArray();

            return true;
        }
        catch (EmvParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(session, _DataExchangeKernelService, _KernelDatabase, _KernelEndpoint);
            resolvedRecords = Array.Empty<Tag>();

            return false;
        }
        catch (BerParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(session, _DataExchangeKernelService, _KernelDatabase, _KernelEndpoint);
            resolvedRecords = Array.Empty<Tag>();

            return false;
        }
        catch (CodecParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(session, _DataExchangeKernelService, _KernelDatabase, _KernelEndpoint);
            resolvedRecords = Array.Empty<Tag>();

            return false;
        }
        catch (Exception)
        {
            // TODO: Logging
            HandleBerParsingException(session, _DataExchangeKernelService, _KernelDatabase, _KernelEndpoint);
            resolvedRecords = Array.Empty<Tag>();

            return false;
        }
    }

    /// <summary>
    /// HandleBerParsingException
    /// </summary>
    /// <param name="session"></param>
    /// <param name="dataExchanger"></param>
    /// <param name="database"></param>
    /// <param name="kernelEndpoint"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private static void HandleBerParsingException(
        KernelSession session,
        DataExchangeKernelService dataExchanger,
        KernelDatabase database,
        IKernelEndpoint kernelEndpoint)
    {
        database.Update(StatusOutcome.EndApplication);
        database.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
        database.Update(Level2Error.ParsingError);
        database.CreateEmvDiscretionaryData(dataExchanger);
        database.SetUiRequestOnRestartPresent(true);

        kernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #region S4.28 - S4.33

    /// <remarks>Book C-2 Section S4.28 - S4.33</remarks>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void UpdateDataNeeded(Kernel2Session session, ReadRecordResponse rapdu, Tag[] resolvedRecords, bool isRecordSigned)
    {
        if (_KernelDatabase.IsIntegratedDataStorageSupported())
            UpdateDataNeededWhenIdsIsSupported(session, rapdu, resolvedRecords, isRecordSigned);
        else
            UpdateDataNeededWhenIdsIsNotSupported(resolvedRecords);
    }

    #endregion

    #region S4.28,  S4.29

    /// <remarks>Book C-2 Section S4.28,  S4.29</remarks>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void UpdateDataNeededWhenIdsIsNotSupported(Tag[] resolvedRecords)
    {
        for (int i = 0; i < resolvedRecords.Length; i++)
        {
            if (resolvedRecords[i] == CardRiskManagementDataObjectList1.Tag)
            {
                HandleCdol1(CardRiskManagementDataObjectList1.Decode(_KernelDatabase.Get(CardRiskManagementDataObjectList1.Tag)
                                                                         .EncodeTagLengthValue().AsSpan()));
            }
        }
    }

    #endregion

    #region S4.28, S4.29, S4.33

    /// <remarks>Book C-2 Section S4.28, S4.29, S4.33</remarks>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Emv.Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    public void UpdateDataNeededWhenIdsIsSupported(
        Kernel2Session session,
        ReadRecordResponse rapdu,
        Tag[] resolvedRecords,
        bool isRecordSigned)
    {
        if (!_KernelDatabase.IsPresent(DataStorageRequestedOperatorId.Tag))
        {
            UpdateDataNeededWhenIdsIsNotSupported(resolvedRecords);

            return;
        }

        for (int i = 0; i < resolvedRecords.Length; i++)
        {
            if (resolvedRecords[i] == CardRiskManagementDataObjectList1.Tag)
            {
                HandleCdol1(CardRiskManagementDataObjectList1.Decode(_KernelDatabase.Get(CardRiskManagementDataObjectList1.Tag)
                                                                         .EncodeTagLengthValue().AsSpan()));
            }

            if (resolvedRecords[i] == DataStorageDataObjectList.Tag)
            {
                HandleDsdol(session, rapdu, isRecordSigned,
                            DataStorageDataObjectList.Decode(_KernelDatabase.Get(DataStorageDataObjectList.Tag).EncodeTagLengthValue()
                                                                 .AsSpan()));
            }
        }
    }

    #endregion

    #region S4.29

    /// <remarks>Book C-2 Section S4.29</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public void HandleCdol1(CardRiskManagementDataObjectList1 cdol)
    {
        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, cdol.GetNeededData(_KernelDatabase));
    }

    #endregion

    #region S4.32 - S4.33

    /// <remarks>Book C-2 Section S4.32 - S4.33</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Play.Emv.Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public void HandleDsdol(Kernel2Session session, ReadRecordResponse rapdu, bool isRecordSigned, DataStorageDataObjectList dsdol)
    {
        if (!_KernelDatabase.TryGet(IntegratedDataStorageStatus.Tag, out TagLengthValue? idsStatus))
            AttemptToUpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

        if (!IntegratedDataStorageStatus.Decode(idsStatus!.EncodeValue().AsSpan()).IsReadSet())
            AttemptToUpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

        if (!_KernelDatabase.TryGet(DataStorageSlotManagementControl.Tag, out TagLengthValue? dsSlotControl))
            AttemptToUpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

        if (!DataStorageSlotManagementControl.Decode(dsSlotControl!.EncodeTagLengthValue().AsSpan()).IsLocked())
            AttemptToUpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, dsdol.GetNeededData(_KernelDatabase));
    }

    #endregion

    #region S4.34 - S4.35

    /// <remarks>Book C-2 Section S4.34 - S4.35</remarks>
    /// <exception cref="Play.Emv.Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    private void AttemptToUpdateStaticDataToBeAuthenticated(Kernel2Session session, ReadRecordResponse rapdu, bool isRecordSigned)
    {
        if (!isRecordSigned)
            return;

        if (session.GetOdaStatus() != OdaStatusTypes.Cda)
            return;

        //  S4.35
        session.EnqueueStaticDataToBeAuthenticated(EmvCodec.GetBerCodec(), rapdu);
    }

    #endregion

    #region S4.36

    /// <remarks>Book C-2 Section S4.36</remarks>
    private bool IsReadingRequired(Kernel2Session session)
    {
        if (!_KernelDatabase.IsReadAllRecordsActivated())
            return true;

        if (session.GetOdaStatus() != OdaStatusTypes.Cda)
            return true;

        return false;
    }

    #endregion

    #region S4.37 - S4.38

    /// <remarks>Book C-2 Section S4.37 - S4.38</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void OptimizeRead(KernelSession session)
    {
        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationExpirationDate.Tag))
        {
            AttemptNextCommand(session);

            return;
        }

        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationPan.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationPanSequenceNumber.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(ApplicationUsageControl.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(CvmList.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(IssuerActionCodeDefault.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(IssuerActionCodeDenial.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(IssuerActionCodeOnline.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(IssuerCountryCode.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(Track2EquivalentData.Tag))
            return;
        if (!_KernelDatabase.IsPresentAndNotEmpty(CardRiskManagementDataObjectList1.Tag))
            return;

        session.ClearActiveTags();
    }

    #endregion

    #region S4.15 - S4.23

    /// <remarks>Book C-2 Section S4.15 - S4.23</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public void AttemptNextCommand(KernelSession session)
    {
        if (!TryHandleGetDataToBeDone(session.GetTransactionSessionId()))
            HandleRemainingApplicationFilesToRead(session);
    }

    #endregion

    #region S4.15 - S4.18

    /// <remarks>Book C-2 Section S4.15 - S4.18</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryHandleGetDataToBeDone(TransactionSessionId sessionId)
    {
        if (!_DataExchangeKernelService.TryPeek(DekRequestType.TagsToRead, out Tag tagToRead))
            return false;

        _PcdEndpoint.Request(GetDataRequest.Create(tagToRead, sessionId));

        return true;
    }

    #endregion

    #region S4.19 - S4.23

    /// <remarks>Book C-2 Section S4.19 - S4.23</remarks>
    public void HandleRemainingApplicationFilesToRead(KernelSession session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            return;

        _PcdEndpoint.Request(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));
    }

    #endregion

    #endregion
}