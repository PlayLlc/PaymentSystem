using AutoFixture;

using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Configuration;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests;

public class TerminalRiskManagerFactory
{
    #region Instance Members

    public static TerminalRiskManagementCommand CreateCommand(IFixture fixture)
    {
        AmountAuthorizedNumeric authorizedAmmount = fixture.Create<AmountAuthorizedNumeric>();
        ApplicationPan primaryAccountNumber = fixture.Create<ApplicationPan>();
        CultureProfile cultureProfile = fixture.Create<CultureProfile>();
        TerminalRiskManagementConfiguration terminalRiskManagementConfiguration = CreateTerminalRiskConfiguration(fixture, cultureProfile);

        TerminalRiskManagementCommand command = new(primaryAccountNumber, cultureProfile, authorizedAmmount, terminalRiskManagementConfiguration);

        return command;
    }

    public static TerminalRiskManagementCommand CreateFullCommand(
        IFixture fixture, ushort? applicationTransactionCount, ushort? lastOnlineApplicationTransactionCount, byte? lowerConsecutiveOfflineLimit,
        byte? upperConsecutiveOfflineLimit)
    {
        AmountAuthorizedNumeric authorizedAmmount = fixture.Create<AmountAuthorizedNumeric>();
        ApplicationPan primaryAccountNumber = fixture.Create<ApplicationPan>();
        CultureProfile cultureProfile = fixture.Create<CultureProfile>();
        TerminalRiskManagementConfiguration terminalRiskManagementConfiguration = CreateTerminalRiskConfiguration(fixture, cultureProfile);

        TerminalRiskManagementCommand command = new(primaryAccountNumber, cultureProfile, authorizedAmmount, terminalRiskManagementConfiguration,
            applicationTransactionCount, lastOnlineApplicationTransactionCount, lowerConsecutiveOfflineLimit, upperConsecutiveOfflineLimit);

        return command;
    }

    private static TerminalRiskManagementConfiguration CreateTerminalRiskConfiguration(IFixture fixture, CultureProfile cultureProfile)
    {
        Money biasedRandomSelectionTreshHold = fixture.Create<Money>();
        Probability randomSelectionTargetProbability = fixture.Create<Probability>();
        Probability randomSelectionMaximumProbability = fixture.Create<Probability>();
        TerminalFloorLimit terminalFloorLimit = fixture.Create<TerminalFloorLimit>();
        TerminalRiskManagementData terminalRiskManagementData = fixture.Create<TerminalRiskManagementData>();

        return new TerminalRiskManagementConfiguration(cultureProfile, terminalRiskManagementData, randomSelectionTargetProbability,
            biasedRandomSelectionTreshHold, randomSelectionMaximumProbability, terminalFloorLimit);
    }

    #endregion
}