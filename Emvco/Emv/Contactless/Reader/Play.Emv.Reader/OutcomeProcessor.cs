using System;

using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Outcomes;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Selection.Contracts;
using Play.Emv.Transactions;
using Play.Messaging;

namespace Play.Emv.Reader;

/// <remarks>Book B Section 3.5</remarks>
public class OutcomeProcessor : IProcessOutcome
{
    #region Instance Values

    protected readonly IHandleSelectionRequests _SelectionEndpoint;
    protected readonly IHandleDisplayRequests _DisplayEndpoint;
    protected readonly IReaderEndpoint _ReaderEndpoint;

    #endregion

    #region Constructor

    public OutcomeProcessor(
        IHandleSelectionRequests selectionEndpoint,
        IHandleDisplayRequests displayEndpoint,
        IReaderEndpoint readerEndpoint)
    {
        _SelectionEndpoint = selectionEndpoint;
        _DisplayEndpoint = displayEndpoint;
        _ReaderEndpoint = readerEndpoint;
    }

    #endregion

    #region Instance Members

    /// <param name="correlationId"></param>
    /// <param name="transaction"></param>
    /// <returns></returns>
    /// <remarks>Book B Section 3.5</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual void Process(CorrelationId correlationId, Transaction transaction)
    {
        HandleFieldOffRequest(transaction.GetOutcome());
        HandleUiRequestOnOutcome(transaction.GetOutcome());
        HandleTryAgainStatus(transaction);

        transaction.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);
        transaction.TryGetDiscretionaryData(out DiscretionaryData? discretionaryData);
        transaction.TryGetDataRecord(out DataRecord? dataRecord);

        _ReaderEndpoint.Send(new OutReaderResponse(correlationId,
            new FinalOutcome(transaction.GetTransactionSessionId(), transaction.GetOutcomeParameterSet(), discretionaryData,
                userInterfaceRequestData, dataRecord)));
    }

    /// <remarks>
    ///     Book B Section 3.5.1.2
    /// </remarks>
    protected void HandleFieldOffRequest(Outcome outcome)
    {
        if (outcome.GetFieldOffRequestOutcome() != FieldOffRequestOutcome.NotAvailable)
        {
            // HACK: This logic needs to be implemented 
        }
    }

    /// <remarks>
    ///     Book B Section 3.5.1.3
    /// </remarks>
    protected void HandleTryAgainStatus(Transaction transaction)
    {
        if (transaction.GetOutcome().GetStatusOutcome() == StatusOutcome.SelectNext)
            _SelectionEndpoint.Request(new ActivateSelectionRequest(transaction));
    }

    /// <remarks>
    ///     Book B Section 3.5.1.1
    /// </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    protected void HandleUiRequestOnOutcome(Outcome outcome)
    {
        if (outcome.IsUiRequestOnOutcomePresent())
        {
            _ = outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? result);

            if (result == null)
            {
                throw new InvalidOperationException(
                    $"The {nameof(OutcomeProcessor)} expected {nameof(UserInterfaceRequestData)} to be present but it was not");
            }

            _DisplayEndpoint.Request(new DisplayMessageRequest(result));
        }
    }

    #endregion
}