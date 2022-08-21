using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Outcomes;
using Play.Globalization;
using Play.Globalization.Time;

namespace Play.Emv.Selection.Start;

public class Preprocessor
{
    #region Instance Members

    // Start A
    /// <summary>
    ///     Entry Point is initiated at Pre-Processing for a new transaction with a variable amount.
    /// </summary>
    public void SetPreprocessingIndicators(
        Outcome outcome, PreProcessingIndicators preProcessingIndicators, AmountAuthorizedNumeric amountAuthorizedNumeric, CultureProfile cultureProfile)
    {
        preProcessingIndicators.Set(amountAuthorizedNumeric, cultureProfile);
        GetPreprocessingOutcome(outcome, preProcessingIndicators);
    }

    /// <remarks>
    ///     Book B Section 3.1.1.13
    /// </remarks>
    /// <returns></returns>
    private void GetPreprocessingOutcome(Outcome outcome, PreProcessingIndicators preProcessingIndicators)
    {
        OutcomeParameterSet.Builder? outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
        UserInterfaceRequestData.Builder userInterfaceSetter = UserInterfaceRequestData.GetBuilder();

        if (preProcessingIndicators.Any(a => !a.Value.ContactlessApplicationNotAllowed))
            return;

        outcomeParameterSetBuilder.Set(StatusOutcomes.TryAnotherInterface);
        outcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(true);
        outcomeParameterSetBuilder.SetIsUiRequestOnRestartPresent(false);
        outcomeParameterSetBuilder.SetIsDataRecordPresent(false);
        outcomeParameterSetBuilder.SetIsDiscretionaryDataPresent(false);
        outcomeParameterSetBuilder.Set(new Milliseconds(0));
        userInterfaceSetter.Set(DisplayMessageIdentifiers.PleaseInsertOrSwipeCard);
        userInterfaceSetter.Set(DisplayStatuses.ProcessingError);

        outcome.Update(outcomeParameterSetBuilder);
        outcome.Update(userInterfaceSetter);
    }

    #endregion
}