using AutoFixture;

using Play.Core;
using Play.Emv.Configuration;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests;

public class TerminalRiskManagerFactory
{
    #region Instance Members

    public static TerminalRiskManagementConfiguration CreateTerminalRiskConfiguration(IFixture fixture)
    {
        Money biasedRandomSelectionThreshold = fixture.Create<Money>();
        Probability randomSelectionTargetProbability = fixture.Create<Probability>();
        Probability randomSelectionMaximumProbability = fixture.Create<Probability>();

        return new TerminalRiskManagementConfiguration(biasedRandomSelectionThreshold, randomSelectionTargetProbability, randomSelectionMaximumProbability);
    }

    #endregion
}