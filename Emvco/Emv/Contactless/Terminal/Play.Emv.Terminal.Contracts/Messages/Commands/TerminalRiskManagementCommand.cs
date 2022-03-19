using Play.Core.Math;
using Play.Emv.Ber.DataElements;
using Play.Emv.Configuration;
using Play.Globalization;
using Play.Globalization.Currency;

using PrimaryAccountNumber = Play.Emv.Ber.DataElements.PrimaryAccountNumber;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

public class TerminalRiskManagementCommand
{
    #region Instance Values

    private readonly AmountAuthorizedNumeric _AmountAmountAuthorizedNumeric;
    private readonly ushort? _ApplicationTransactionCount;
    private readonly CultureProfile _CultureProfile;
    private readonly ushort? _LastOnlineApplicationTransactionCount;
    private readonly byte? _LowerConsecutiveOfflineLimit;
    private readonly PrimaryAccountNumber _PrimaryAccountNumber;
    private readonly TerminalRiskConfiguration _TerminalRiskConfiguration;
    private readonly byte? _UpperConsecutiveOfflineLimit;

    #endregion

    #region Constructor

    public TerminalRiskManagementCommand(
        PrimaryAccountNumber primaryAccountNumber,
        CultureProfile cultureProfile,
        AmountAuthorizedNumeric amountAmountAuthorizedNumeric,
        TerminalRiskConfiguration terminalRiskConfiguration)
    {
        _PrimaryAccountNumber = primaryAccountNumber;
        _CultureProfile = cultureProfile;
        _AmountAmountAuthorizedNumeric = amountAmountAuthorizedNumeric;
        _TerminalRiskConfiguration = terminalRiskConfiguration;
    }

    public TerminalRiskManagementCommand(
        PrimaryAccountNumber primaryAccountNumber,
        CultureProfile cultureProfile,
        AmountAuthorizedNumeric amountAmountAuthorizedNumeric,
        TerminalRiskConfiguration terminalRiskConfiguration,
        ushort applicationTransactionCount,
        ushort lastOnlineApplicationTransactionCount,
        byte lowerConsecutiveOfflineLimit,
        byte upperConsecutiveOfflineLimit)
    {
        _PrimaryAccountNumber = primaryAccountNumber;
        _CultureProfile = cultureProfile;
        _AmountAmountAuthorizedNumeric = amountAmountAuthorizedNumeric;
        _TerminalRiskConfiguration = terminalRiskConfiguration;
        _ApplicationTransactionCount = applicationTransactionCount;
        _LastOnlineApplicationTransactionCount = lastOnlineApplicationTransactionCount;
        _LowerConsecutiveOfflineLimit = lowerConsecutiveOfflineLimit;
        _UpperConsecutiveOfflineLimit = upperConsecutiveOfflineLimit;
    }

    #endregion

    #region Instance Members

    public Money GetAmountAuthorizedNumeric() => _AmountAmountAuthorizedNumeric.AsMoney(_CultureProfile.GetNumericCurrencyCode());
    public ushort? GetApplicationTransactionCount() => _ApplicationTransactionCount;
    public Percentage GetBiasedRandomSelectionMaximumPercentage() => _TerminalRiskConfiguration.GetBiasedRandomSelectionMaximumPercentage();
    public Money GetBiasedRandomSelectionThreshold() => _TerminalRiskConfiguration.GetBiasedRandomSelectionThreshold();
    public ushort? GetLastOnlineApplicationTransactionCount() => _LastOnlineApplicationTransactionCount;
    public byte? GetLowerConsecutiveOfflineLimit() => _LowerConsecutiveOfflineLimit;
    public PrimaryAccountNumber GetPrimaryAccountNumber() => _PrimaryAccountNumber;
    public Percentage GetRandomSelectionTargetPercentage() => _TerminalRiskConfiguration.GetRandomSelectionTargetPercentage();
    public Money GetTerminalFloorLimit() => _TerminalRiskConfiguration.GetTerminalFloorLimit();
    public byte? GetUpperConsecutiveOfflineLimit() => _UpperConsecutiveOfflineLimit;
    public bool IsVelocityCheckSupported() => (_UpperConsecutiveOfflineLimit != null) && (_LowerConsecutiveOfflineLimit != null);

    #endregion
}