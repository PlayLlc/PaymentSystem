using System.Threading.Tasks;

using AutoFixture;

using Moq;

using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services._TempLogShit;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Globalization.Currency;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests;


//Testing Integration of TerminalRiskManager with the ProbabilitySelectionQueue
public class TerminalRiskManagerIntegrationTests
{
    #region Instance Values

    private readonly IFixture _Fixture;

    private readonly Mock<IStoreApprovedTransactions> _SplitPaymentsCoordinator;
    private readonly IProbabilitySelectionQueue _ProbabilitySelectionQueue;

    private readonly IManageTerminalRisk _SystemUnderTest;

    #endregion

    public TerminalRiskManagerIntegrationTests()
    {
        _Fixture = new ContactlessFixture().Create();

        ContactlessFixture.RegisterDefaultDatabase(_Fixture);
        _Fixture.RegisterGlobalizationCodes();

        _SplitPaymentsCoordinator = new Mock<IStoreApprovedTransactions>(MockBehavior.Strict);
        _ProbabilitySelectionQueue = new ProbabilitySelectionQueue();

        _SystemUnderTest = new TerminalRiskManager(_SplitPaymentsCoordinator.Object, _ProbabilitySelectionQueue);
    }

    [Fact]
    public async Task CommandWithAuthorizedAmount_RandomSelectionTargetPercentageIsSetTo100Percent_TransactionWillAlwaysBeSelectedForOnlineProcessing()
    {
        //Arrange & setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        Probability randomSelectionTargetProbability = new Probability(100);

        _Fixture.Register(() => randomSelectionTargetProbability);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementCommand command = TerminalRiskManagerFactory.CreateCommand(_Fixture);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        TerminalRiskManagementResponse actual = await _SystemUnderTest.Process(command);

        //Assert
        Assert.Equal(tvr, actual.GetTerminalVerificationResult());
    }

    [Fact]
    public async Task CommandWithAuthorizedAmountGreatherThenBiasedThresholValue_IsUpForBiasedRandomSelectionWithTargetPercentageOf100Percent_TransactionWillAlwaysBeSelectedForOnlineProcessing()
    {
        //Arrange & setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(3671));
        _Fixture.Register(() => new TerminalFloorLimit(12345));

        Alpha3CurrencyCode currencyCodeToBeUsed = _Fixture.Create<Alpha3CurrencyCode>();
        Money biasedRandomSelectionThreshold = new Money(2591, currencyCodeToBeUsed);

        _Fixture.Register(() => biasedRandomSelectionThreshold);

        Probability randomSelectionTargetProbability = new Probability(100);

        _Fixture.Register(() => randomSelectionTargetProbability);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementCommand command = TerminalRiskManagerFactory.CreateCommand(_Fixture);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        TerminalRiskManagementResponse actual = await _SystemUnderTest.Process(command);

        //Assert
        Assert.Equal(tvr, actual.GetTerminalVerificationResult());
    }

    [Fact]
    public async Task CommandWithAuthorizedAmount_WithRandomTargetPercentAndRandomBiasedSelectionTreshold_TransactionWith2PossibleOutcomes_WillOrWillNotBeProcessedOnline()
    {
        //Arrange & setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(3671));
        _Fixture.Register(() => new TerminalFloorLimit(12345));

        Alpha3CurrencyCode currencyCodeToBeUsed = _Fixture.Create<Alpha3CurrencyCode>();
        Money biasedRandomSelectionThreshold = new Money(2591, currencyCodeToBeUsed);

        _Fixture.Register(() => biasedRandomSelectionThreshold);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementCommand command = TerminalRiskManagerFactory.CreateCommand(_Fixture);

        TerminalVerificationResult possibleTvr = new();
        possibleTvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        TerminalRiskManagementResponse actual = await _SystemUnderTest.Process(command);

        //Assert
        Assert.True(possibleTvr.CompareTo(actual.GetTerminalVerificationResult()) == 0 || possibleTvr.CompareTo(TerminalVerificationResult.Empty) == 0);
    }
}
