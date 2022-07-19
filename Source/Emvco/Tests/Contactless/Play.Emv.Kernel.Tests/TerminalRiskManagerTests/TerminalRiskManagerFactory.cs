using AutoFixture;

using Play.Core;
using Play.Emv.Configuration;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests;

public class TerminalRiskManagerFactory
{
    #region Instance Members

    //public static TerminalRiskManagementCommand CreateCommand(IFixture fixture)
    //{
    //    AmountAuthorizedNumeric authorizedAmmount = fixture.Create<AmountAuthorizedNumeric>();
    //    ApplicationPan primaryAccountNumber = fixture.Create<ApplicationPan>();
    //    CultureProfile cultureProfile = fixture.Create<CultureProfile>();
    //    TerminalRiskManagementConfiguration terminalRiskManagementConfiguration = CreateTerminalRiskConfiguration(fixture, cultureProfile);

    //    TerminalRiskManagementCommand command = new(primaryAccountNumber, cultureProfile, authorizedAmmount, terminalRiskManagementConfiguration);

    //    return command;
    //}

    //public static TerminalRiskManagementCommand CreateFullCommand(
    //    IFixture fixture, ushort? applicationTransactionCount, ushort? lastOnlineApplicationTransactionCount, byte? lowerConsecutiveOfflineLimit,
    //    byte? upperConsecutiveOfflineLimit)
    //{
    //    AmountAuthorizedNumeric authorizedAmmount = fixture.Create<AmountAuthorizedNumeric>();
    //    ApplicationPan primaryAccountNumber = fixture.Create<ApplicationPan>();
    //    CultureProfile cultureProfile = fixture.Create<CultureProfile>();
    //    TerminalRiskManagementConfiguration terminalRiskManagementConfiguration = CreateTerminalRiskConfiguration(fixture, cultureProfile);

    //    TerminalRiskManagementCommand command = new(primaryAccountNumber, cultureProfile, authorizedAmmount, terminalRiskManagementConfiguration,
    //        applicationTransactionCount, lastOnlineApplicationTransactionCount, lowerConsecutiveOfflineLimit, upperConsecutiveOfflineLimit);

    //    return command;
    //}

    public static TerminalRiskManagementConfiguration CreateTerminalRiskConfiguration(IFixture fixture)
    {
        Money biasedRandomSelectionThreshold = fixture.Create<Money>();
        Probability randomSelectionTargetProbability = fixture.Create<Probability>();
        Probability randomSelectionMaximumProbability = fixture.Create<Probability>();

        return new TerminalRiskManagementConfiguration(biasedRandomSelectionThreshold, randomSelectionTargetProbability, randomSelectionMaximumProbability);
    }

    #endregion
}