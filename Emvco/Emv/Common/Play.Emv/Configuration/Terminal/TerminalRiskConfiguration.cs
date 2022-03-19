using Play.Core.Math;
using Play.Emv.Ber.DataElements;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Configuration;

public class TerminalRiskConfiguration
{
    #region Instance Values

    private readonly Percentage _BiasedRandomSelectionMaximumPercentage;

    /// <summary>
    ///     This is a threshold amount, simply referred to as the threshold value, which can be zero or a positive number
    ///     smaller than the Terminal Floor Limit
    /// </summary>
    private readonly Money _BiasedRandomSelectionThreshold;

    private readonly CultureProfile _CultureProfile;
    private readonly Percentage _RandomSelectionTargetPercentage;
    private readonly TerminalFloorLimit _TerminalFloorLimit;
    private readonly TerminalRiskManagementData _TerminalRiskManagementData;

    #endregion

    #region Constructor

    public TerminalRiskConfiguration(
        CultureProfile cultureProfile,
        TerminalRiskManagementData terminalRiskManagementData,
        Percentage biasedRandomSelectionMaximumPercentage,
        Money biasedRandomSelectionThreshold,
        Percentage randomSelectionTargetPercentage,
        TerminalFloorLimit terminalFloorLimit)
    {
        _CultureProfile = cultureProfile;
        _TerminalRiskManagementData = terminalRiskManagementData;
        _BiasedRandomSelectionMaximumPercentage = biasedRandomSelectionMaximumPercentage;
        _BiasedRandomSelectionThreshold = biasedRandomSelectionThreshold;
        _RandomSelectionTargetPercentage = randomSelectionTargetPercentage;
        _TerminalFloorLimit = terminalFloorLimit;
    }

    #endregion

    #region Instance Members

    public Percentage GetBiasedRandomSelectionMaximumPercentage() => _BiasedRandomSelectionMaximumPercentage;
    public Money GetBiasedRandomSelectionThreshold() => _BiasedRandomSelectionThreshold;
    public Percentage GetRandomSelectionTargetPercentage() => _RandomSelectionTargetPercentage;
    public Money GetTerminalFloorLimit() => _TerminalFloorLimit.AsMoney(_CultureProfile);
    public TerminalRiskManagementData GetTerminalRiskManagementData() => _TerminalRiskManagementData;

    #endregion
}