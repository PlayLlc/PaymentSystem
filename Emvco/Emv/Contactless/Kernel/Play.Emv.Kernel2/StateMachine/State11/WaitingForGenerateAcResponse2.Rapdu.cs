using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse2
{
    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        GenerateApplicationCryptogramResponse rapdu = (GenerateApplicationCryptogramResponse) signal;
        Kernel2Session kernel2Session = (Kernel2Session) session;

        if (TryHandlingL1Error(session.GetKernelSessionId(), rapdu))
            return _KernelStateResolver.GetKernelState(StateId);

        throw new NotImplementedException();
    }

    #region L1 Error

    /// <exception cref="TerminalDataException"></exception>
    private bool TryHandlingL1Error(Kernel2Session session, QueryPcdResponse signal)
    {
        if (!signal.IsLevel1ErrorPresent())
            return false;

        if (!session.TryGetTornEntry(out TornEntry? tornEntry))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornEntry)} could not be retrieved from the {nameof(Kernel2Session)}");
        }

        // S11.11
        if (IsIdsWriteFlagSet(session, tornEntry!))
            _TornTransactionLog.Remove(tornEntry!); // S11.12
    }

    #endregion

    #region S11.11

    /// <exception cref="TerminalDataException"></exception>
    private bool HandleIdsReadFlagSet(Kernel2Session session, TornEntry tornEntry)
    {
        if (!_TornTransactionLog.TryGet(tornEntry!, out TornRecord? tornTempRecord))
        {
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not complete processing because the {nameof(TornRecord)} could not be retrieved from the {nameof(TornTransactionLog)}");
        }

        if (!tornTempRecord!.TryGetRecordItem(IntegratedDataStorageStatus.Tag, out PrimitiveValue? idsStatus))
            return false;

        return ((IntegratedDataStorageStatus) idsStatus!).IsWriteSet();
    }

    #endregion

    #region S11.13, S11.15

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private void PrepareNewTornRecord()
    {
        if (!_Database.TryGet(DataRecoveryDataObjectList.Tag, out DataRecoveryDataObjectList? drDol))
            throw new TerminalDataException(
                $"The {nameof(WaitingForGenerateAcResponse2)} could not  create a new {nameof(TornRecord)} because the  {nameof(DataRecoveryDataObjectList)} could not be retrieved from the {nameof(KernelDatabase)}");

        DataRecoveryDataObjectListRelatedData drDolRelatedData = drDol!.AsRelatedData(_Database);

        _Database.Update(drDolRelatedData);

        // HACK: /\ The fuck did we just do that for?

        // S11.13, S11.15
        _TornTransactionLog.Add(TornRecord.Create(_Database), _Database);
    }

    #endregion

    #region S11.16 - S11.17

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
}