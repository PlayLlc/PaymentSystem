using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Configuration;

public class TerminalRiskConfiguration
{
    #region Instance Values

    private readonly Probability _BiasedRandomSelectionMaximumProbability;

    /// <summary>
    ///     This is a threshold amount, simply referred to as the threshold value, which can be zero or a positive number
    ///     smaller than the Terminal Floor Limit
    /// </summary>
    private readonly Money _BiasedRandomSelectionThreshold;

    private readonly CultureProfile _CultureProfile;
    private readonly Probability _RandomSelectionTargetProbability;
    private readonly TerminalFloorLimit _TerminalFloorLimit;
    private readonly TerminalRiskManagementData _TerminalRiskManagementData;

    #endregion

    #region Constructor

    public TerminalRiskConfiguration(
        CultureProfile cultureProfile, TerminalRiskManagementData terminalRiskManagementData, Probability biasedRandomSelectionMaximumProbability,
        Money biasedRandomSelectionThreshold, Probability randomSelectionTargetProbability, TerminalFloorLimit terminalFloorLimit)
    {
        _CultureProfile = cultureProfile;
        _TerminalRiskManagementData = terminalRiskManagementData;
        _BiasedRandomSelectionMaximumProbability = biasedRandomSelectionMaximumProbability;
        _BiasedRandomSelectionThreshold = biasedRandomSelectionThreshold;
        _RandomSelectionTargetProbability = randomSelectionTargetProbability;
        _TerminalFloorLimit = terminalFloorLimit;
    }

    #endregion

    #region Instance Members

    public Probability GetBiasedRandomSelectionMaximumPercentage() => _BiasedRandomSelectionMaximumProbability;
    public Money GetBiasedRandomSelectionThreshold() => _BiasedRandomSelectionThreshold;
    public Probability GetRandomSelectionTargetPercentage() => _RandomSelectionTargetProbability;
    public Money GetTerminalFloorLimit() => _TerminalFloorLimit.AsMoney(_CultureProfile);
    public TerminalRiskManagementData GetTerminalRiskManagementData() => _TerminalRiskManagementData;

    #endregion
}