using System;

using Play.Ber.DataObjects;
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

        GenerateApplicationCryptogramResponse rapdu = (GenerateApplicationCryptogramResponse) signal;
        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (TryHandlingL1Error(kernel2Session, rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        throw new NotImplementedException();
    }

    #region L1 Error

    #region S11.1, S11.11 - 13, S11.15 - S11.17

    /// <remarks>Book C-2 Section S11.1, S11.11 - 13, S11.15 - S11.17</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private bool TryHandlingL1Error(Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
    {
        if (!rapdu.IsLevel1ErrorPresent())
            return false;

        if (!session.TryGetTornEntry(out TornEntry? tornEntry))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornEntry)} could not be retrieved from the {nameof(Kernel2Session)}");
        }

        HandleIdsReadFlagSet(session, tornEntry!);

        PrepareNewTornRecord();
        HandleLevel1Response(session.GetKernelSessionId(), rapdu);

        return true;
    }

    #endregion

    #region S11.11 - S11.12

    /// <remarks>Book C-2 Section S11.11 - S11.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void HandleIdsReadFlagSet(Kernel2Session session, TornEntry tornEntry)
    {
        if (!_TornTransactionLog.TryGet(tornEntry!, out TornRecord? tornTempRecord))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornRecord)} could not be retrieved from the {nameof(TornTransactionLog)}");
        }

        if (tornTempRecord!.TryGetRecordItem(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
            Remove(tornTempRecord);

        // S11.12
        if (!((IntegratedDataStorageStatus) idsStatus!).IsWriteSet())
            Remove(tornTempRecord);
    }

    #endregion

    #region S11.11 - S11.12 continued

    /// <remarks>Book C-2 Section S11.11 - S11.12</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void Remove(TornRecord tornRecord)
    {
        _DataExchangeKernelService.Enqueue(DekResponseType.TornRecord, tornRecord);
        _TornTransactionLog.Remove(tornRecord.GetKey());
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
    private void HandleLevel1Response(KernelSessionId sessionId, GenerateApplicationCryptogramResponse rapdu)
    {
        try
        {
            _Database.Update(MessageIdentifiers.TryAgain);
            _Database.Update(Status.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcome.B);
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

    #endregion
}