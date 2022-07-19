using System.Threading.Tasks;

using AutoFixture;

using Moq;

using Play.Core;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Configuration;
using Play.Emv.Kernel.Databases;
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
    private readonly ITlvReaderAndWriter _Database;
    private readonly Mock<ICoordinateSplitPayments> _SplitPaymentsCoordinator;
    private readonly IManageTerminalRisk _SystemUnderTest;

    #endregion

    #region Constructor

    public TerminalRiskManagerIntegrationTests()
    {
        _Fixture = new ContactlessFixture().Create();

        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
        _Fixture.RegisterGlobalizationCodes();

        _SplitPaymentsCoordinator = new Mock<ICoordinateSplitPayments>(MockBehavior.Strict);

        _SystemUnderTest = new TerminalRiskManager(_SplitPaymentsCoordinator.Object, new ProbabilitySelectionQueue());
    }

    #endregion

    #region Instance Members

    [Fact]
    public void CommandWithAuthorizedAmount_RandomSelectionTargetPercentageIsSetTo100Percent_TransactionWillAlwaysBeSelectedForOnlineProcessing()
    {
        //Arrange & setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        _Fixture.RegisterTerminalRiskData(_Database);

        Probability randomSelectionTargetProbability = new(100);

        _Fixture.Register(() => randomSelectionTargetProbability);

        SplitPaymentLogItem? splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult) result);
    }

    [Fact]
    public void
        CommandWithAuthorizedAmountGreaterThanBiasedThresholdValue_IsUpForBiasedRandomSelectionWithTargetPercentageOf100Percent_TransactionWillAlwaysBeSelectedForOnlineProcessing()
    {
        //Arrange & setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(3671));
        _Fixture.Register(() => new TerminalFloorLimit(12345));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Alpha3CurrencyCode currencyCodeToBeUsed = _Fixture.Create<Alpha3CurrencyCode>();
        Money biasedRandomSelectionThreshold = new(2591, currencyCodeToBeUsed);

        _Fixture.Register(() => biasedRandomSelectionThreshold);

        Probability randomSelectionTargetProbability = new(100);

        _Fixture.Register(() => randomSelectionTargetProbability);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        SplitPaymentLogItem? splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult) result);
    }

    [Fact]
    public void
        CommandWithAuthorizedAmount_WithRandomTargetPercentAndRandomBiasedSelectionTreshold_TransactionWith2PossibleOutcomes_WillOrWillNotBeProcessedOnline()
    {
        //Arrange & setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(3671));
        _Fixture.Register(() => new TerminalFloorLimit(12345));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Alpha3CurrencyCode currencyCodeToBeUsed = _Fixture.Create<Alpha3CurrencyCode>();
        Money biasedRandomSelectionThreshold = new(2591, currencyCodeToBeUsed);

        _Fixture.Register(() => biasedRandomSelectionThreshold);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        SplitPaymentLogItem? splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalVerificationResult possibleTvr = new();
        possibleTvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.True(((ulong) result == 4096) || ((TerminalVerificationResult) result == TerminalVerificationResult.Empty));
    }

    #endregion
}