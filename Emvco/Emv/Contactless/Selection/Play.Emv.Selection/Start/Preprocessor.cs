using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Outcomes;
using Play.Globalization;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Selection.Start;

public class Preprocessor
{
    #region Instance Members

    // Start A
    /// <summary>
    ///     Entry Point is initiated at Pre-Processing for a new transaction with a variable amount.
    /// </summary>
    public void SetPreprocessingIndicators(
        in Outcome outcome,
        in PreProcessingIndicators preProcessingIndicators,
        in AmountAuthorizedNumeric amountAuthorizedNumeric,
        in CultureProfile cultureProfile)
    {
        preProcessingIndicators.Set(amountAuthorizedNumeric, cultureProfile);
        GetPreprocessingOutcome(outcome, preProcessingIndicators);
    }

    /// <remarks>
    ///     Book B Section 3.1.1.13
    /// </remarks>
    /// <returns></returns>
    private void GetPreprocessingOutcome(in Outcome outcome, in PreProcessingIndicators preProcessingIndicators)
    {
        OutcomeParameterSet.Builder? outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
        UserInterfaceRequestData.Builder userInterfaceSetter = UserInterfaceRequestData.GetBuilder();

        if (preProcessingIndicators.Any(a => !a.Value.ContactlessApplicationNotAllowed))
            return;

        outcomeParameterSetBuilder.Set(StatusOutcome.TryAnotherInterface);
        outcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(true);
        outcomeParameterSetBuilder.SetIsUiRequestOnRestartPresent(false);
        outcomeParameterSetBuilder.SetIsDataRecordPresent(false);
        outcomeParameterSetBuilder.SetIsDiscretionaryDataPresent(false);
        outcomeParameterSetBuilder.Set(new Milliseconds(0));
        userInterfaceSetter.Set(MessageIdentifier.PleaseInsertOrSwipeCard);
        userInterfaceSetter.Set(Status.ProcessingError);

        outcome.Update(outcomeParameterSetBuilder);
        outcome.Update(userInterfaceSetter);
    }

    #endregion
}