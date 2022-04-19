using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPutDataResponseBeforeGenerateAc
{
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        Kernel2Session kernel2Session = (Kernel2Session) session;
        PutDataResponse rapdu = (PutDataResponse) signal;

        if (TryHandleLevel1Error(session.GetKernelSessionId(), rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S12.9 - S12.11
        if (TryToHandleTagsToWriteYet(session.GetTransactionSessionId(), rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        // S12.12
        UpdatePreGenAcPutDataStatus(rapdu);

        // S12.13 - S12.14, S12.17 - S12.19
        if (TryRecoveringTornTransaction(kernel2Session))
            return _KernelStateResolver.GetKernelState(WaitingForRecoverAcResponse.StateId);

        // S12.15 - S12.16
        return _KernelStateResolver.GetKernelState(SendGenerateAcCapdu(kernel2Session, rapdu));
    }

    #region S12.1, S12.5 - S12.6

    /// <remarks>Book C-2 Section S12.1, S12.5 - S12.6</remarks>
    private bool TryHandleLevel1Error(KernelSessionId sessionId, PutDataResponse rapdu)
    {
        try
        {
            if (!rapdu.IsLevel1ErrorPresent())
                return false;

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

            _KernelEndpoint.Request(new StopKernelRequest(sessionId));

            return true;
        }
        catch (Exception)
        {
            // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception

            _KernelEndpoint.Request(new StopKernelRequest(sessionId));

            return true;
        }

        return true;
    }

    #endregion

    #region S12.9 - S12.11

    /// <remarks>Book C-2 Section S12.9 - S12.11</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    private bool TryToHandleTagsToWriteYet(TransactionSessionId sessionId, PutDataResponse rapdu)
    {
        if (rapdu.IsLevel2ErrorPresent())
            return false;

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.TagsToWriteBeforeGenAc))
            return false;

        HandleTagsToWrite(sessionId);

        return true;
    }

    #endregion

    #region S12.10 - S12.11

    /// <remarks>Book C-2 Section S12.10 - S12.11</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    private void HandleTagsToWrite(TransactionSessionId sessionId)
    {
        if (!_DataExchangeKernelService.TryPeek(DekResponseType.TagsToWriteBeforeGenAc, out PrimitiveValue? tagToWrite))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForPutDataResponseBeforeGenerateAc)} failed because the expected {DekResponseType.TagsToWriteBeforeGenAc} could not be dequeued");
        }

        PutDataRequest capdu = PutDataRequest.Create(sessionId, tagToWrite!);
        _PcdEndpoint.Request(capdu);
    }

    #endregion

    #region S12.12

    /// <remarks>Book C-2 Section S12.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdatePreGenAcPutDataStatus(PutDataResponse rapdu)
    {
        if (rapdu.IsLevel2ErrorPresent())
            return;

        _Database.Update(PreGenAcPutDataStatus.GetDefaultCompletedPreGenAcPutDataStatus());
    }

    #endregion

    #region S12.13 - S12.14, S12.17 - S12.19

    /// <remarks>Book C-2 Section S12.13 - S12.14, S12.17 - S12.19</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private bool TryRecoveringTornTransaction(Kernel2Session session)
    {
        if (!_Database.IsIdsAndTtrImplemented())
            return false;
        if (!_Database.IsTornTransactionRecoverySupported())
            return false;

        if (!_Database.IsPresentAndNotEmpty(DataRecoveryDataObjectList.Tag))
            return false;

        if ((byte) _Database.Get<MaxNumberOfTornTransactionLogRecords>(MaxNumberOfTornTransactionLogRecords.Tag) == 0)
            return false;

        if (_Database.IsPresentAndNotEmpty(ApplicationPanSequenceNumber.Tag))
            return false;

        if (!_Database.TryGet(
            new TornEntry(_Database.Get<ApplicationPan>(ApplicationPan.Tag), _Database.Get<ApplicationPanSequenceNumber>(ApplicationPanSequenceNumber.Tag)),
            out TornRecord? tornRecord))
            return false;

        session.Update(tornRecord!.GetKey());

        if (!_Database.TryGet(DataRecoveryDataObjectListRelatedData.Tag, out DataRecoveryDataObjectListRelatedData? ddolRelatedData))
            throw new TerminalDataException($"The {nameof(S456)} could not complete {nameof(TryRecoveringTornTransaction)} ");

        RecoverAcRequest capdu = RecoverAcRequest.Create(session.GetTransactionSessionId(), ddolRelatedData!);
        _PcdEndpoint.Request(capdu);

        return true;
    }

    #endregion

    #region S12.15 - S12.16

    /// <remarks>Book C-2 Section S12.15 - S12.16</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataException"></exception>
    private StateId SendGenerateAcCapdu(Kernel2Session session, PutDataResponse rapdu)
    {
        _PrepareGenAcService.Process(this, session, rapdu);

        return WaitingForGenerateAcResponse1.StateId;
    }

    #endregion
}