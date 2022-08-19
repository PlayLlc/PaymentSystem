﻿using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Exceptions;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

using IHandleKernelStopRequests = Play.Emv.Kernel.IHandleKernelStopRequests;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForEmvReadRecordResponse : KernelState
{
    #region Instance Values

    private readonly S456 _S456;

    #endregion

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
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

        return _KernelStateResolver.GetKernelState(_S456.Process(this, kernel2Session, signal));
    }

    #region S4.4 - S4.6

    /// <remarks>Book C-2 Section S4.4 - S4.6</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsLevel1ErrorPresent())
            return false;

        _Database.Update(MessageIdentifiers.TryAgain);
        _Database.Update(Statuses.ReadyToRead);
        _Database.Update(new MessageHoldTime(0));
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(StartOutcomes.B);
        _Database.SetUiRequestOnRestartPresent(true);
        _Database.Update(signal.GetLevel1Error());
        _Database.Update(MessageOnErrorIdentifiers.TryAgain);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _EndpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));

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

        _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Statuses.NotReady);
        _Database.Update(StatusOutcomes.EndApplication);
        _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        _Database.Update(Level2Error.StatusBytes);
        _Database.Update(signal.GetStatusWords());
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
        _Database.SetUiRequestOnOutcomePresent(true);

        _EndpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));

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
            throw new TerminalDataException(
                $"The state {nameof(WaitingForEmvModeFirstWriteFlag)} expected the {nameof(KernelSession)} to return a {nameof(RecordRange)} because the {nameof(ApplicationFileLocator)} indicated more files need to be read");
        }

        return result.GetOfflineDataAuthenticationLength() > 0;
    }

    #endregion

    #region S4.14, S4.24 - S4.25, S5.27.1 - S5.27.2

    /// <remarks>Book C-2 Section S4.14, S4.24 - S4.25, S5.27.1 - S5.27.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryResolveActiveRecords(KernelSession session, ReadRecordResponse rapdu, out Tag[] resolvedRecords)
    {
        try
        {
            EmvRuntimeCodec runtimeCodec = EmvRuntimeCodec.GetCodec();

            PrimitiveValue[] records = session.ResolveActiveTag(runtimeCodec, rapdu);

            _Database.Update(rapdu.GetPrimitiveDataObjects(runtimeCodec));

            resolvedRecords = records.Select(a => a.GetTag()).ToArray();

            return true;
        }
        catch (BerParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(session, _DataExchangeKernelService, _Database, _EndpointClient);
            resolvedRecords = Array.Empty<Tag>();

            return false;
        }
        catch (CodecParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(session, _DataExchangeKernelService, _Database, _EndpointClient);
            resolvedRecords = Array.Empty<Tag>();

            return false;
        }
        catch (Exception)
        {
            // TODO: Logging
            HandleBerParsingException(session, _DataExchangeKernelService, _Database, _EndpointClient);
            resolvedRecords = Array.Empty<Tag>();

            return false;
        }
    }

    /// <summary>
    ///     HandleBerParsingException
    /// </summary>
    /// <param name="session"></param>
    /// <param name="dataExchanger"></param>
    /// <param name="database"></param>
    /// <param name="kernelEndpoint"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private static void HandleBerParsingException(
        KernelSession session, DataExchangeKernelService dataExchanger, KernelDatabase database, IEndpointClient endpointClient)
    {
        database.Update(StatusOutcomes.EndApplication);
        database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
        database.Update(Level2Error.ParsingError);
        database.CreateEmvDiscretionaryData(dataExchanger);
        database.SetUiRequestOnOutcomePresent(true);

        endpointClient.Send(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion

    #region S4.28 - S4.33

    /// <remarks>Book C-2 Section S4.28 - S4.33</remarks>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void UpdateDataNeeded(Kernel2Session session, ReadRecordResponse rapdu, Tag[] resolvedRecords, bool isRecordSigned)
    {
        if (_Database.IsIntegratedDataStorageSupported())
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
    private void UpdateDataNeededWhenIdsIsNotSupported(Tag[] resolvedRecords)
    {
        for (int i = 0; i < resolvedRecords.Length; i++)
        {
            if (resolvedRecords[i] == CardRiskManagementDataObjectList1.Tag)
                HandleCdol1((CardRiskManagementDataObjectList1) _Database.Get(CardRiskManagementDataObjectList1.Tag));
        }
    }

    #endregion

    #region S4.28 - S4.30, S4.33

    /// <remarks>Book C-2 Section S4.28, S4.29, S4.33</remarks>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void UpdateDataNeededWhenIdsIsSupported(Kernel2Session session, ReadRecordResponse rapdu, Tag[] resolvedRecords, bool isRecordSigned)
    {
        if (!_Database.IsPresent(DataStorageRequestedOperatorId.Tag))
        {
            UpdateDataNeededWhenIdsIsNotSupported(resolvedRecords);

            return;
        }

        for (int i = 0; i < resolvedRecords.Length; i++)
        {
            if (resolvedRecords[i] == CardRiskManagementDataObjectList1.Tag)
                HandleCdol1((CardRiskManagementDataObjectList1) _Database.Get(CardRiskManagementDataObjectList1.Tag));

            if (resolvedRecords[i] == DataStorageDataObjectList.Tag)
                HandleDsdol(session, rapdu, isRecordSigned, (DataStorageDataObjectList) _Database.Get(DataStorageDataObjectList.Tag));
        }
    }

    #endregion

    #region S4.29

    /// <remarks>Book C-2 Section S4.29</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleCdol1(CardRiskManagementDataObjectList1 cdol)
    {
        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, cdol.GetNeededData(_Database));
    }

    #endregion

    #region S4.31

    /// <remarks>Book C-2 Section S4.32 - S4.33</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleDsdol(Kernel2Session session, ReadRecordResponse rapdu, bool isRecordSigned, DataStorageDataObjectList dsdol)
    {
        // BUG: I think this section is wrong
        if (!_Database.TryGet(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
        {
            AttemptToUpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

            return;
        }

        if (!((IntegratedDataStorageStatus) idsStatus!).IsReadSet())
        {
            AttemptToUpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

            return;
        }

        if (_Database.TryGet(DataStorageSlotManagementControl.Tag, out PrimitiveValue? dsSlotControl))
        {
            AttemptToUpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

            return;
        }

        if (((DataStorageSlotManagementControl) dsSlotControl!).IsLocked())
        {
            AttemptToUpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

            return;
        }

        EnqueueDsdolToDataNeeded(dsdol);
    }

    #endregion

    #region S4.33

    /// <summary>
    ///     EnqueueDsdolToDataNeeded
    /// </summary>
    /// <param name="dsdol"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void EnqueueDsdolToDataNeeded(DataStorageDataObjectList dsdol)
    {
        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, dsdol.GetNeededData(_Database));
    }

    #endregion

    #region S4.34 - S4.35

    /// <remarks>Book C-2 Section S4.34 - S4.35</remarks>
    /// <exception cref="Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    private static void AttemptToUpdateStaticDataToBeAuthenticated(Kernel2Session session, ReadRecordResponse rapdu, bool isRecordSigned)
    {
        if (!isRecordSigned)
            return;

        if (session.GetOdaStatus() != OdaStatusTypes.Cda)
            return;

        //  S4.35
        session.EnqueueStaticDataToBeAuthenticated(EmvCodec.GetCodec(), rapdu);
    }

    #endregion

    #region S4.36

    /// <remarks>Book C-2 Section S4.36</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsReadingRequired(Kernel2Session session)
    {
        if (!_Database.IsReadAllRecordsActivated())
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
        if (!_Database.IsPresentAndNotEmpty(ApplicationExpirationDate.Tag))
        {
            AttemptNextCommand(session);

            return;
        }

        if (!_Database.IsPresentAndNotEmpty(ApplicationPan.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(ApplicationPanSequenceNumber.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(ApplicationUsageControl.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(CvmList.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(IssuerActionCodeDefault.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(IssuerActionCodeDenial.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(IssuerActionCodeOnline.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(IssuerCountryCode.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(Track2EquivalentData.Tag))
            return;
        if (!_Database.IsPresentAndNotEmpty(CardRiskManagementDataObjectList1.Tag))
            return;

        session.ClearActiveTags();
    }

    #endregion

    #region S4.15 - S4.23

    /// <remarks>Book C-2 Section S4.15 - S4.23</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private void AttemptNextCommand(KernelSession session)
    {
        if (!TryHandleGetDataToBeDone(session.GetTransactionSessionId()))
            HandleRemainingApplicationFilesToRead(session);
    }

    #endregion

    #region S4.15 - S4.18

    /// <remarks>Book C-2 Section S4.15 - S4.18</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private bool TryHandleGetDataToBeDone(TransactionSessionId sessionId)
    {
        if (!_DataExchangeKernelService.TryPeek(DekRequestType.TagsToRead, out Tag tagToRead))
            return false;

        _EndpointClient.Send(GetDataRequest.Create(tagToRead, sessionId));

        return true;
    }

    #endregion

    #region S4.19 - S4.23

    /// <remarks>Book C-2 Section S4.19 - S4.23</remarks>
    private void HandleRemainingApplicationFilesToRead(KernelSession session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            return;

        _EndpointClient.Send(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));
    }

    #endregion
}