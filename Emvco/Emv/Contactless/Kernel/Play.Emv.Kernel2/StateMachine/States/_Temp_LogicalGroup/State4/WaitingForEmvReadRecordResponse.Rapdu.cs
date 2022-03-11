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
using Play.Emv.Icc.GetData;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Pcd;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup.State4;

public partial class WaitingForEmvReadRecordResponse : KernelState
{
    #region RAPDU

    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        bool isRecordSigned = false;

        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        if (TryHandleInvalidResultCode(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        IsOfflineDataAuthenticationRecordPresent(session, ref isRecordSigned);

        if (!TryResolveActiveRecords(session, (ReadRecordResponse) signal, out Tag[] resolvedRecords))
            return _KernelStateResolver.GetKernelState(StateId);

        UpdateDataNeeded((Kernel2Session) session, (ReadRecordResponse) signal, resolvedRecords, isRecordSigned);

        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
    }

    #region S4.4 - S4.6

    /// <remarks>Book C-2 Section S4.4 - S4.6</remarks>
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

    /// <remarks>Book C-2 Section S4.11 - S4.13</remarks>
    private void IsOfflineDataAuthenticationRecordPresent(KernelSession session, ref bool isRecordSigned)
    {
        if (!session.TryPeekActiveTag(out RecordRange result))
        {
            throw new TerminalDataException(
                $"The state {nameof(WaitingForEmvReadRecordResponse)} expected the {nameof(KernelSession)} to return a {nameof(RecordRange)} because the {nameof(ApplicationFileLocator)} indicated more files need to be read");
        }

        if (result.GetOfflineDataAuthenticationLength() > 0)
            isRecordSigned = true;

        isRecordSigned = false;
    }

    #endregion

    #region S4.14, S4.24 - S4.25, S5.27.1 - S5.27.2

    /// <remarks>Book C-2 Section S4.14, S4.24 - S4.25, S5.27.1 - S5.27.2</remarks>
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
                    DataStorageDataObjectList.Decode(_KernelDatabase.Get(DataStorageDataObjectList.Tag).EncodeTagLengthValue().AsSpan()));
            }
        }
    }

    #endregion

    #region S4.29

    /// <remarks>Book C-2 Section S4.29</remarks>
    public void HandleCdol1(CardRiskManagementDataObjectList1 cdol)
    {
        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, cdol.GetNeededData(_KernelDatabase));
    }

    #endregion

    #region S4.32 - S4.33

    /// <remarks>Book C-2 Section S4.32 - S4.33</remarks>
    public void HandleDsdol(Kernel2Session session, ReadRecordResponse rapdu, bool isRecordSigned, DataStorageDataObjectList dsdol)
    {
        if (!_KernelDatabase.TryGet(IntegratedDataStorageStatus.Tag, out TagLengthValue? idsStatus))
            UpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

        if (!IntegratedDataStorageStatus.Decode(idsStatus!.EncodeValue().AsSpan()).IsReadSet())
            UpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

        if (!_KernelDatabase.TryGet(DataStorageSlotManagementControl.Tag, out TagLengthValue? dsSlotControl))
            UpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

        if (!DataStorageSlotManagementControl.Decode(dsSlotControl!.EncodeTagLengthValue().AsSpan()).IsLocked())
            UpdateStaticDataToBeAuthenticated(session, rapdu, isRecordSigned);

        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, dsdol.GetNeededData(_KernelDatabase));
    }

    #endregion

    #region S4.34 - S4.35

    /// <remarks>Book C-2 Section S4.34 - S4.35</remarks>
    private void UpdateStaticDataToBeAuthenticated(Kernel2Session session, ReadRecordResponse rapdu, bool isRecordSigned)
    {
        if (!isRecordSigned)
        {
            S436(session);

            return;
        }

        if (session.GetOdaStatus() != OdaStatusTypes.Cda)
        {
            S436(session);

            return;
        }

        S435(session, rapdu);
    }

    private void S435(Kernel2Session session, ReadRecordResponse rapdu)
    {
        session.EnqueueStaticDataToBeAuthenticated(EmvCodec.GetBerCodec(), rapdu);
    }

    private void S436(Kernel2Session session)
    {
        if (_KernelDatabase.IsReadAllRecordsActivated())
            SetNextCommand();

        if (session.GetOdaStatus() != OdaStatusTypes.Cda)
            SetNextCommand();

        OptimizeRead(session);
        SetNextCommand();
        S456.Process();
    }

    private void OptimizeRead(KernelSession session)
    {
        // TODO: Check if you have the minimum required shit to process without offline auth
        session.ClearActiveTags();
    }

    private void SetNextCommand()
    { }

    #endregion

    #endregion
}