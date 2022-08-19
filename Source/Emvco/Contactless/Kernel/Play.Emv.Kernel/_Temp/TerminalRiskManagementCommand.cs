using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

// DEPRECATING: We're refactoring Terminal Risk Management a bit
public class TerminalRiskManagementCommand
{
    #region Instance Values

    private readonly AmountAuthorizedNumeric _AmountAmountAuthorizedNumeric;
    private readonly ushort? _ApplicationTransactionCount;
    private readonly ushort? _LastOnlineApplicationTransactionCount;
    private readonly byte? _LowerConsecutiveOfflineLimit;
    private readonly byte? _UpperConsecutiveOfflineLimit;
    private readonly Probability _BiasedRandomSelectionMaximumProbability;
    private readonly ApplicationPan _PrimaryAccountNumber;

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

    public TerminalRiskManagementCommand(
        ApplicationPan primaryAccountNumber, CultureProfile cultureProfile, AmountAuthorizedNumeric amountAmountAuthorizedNumeric,
        Money biasedRandomSelectionThreshold, Probability randomSelectionTargetProbability, TerminalFloorLimit terminalFloorLimit,
        TerminalRiskManagementData terminalRiskManagementData)
    {
        _PrimaryAccountNumber = primaryAccountNumber;
        _CultureProfile = cultureProfile;
        _AmountAmountAuthorizedNumeric = amountAmountAuthorizedNumeric;
        _BiasedRandomSelectionThreshold = biasedRandomSelectionThreshold;
        _RandomSelectionTargetProbability = randomSelectionTargetProbability;
        _TerminalFloorLimit = terminalFloorLimit;
        _TerminalRiskManagementData = terminalRiskManagementData;
    }

    public TerminalRiskManagementCommand(
        ApplicationPan primaryAccountNumber, CultureProfile cultureProfile, AmountAuthorizedNumeric amountAmountAuthorizedNumeric,
        Money biasedRandomSelectionThreshold, Probability randomSelectionTargetProbability, TerminalFloorLimit terminalFloorLimit,
        TerminalRiskManagementData terminalRiskManagementData, ushort applicationTransactionCount, ushort lastOnlineApplicationTransactionCount,
        byte lowerConsecutiveOfflineLimit, byte upperConsecutiveOfflineLimit)
    {
        _PrimaryAccountNumber = primaryAccountNumber;
        _CultureProfile = cultureProfile;
        _AmountAmountAuthorizedNumeric = amountAmountAuthorizedNumeric;
        _ApplicationTransactionCount = applicationTransactionCount;
        _LastOnlineApplicationTransactionCount = lastOnlineApplicationTransactionCount;
        _LowerConsecutiveOfflineLimit = lowerConsecutiveOfflineLimit;
        _UpperConsecutiveOfflineLimit = upperConsecutiveOfflineLimit;
        _BiasedRandomSelectionThreshold = biasedRandomSelectionThreshold;
        _RandomSelectionTargetProbability = randomSelectionTargetProbability;
        _TerminalFloorLimit = terminalFloorLimit;
        _TerminalRiskManagementData = terminalRiskManagementData;
    }

    #endregion

    #region Instance Members

    public Money GetAmountAuthorizedNumeric() => _AmountAmountAuthorizedNumeric.AsMoney(_CultureProfile.GetNumericCurrencyCode());
    public ushort? GetApplicationTransactionCount() => _ApplicationTransactionCount;
    public Probability GetBiasedRandomSelectionMaximumPercentage() => _BiasedRandomSelectionMaximumProbability;
    public Money GetBiasedRandomSelectionThreshold() => _BiasedRandomSelectionThreshold;
    public ushort? GetLastOnlineApplicationTransactionCount() => _LastOnlineApplicationTransactionCount;
    public byte? GetLowerConsecutiveOfflineLimit() => _LowerConsecutiveOfflineLimit;
    public ApplicationPan GetPrimaryAccountNumber() => _PrimaryAccountNumber;
    public Probability GetRandomSelectionTargetPercentage() => _RandomSelectionTargetProbability;
    public Money GetTerminalFloorLimit() => _TerminalFloorLimit.AsMoney(_CultureProfile);
    public byte? GetUpperConsecutiveOfflineLimit() => _UpperConsecutiveOfflineLimit;
    public bool IsVelocityCheckSupported() => (_UpperConsecutiveOfflineLimit != null) && (_LowerConsecutiveOfflineLimit != null);

    #endregion
}