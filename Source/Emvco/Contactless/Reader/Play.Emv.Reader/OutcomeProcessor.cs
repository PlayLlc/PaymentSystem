using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Selection.Contracts;
using Play.Messaging;

namespace Play.Emv.Reader;

/// <remarks>Book B Section 3.5</remarks>
public class OutcomeProcessor : IProcessOutcome
{
    #region Instance Values

    protected readonly IEndpointClient _EndpointClient;

    #endregion

    #region Constructor

    public OutcomeProcessor(IEndpointClient endpointClient)
    {
        _EndpointClient = endpointClient;
    }

    #endregion

    #region Instance Members

    // TODO: Check that the logic for handling errors from Entry Point Start processing is the same as handling an outcome from the kernel. If not, then we need to update the logic in the Process methods below

    /// <exception cref="InvalidOperationException"></exception>
    public virtual void Process(CorrelationId correlationId, TransactionSessionId sessionId, Transaction transaction)
    {
        HandleFieldOffRequest(transaction.GetOutcome());
        HandleUiRequestOnOutcome(transaction.GetOutcome());

        // Book B Section 3.5 Try Again and Select Next are processed immediately by Entry Point which re-starts processing at the appropriate start
        if (TryHandleTryAgainStatus(transaction))
            return;

        transaction.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);
        transaction.TryGetDiscretionaryData(out DiscretionaryData? discretionaryData);
        transaction.TryGetDataRecord(out DataRecord? dataRecord);

        _EndpointClient.Send(new OutReaderResponse(correlationId,
            new FinalOutcome(sessionId, transaction.GetOutcomeParameterSet(), discretionaryData, userInterfaceRequestData, dataRecord)));
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
    protected bool TryHandleTryAgainStatus(Transaction transaction)
    {
        if (transaction.GetOutcome().GetStatusOutcome() == StatusOutcomes.SelectNext)
        {
            _EndpointClient.Send(new ActivateSelectionRequest(transaction));

            return true;
        }

        if (transaction.GetOutcome().GetStatusOutcome() == StatusOutcomes.TryAgain)
        {
            _EndpointClient.Send(new ActivateSelectionRequest(transaction));

            return true;
        }

        return false;
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
                throw new InvalidOperationException($"The {nameof(OutcomeProcessor)} expected {nameof(UserInterfaceRequestData)} to be present but it was not");

            _EndpointClient.Send(new DisplayMessageRequest(result));
        }
    }

    #endregion
}