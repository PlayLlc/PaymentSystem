using System;

using AutoFixture;

using Moq;

using Play.Ber.DataObjects;
using Play.Core;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Terminal.RiskManagement;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Ber.ValueTypes.DataStorage;
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
    private readonly Mock<ICoordinateSplitPayments> _SplitPaymentsCoordinator;
    private readonly Mock<IProbabilitySelectionQueue> _ProbabilitySelectionQueue;
    private readonly IManageTerminalRisk _SystemUnderTest;

    #endregion

    #region Constructor

    public TerminalRiskManagerTests()
    {
        _Fixture = new ContactlessFixture().Create();

        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
        _Fixture.RegisterGlobalizationCodes();

        _SplitPaymentsCoordinator = new Mock<ICoordinateSplitPayments>(MockBehavior.Strict);
        _ProbabilitySelectionQueue = new Mock<IProbabilitySelectionQueue>(MockBehavior.Strict);

        _SystemUnderTest = new TerminalRiskManager(_SplitPaymentsCoordinator.Object, _ProbabilitySelectionQueue.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void NoPriorSplitPaymentLogs_ProcessingCommand_ReturnsFloorLimitExceededResponse()
    {
        //Arrange
        _Fixture.Register(() => new AmountAuthorizedNumeric(1234));
        _Fixture.Register(() => new TerminalFloorLimit(123));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem? splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.True(result.TransactionExceedsFloorLimit());
    }

    [Fact]
    public void PriorSplitPaymentLogsForGivenPan_ProcessingCommand_ReturnsFloorLimitExceededResponseAsync()
    {
        //Arrange
        _Fixture.Register(() => new AmountAuthorizedNumeric(1234));
        _Fixture.Register(() => new TerminalFloorLimit(123));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);
        RecordKey recordKey = _Fixture.Create<RecordKey>();
        Record dummyRecord = new(recordKey, Array.Empty<PrimitiveValue>());
        ApplicationCurrencyCode applicationCurrencyCode = _Fixture.Create<ApplicationCurrencyCode>();
        AmountAuthorizedNumeric previousAmount = new(123);
        SplitPaymentLogItem expectedLogItem = new(dummyRecord, applicationCurrencyCode, previousAmount);
        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out expectedLogItem)).Returns(true);

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        _SplitPaymentsCoordinator.Verify(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out expectedLogItem), Times.Once);
        Assert.True(result.TransactionExceedsFloorLimit());
    }

    [Fact]
    public void CommandWithAuthorizedAmount_AuthorizedAmountIsSmallerThanRandomSelectionThreshold_TransactionIsRandomlySelectedForOnlineProcessing()
    {
        //Arrange 
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);
        SplitPaymentLogItem? splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);
        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);
        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(true);

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.True(result.TransactionSelectedRandomlyForOnlineProcessing());
    }

    [Fact]
    public void
        CommandWithAuthorizedAmount_AuthorizedAmountIsSmallerThanBiasedRandomSelectionThresholdAndTerminalFloorLimit_TransactionIsRandomlySelectedForOnlineProcessing()
    {
        //Arrange & setup.
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));
        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Alpha3CurrencyCode currencyCode = _Fixture.Create<Alpha3CurrencyCode>();
        Money expectedBiasedRandomSelectionThreshold = new(100, currencyCode);

        _Fixture.Register(() => expectedBiasedRandomSelectionThreshold);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(true);

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert

        Assert.True(result.TransactionSelectedRandomlyForOnlineProcessing());
    }

    [Fact]
    public void CommandWithAuthorizedAmount_ProbabilitySetTo100_TransactionIsRandomlySelectedForOnlineProcessing()
    {
        //Arrange & setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Probability randomSelectionTargetProbability = new(100);

        _Fixture.Register(() => randomSelectionTargetProbability);

        SplitPaymentLogItem? splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(true);

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.True(result.TransactionSelectedRandomlyForOnlineProcessing());
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsNotSupported_TransactionNotValid()
    {
        //Arrange 
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        Alpha3CurrencyCode currencyCode = _Fixture.Create<Alpha3CurrencyCode>();
        Money expectedBiasedRandomSelectionThreshold = new(134, currencyCode);

        _Fixture.Register(() => expectedBiasedRandomSelectionThreshold);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult) result);
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsSupportedButDoesNotHaveRequiredItems_ReturnsUpperAndLowerConsecutiveOfflineLimitExceeded()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new(35);
        _Database.Update(upperConsecutiveOfflineLimit);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetUpperConsecutiveOfflineLimitExceeded();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult) result);
    }

    [Fact]
    public void
        CommandWithAuthorizedAmount_VelocityCheckIsSupportedAndHasRequiredItemsButApplicationTransactionCountIsSmallerThenLastTransactionCount_ReturnsUpperAndLowerConsecutiveOfflineLimitExceeded()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new(35);
        _Database.Update(upperConsecutiveOfflineLimit);

        ApplicationTransactionCounter atc = new(12);
        _Database.Update(atc);

        LastOnlineApplicationTransactionCounterRegister lastOnlineAtc = new(118);
        _Database.Update(lastOnlineAtc);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem? splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(false);

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.True(result.LowerConsecutiveOfflineLimitExceeded());
        Assert.True(result.UpperConsecutiveOfflineLimitExceeded());
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsSupportedAndHasRequiredItemsButLowerVelocityIsExceeded_ReturnsLowerConsecutiveOfflineLimitExceeded()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new(140);
        _Database.Update(upperConsecutiveOfflineLimit);

        ApplicationTransactionCounter atc = new(118);
        _Database.Update(atc);

        LastOnlineApplicationTransactionCounterRegister lastOnlineAtc = new(13);
        _Database.Update(lastOnlineAtc);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult) result);
    }

    [Fact]
    public void
        CommandWithAuthorizedAmount_VelocityCheckIsSupportedAndHasRequiredItemsButLowerAndHigherVelocityAreExceeded_ReturnsHigherAndLowerConsecutiveOfflineLimitExceeded()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new(35);
        _Database.Update(upperConsecutiveOfflineLimit);

        ApplicationTransactionCounter atc = new(118);
        _Database.Update(atc);

        LastOnlineApplicationTransactionCounterRegister lastOnlineAtc = new(13);
        _Database.Update(lastOnlineAtc);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();
        tvr.SetUpperConsecutiveOfflineLimitExceeded();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult) result);
    }

    [Fact]
    public void CommandWithAuthorizedAmount_VelocityCheckIsSupportedAndHasRequiredItemsButLastOnlineAtcIs0_SetsTheNewCardTvrBitTo1()
    {
        //Arrange & Setup
        _Fixture.Register(() => new AmountAuthorizedNumeric(123));
        _Fixture.Register(() => new TerminalFloorLimit(1234));

        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = new(15);
        _Database.Update(lowerConsecutiveOfflineLimit);

        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = new(35);
        _Database.Update(upperConsecutiveOfflineLimit);

        ApplicationTransactionCounter atc = new(118);
        _Database.Update(atc);

        LastOnlineApplicationTransactionCounterRegister lastOnlineAtc = new(0);
        _Database.Update(lastOnlineAtc);

        TerminalRiskManagerFixture.RegisterTerminalRiskData(_Fixture, _Database);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _SplitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem)).Returns(false);

        TerminalRiskManagementConfiguration terminalConfiguration = TerminalRiskManagerFactory.CreateTerminalRiskConfiguration(_Fixture);

        _ProbabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(terminalConfiguration.BiasedRandomSelectionTargetPercentage)))).Returns(false);

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();
        tvr.SetLowerConsecutiveOfflineLimitExceeded();
        tvr.SetUpperConsecutiveOfflineLimitExceeded();
        tvr.SetNewCard();

        //Act
        _SystemUnderTest.Process(_Database, terminalConfiguration);
        TerminalVerificationResults result = _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //Assert
        Assert.Equal(tvr, (TerminalVerificationResult) result);
    }

    #endregion
}