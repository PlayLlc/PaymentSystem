using Play.Core;
using Play.Globalization.Currency;

namespace Play.Emv.Configuration;

public class TerminalRiskManagementConfiguration
{
    #region Instance Values

    /// <summary>
    ///     This is a threshold amount, simply referred to as the threshold value, which can be zero or a positive number
    ///     smaller than the Terminal Floor Limit
    /// </summary>
    public Money BiasedRandomSelectionThreshold { get; init; }
    public Probability BiasedRandomSelectionTargetPercentage { get; init; }
    public Probability BiasedRandomSelectionMaximumPercentage { get; init; }

    #endregion

    #region Constructor

    public TerminalRiskManagementConfiguration(
        Money biasedRandomSelectionThreshold, Probability biasedRandomSelectionTargetPercentage, Probability biasedRandomSelectionMaximumPercentage)
    {
        BiasedRandomSelectionThreshold = biasedRandomSelectionThreshold;
        BiasedRandomSelectionTargetPercentage = biasedRandomSelectionTargetPercentage;
        BiasedRandomSelectionMaximumPercentage = biasedRandomSelectionMaximumPercentage;
    }

    #endregion
}