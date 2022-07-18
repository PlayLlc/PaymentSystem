using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Configuration;

public class TerminalRiskManagementConfiguration
{
    #region Instance Values

    /// <summary>
    ///     This is a threshold amount, simply referred to as the threshold value, which can be zero or a positive number
    ///     smaller than the Terminal Floor Limit
    /// </summary>
    public readonly Money BiasedRandomSelectionThreshold;

    public readonly Probability BiasedRandomSelectionTargetPercentage;
    public readonly Probability _BiasedRandomSelectionMaximumPercentage;

    #endregion

    #region Constructor

    public TerminalRiskManagementConfiguration(
        Money biasedRandomSelectionThreshold, Probability biasedRandomSelectionTargetPercentage, Probability biasedRandomSelectionMaximumPercentage)
    {
        BiasedRandomSelectionThreshold = biasedRandomSelectionThreshold;
        BiasedRandomSelectionTargetPercentage = biasedRandomSelectionTargetPercentage;
        _BiasedRandomSelectionMaximumPercentage = biasedRandomSelectionMaximumPercentage;
    }

    #endregion
}