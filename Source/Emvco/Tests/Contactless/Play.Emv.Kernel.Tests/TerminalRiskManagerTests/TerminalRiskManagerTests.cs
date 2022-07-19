
using AutoFixture;

using Moq;

using Play.Ber.DataObjects;
using Play.Core;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Terminal.RiskManagement;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Configuration;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services._TempLogShit;
using Play.Globalization.Currency;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

using Record = Play.Emv.Ber.ValueTypes.DataStorage.Record;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests;

public class TerminalRiskManagerTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    private readonly ITlvReaderAndWriter _Database;
    private readonly Mock<IStoreApprovedTransactions> _SplitPaymentsCoordinator;
    private readonly Mock<IProbabilitySelectionQueue> _ProbabilitySelectionQueue;

    private readonly IManageTerminalRisk _SystemUnderTest;

    #endregion

    #region Constructor

    public TerminalRiskManagerTests()
    {
        _Fixture = new ContactlessFixture().Create();

        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
        _Fixture.RegisterGlobalizationCodes();

        _SplitPaymentsCoordinator = new Mock<IStoreApprovedTransactions>(MockBehavior.Strict);
        _ProbabilitySelectionQueue = new Mock<IProbabilitySelectionQueue>(MockBehavior.Strict);

        _SystemUnderTest = new TerminalRiskManager(_SplitPaymentsCoordinator.Object, _ProbabilitySelectionQueue.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void CommandWithAuthorizedAmmount_And_NoPriorSplitPaymentLogsForGivenPan_ProcessingCommand_ReturnsFloorLimitExceededResponseAsync()
    {
        //Arrange
        _Fixture.Register(() => new AmountAuthorizedNumeric(1234));
        _Fixture.Register(() => new TerminalFloorLimit(123));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionExceedsFloorLimit();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        _SplitPaymentsCoordinator.Verify(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem), Times.Once);

        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmmount_And_WithPriorSplitPaymentLogsForGivenPan_ProcessingCommand_ReturnsFloorLimitExceededResponseAsync()
    {
        //Arrange
        _Fixture.Register(() => new AmountAuthorizedNumeric(1234));
        _Fixture.Register(() => new TerminalFloorLimit(123));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Ber.ValueTypes.DataStorage.RecordKey recordKey = _Fixture.Create<Ber.ValueTypes.DataStorage.RecordKey>();
        Ber.ValueTypes.DataStorage.Record dummyRecord = new Ber.ValueTypes.DataStorage.Record(recordKey, new Play.Ber.DataObjects.PrimitiveValue[0]);

        ApplicationCurrencyCode applicationCurrencyCode = _Fixture.Create<ApplicationCurrencyCode>();
        AmountAuthorizedNumeric previousAmount = new AmountAuthorizedNumeric(123);

        SplitPaymentLogItem expectedLogItem = new SplitPaymentLogItem(dummyRecord, applicationCurrencyCode, previousAmount);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out expectedLogItem))
            .Returns(true);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionExceedsFloorLimit();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        _SplitPaymentsCoordinator.Verify(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out expectedLogItem), Times.Once);
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmount_AuthorizedAmountSmallerThenRandomSelectionTreshold_TransactionIsRandomlySelectedForOnlineProcessing()
    {
        //Arrange & Setup.
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(true);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmmount_AuthorizedAmountSmallerThenBiasedRandomSelectionTresholdAndTerminalFloorLimit_TransactionIsRandomlySelectedForOnlineProcessing()
    {
        //Arrange & setup.
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Alpha3CurrencyCode currencyCode = _Fixture.Create<Alpha3CurrencyCode>();
        Money expectedBiasedRandomSelectionTreshHold = new Money(100, currencyCode);

        _Fixture.Register(() => expectedBiasedRandomSelectionTreshHold);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(true);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmmount_ProbabilitySetTo100_TransactionIsRandomlySelectedForOnlineProcessing()
    {
        //Arrange & setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Probability randomSelectionTargetProbability = new(100);

        _Fixture.Register(() => randomSelectionTargetProbability);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(true);

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmmount_VelocityCheckIsNotSupported_TransactionNotValid()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Alpha3CurrencyCode currencyCode = _Fixture.Create<Alpha3CurrencyCode>();
        Money expectedBiasedRandomSelectionTreshHold = new Money(134, currencyCode);

        _Fixture.Register(() => expectedBiasedRandomSelectionTreshHold);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsSuportedButDoesNotHaveRequiredItems_ReturnsUpperAndLowerConsecutiveOfflineLimitExceeded()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new LowerConsecutiveOfflineLimit(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new UpperConsecutiveOfflineLimit(35);
        _Database.Update(upperConsecutiveOfflineLimit);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetUpperConsecutiveOfflineLimitExceeded();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsSupportedAndHasRequiredItemsButApplicationTransactionCountIsSmallerThenLastTransactionCount_ReturnsUpperAndLowerConsecutiveOfflineLimitExceeded()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new LowerConsecutiveOfflineLimit(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new UpperConsecutiveOfflineLimit(35);
        _Database.Update(upperConsecutiveOfflineLimit);

        ApplicationTransactionCounter atc = new ApplicationTransactionCounter(12);
        _Database.Update(atc);

        LastOnlineApplicationTransactionCounterRegister lastOnlineAtc = new LastOnlineApplicationTransactionCounterRegister(118);
        _Database.Update(lastOnlineAtc);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();
        tvr.SetUpperConsecutiveOfflineLimitExceeded();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsSupportedAndHasRequiredItemsButLowerVelocityIsExceeded_ReturnsLowerConsecutiveOfflineLimitExceeded()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new LowerConsecutiveOfflineLimit(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new UpperConsecutiveOfflineLimit(140);
        _Database.Update(upperConsecutiveOfflineLimit);

        ApplicationTransactionCounter atc = new ApplicationTransactionCounter(118);
        _Database.Update(atc);

        LastOnlineApplicationTransactionCounterRegister lastOnlineAtc = new LastOnlineApplicationTransactionCounterRegister(13);
        _Database.Update(lastOnlineAtc);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator
            .Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsSupportedAndHasRequiredItemsButLowerAndHigherVelocityAreExceeded_ReturnsHigherAndLowerConsecutiveOfflineLimitExceeded()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new LowerConsecutiveOfflineLimit(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new UpperConsecutiveOfflineLimit(35);
        _Database.Update(upperConsecutiveOfflineLimit);

        ApplicationTransactionCounter atc = new ApplicationTransactionCounter(118);
        _Database.Update(atc);

        LastOnlineApplicationTransactionCounterRegister lastOnlineAtc = new LastOnlineApplicationTransactionCounterRegister(13);
        _Database.Update(lastOnlineAtc);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();
        tvr.SetUpperConsecutiveOfflineLimitExceeded();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsSupportedAndHasRequiredItemsButLastOnlineATCIs0_SetsTheNewCardTVRBitTo1()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new LowerConsecutiveOfflineLimit(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new UpperConsecutiveOfflineLimit(35);
        _Database.Update(upperConsecutiveOfflineLimit);

        ApplicationTransactionCounter atc = new ApplicationTransactionCounter(118);
        _Database.Update(atc);

        LastOnlineApplicationTransactionCounterRegister lastOnlineAtc = new LastOnlineApplicationTransactionCounterRegister(0);
        _Database.Update(lastOnlineAtc);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage))))
            .Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();
        tvr.SetUpperConsecutiveOfflineLimitExceeded();
        tvr.SetNewCard();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult)result);
    }

    #endregion
}