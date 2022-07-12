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
    public static TerminalRiskManagementCommand CreateCommand(IFixture fixture)
    {
        AmountAuthorizedNumeric authorizedAmmount = fixture.Create<AmountAuthorizedNumeric>();
        ApplicationPan primaryAccountNumber = fixture.Create<ApplicationPan>();
        CultureProfile cultureProfile = fixture.Create<CultureProfile>();
        TerminalRiskConfiguration terminalRiskConfiguration = CreateTerminalRiskConfiguration(fixture, cultureProfile);

        TerminalRiskManagementCommand command = new(primaryAccountNumber, cultureProfile, authorizedAmmount, terminalRiskConfiguration);

        return command;
    }

    private static TerminalRiskConfiguration CreateTerminalRiskConfiguration(IFixture fixture, CultureProfile cultureProfile)
    {
        Money biasedRandomSelectionTreshHold = fixture.Create<Money>();
        Probability randomSelectionTargetProbability = fixture.Create<Probability>();
        Probability randomSelectionMaximumProbability = fixture.Create<Probability>();
        TerminalFloorLimit terminalFloorLimit = fixture.Create<TerminalFloorLimit>();
        TerminalRiskManagementData terminalRiskManagementData = fixture.Create<TerminalRiskManagementData>();

        return new TerminalRiskConfiguration(cultureProfile, terminalRiskManagementData, randomSelectionTargetProbability, biasedRandomSelectionTreshHold,
            randomSelectionMaximumProbability, terminalFloorLimit);
    }
}
