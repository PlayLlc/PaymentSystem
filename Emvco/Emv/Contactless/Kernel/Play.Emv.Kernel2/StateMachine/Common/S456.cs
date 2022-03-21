using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;
using Play.Globalization.Time.Seconds;
using Play.Messaging;

using KernelDatabase = Play.Emv.Kernel.Databases.KernelDatabase;

namespace Play.Emv.Kernel2.StateMachine;

/// <summary>
///     This object includes logic that is common to states 4, 5, and 6
/// </summary>
public class S456 : CommonProcessing
{
    #region Instance Values

    protected override StateId[] _ValidStateIds { get; } =
    {
        WaitingForEmvReadRecordResponse.StateId, WaitingForGetDataResponse.StateId, WaitingForEmvModeFirstWriteFlag.StateId
    };

    #endregion

    #region Constructor

    public S456(
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchangeKernelService,
        IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint,
        IKernelEndpoint kernelEndpoint) : base(kernelDatabase, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    public override KernelState Process(IGetKernelStateId kernelStateId, Kernel2Session session)
    {
        HandleRequestOutOfSync(kernelStateId.GetStateId());

        if (TryToGetNeededData())
            return _KernelStateResolver.GetKernelState(WaitingForGetDataResponse.StateId);

        if (TryToReadApplicationData(session))
            return _KernelStateResolver.GetKernelState(WaitingForEmvReadRecordResponse.StateId);

        if (TryWaitingForFirstWriteFlag(session))
            return _KernelStateResolver.GetKernelState(WaitingForEmvModeFirstWriteFlag.StateId);

        if (IsAmountAuthorizedEmpty())
        {
            HandleLevel3Error(session.GetKernelSessionId());

            return _KernelStateResolver.GetKernelState(kernelStateId.GetStateId());
        }

        if (TryHandlingMaxTransactionAmountExceeded(session.GetKernelSessionId()))
            return _KernelStateResolver.GetKernelState(kernelStateId.GetStateId());

        throw new InvalidOperationException();
    }

    #region S456.1

    private bool TryToGetNeededData() => _DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead);

    #endregion

    #region S456.2 - S456.4

    private bool TryToReadApplicationData(Kernel2Session session)
    {
        if (!session.IsActiveTagEmpty())
            return false;

        _DataExchangeKernelService.ResolveTagsToReadYet(_KernelDatabase);
        _DataExchangeKernelService.SendResponse(session.GetKernelSessionId());

        return true;
    }

    #endregion

    #region S456.5

    private bool IsProceedToFirstWriteFlagEmpty() => !_KernelDatabase.IsPresentAndNotEmpty(ProceedToFirstWriteFlag.Tag);

    #endregion

    #region S456.8

    private void AttemptToExchangeData(KernelSessionId sessionId)
    {
        if (_DataExchangeKernelService.IsEmpty(DekRequestType.DataNeeded))
            _DataExchangeKernelService.SendRequest(sessionId);

        if (!_DataExchangeKernelService.IsEmpty(DekRequestType.TagsToRead))
            return;

        if (_DataExchangeKernelService.IsEmpty(DekResponseType.DataToSend))
            return;

        _DataExchangeKernelService.SendResponse(sessionId);
    }

    #endregion

    #region S456.6 - S456.10

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool TryWaitingForFirstWriteFlag(KernelSession session)
    {
        if (!IsProceedToFirstWriteFlagNonZero())
            return false;

        if (IsProceedToFirstWriteFlagEmpty())
            _DataExchangeKernelService.Enqueue(DekRequestType.DataNeeded, ProceedToFirstWriteFlag.Tag);

        HandleWaitingForFirstWriteFlag(session);

        return true;
    }

    #endregion

    #region S456.7 - S456.10

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private void HandleWaitingForFirstWriteFlag(KernelSession session)
    {
        _DataExchangeKernelService.Resolve(_KernelDatabase);
        AttemptToExchangeData(session.GetKernelSessionId());
        session.Timer.Start((Milliseconds) (TimeoutValue) _KernelDatabase.Get(TimeoutValue.Tag));
    }

    #endregion

    #region S456.11

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool IsProceedToFirstWriteFlagNonZero()
    {
        if (_KernelDatabase.TryGet(ProceedToFirstWriteFlag.Tag, out PrimitiveValue? result))
            return false;

        return (ProceedToFirstWriteFlag) result! != 0;
    }

    #endregion

    #region S456.12

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool IsAmountAuthorizedEmpty() => !_KernelDatabase.IsPresentAndNotEmpty(AmountAuthorizedNumeric.Tag);

    #endregion

    #region S456.13

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private void HandleLevel3Error(KernelSessionId sessionId)
    {
        _KernelDatabase.Update(StatusOutcome.EndApplication);
        _KernelDatabase.Update(Level3Error.AmountNotPresent);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #region S456.14 - S456.15

    private bool TryHandlingMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        if (!IsMaxTransactionAmountExceeded())
            return false;

        HandleMaxTransactionAmountExceeded(sessionId);

        return true;
    }

    #endregion

    #region S456.14

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private bool IsMaxTransactionAmountExceeded()
    {
        AmountAuthorizedNumeric authorizedAmount = _KernelDatabase.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        ReaderContactlessTransactionLimit transactionLimit = _KernelDatabase.GetReaderContactlessTransactionLimit();

        // BUG: We need to make sure that the application currency and transaction currency are the same. Need to resolve the Terminal Reference Currency if they are different
        TransactionCurrencyCode currency = _KernelDatabase.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);

        return authorizedAmount.AsMoney(currency) > transactionLimit.AsMoney(currency);
    }

    #endregion

    #region S456.15

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    private void HandleMaxTransactionAmountExceeded(KernelSessionId sessionId)
    {
        _KernelDatabase.Update(FieldOffRequestOutcome.NotAvailable);
        _KernelDatabase.Update(StatusOutcome.SelectNext);
        _KernelDatabase.Update(StartOutcome.C);
        _KernelDatabase.Update(Level2Error.MaxLimitExceeded);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(sessionId));
    }

    #endregion

    #endregion
}