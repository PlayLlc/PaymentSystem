﻿using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv.Primitives.Card;
using Play.Emv.DataElements.Emv.Primitives.DataStorage;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Icc.GetData;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Kernel.State;
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

        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
    }

    #region S4.4 - S4.6

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

    #region S4.28

    #endregion

    public void UpdateDataToSend(Tag[] resolvedRecords)
    {
        for (int i = 0; i < resolvedRecords.Length; i++)
        {
            if (resolvedRecords[i] == CardRiskManagementDataObjectList1.Tag)
                HandleCdol1(CardRiskManagementDataObjectList1.Decode(_KernelDatabase.Get(CardRiskManagementDataObjectList1.Tag)
                    .EncodeTagLengthValue().AsSpan()));

            if (resolvedRecords[i] == DataStorageDataObjectList.Tag)
                HandleDsdol();
        }
    }

    public void HandleCdol1(CardRiskManagementDataObjectList1 cdol)
    {
        _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, cdol.GetNeededData(_KernelDatabase));
    }

    public void HandleDsdol(DataStorageDataObjectList dsdol)
    { }

    #endregion
}