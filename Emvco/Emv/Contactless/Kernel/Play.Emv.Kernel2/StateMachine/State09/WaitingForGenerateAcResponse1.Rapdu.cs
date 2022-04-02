using System;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;

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
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleL1Error(session.GetKernelSessionId(), signal))
            return _KernelStateResolver.GetKernelState(StateId);

        throw new NotImplementedException();
    }

    #endregion

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
            _Database.Update(MessageIdentifier.InsertSwipeOrTryAnotherCard);
            _Database.Update(Status.NotReady);
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(MessageOnErrorIdentifier.InsertSwipeOrTryAnotherCard);
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
            _Database.Update(MessageIdentifier.TryAgain);
            _Database.Update(Status.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);

            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcome.B);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.Update(signal.GetLevel1Error());
            _Database.Update(MessageOnErrorIdentifier.TryAgain);
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
}