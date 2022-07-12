﻿using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.AutoMoq;

using Moq;

using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.Services._TempLogShit;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests;

public class TerminalRiskManagerTests : TestBase
{
    #region Instance Values

    private readonly IFixture _fixture;

    private readonly Mock<ICoordinateSplitPayments> _splitPaymentsCoordinator;
    private readonly Mock<IProbabilitySelectionQueue> _probabilitySelectionQueue;

    private readonly IManageTerminalRisk _systemUnderTest;

    #endregion

    public TerminalRiskManagerTests()
    {
        _fixture = new ContactlessFixture().Create();

        ContactlessFixture.RegisterDefaultDatabase(_fixture);
        _fixture.RegisterGlobalizationCodes();

        _splitPaymentsCoordinator = new Mock<ICoordinateSplitPayments>(MockBehavior.Strict);
        _probabilitySelectionQueue = new Mock<IProbabilitySelectionQueue>(MockBehavior.Strict);

        _systemUnderTest = new TerminalRiskManager(_splitPaymentsCoordinator.Object, _probabilitySelectionQueue.Object);
    }

    [Fact]
    public async Task CommandWithAuthorizedAmmount_And_NoPriorSplitPaymentLogsForGivenPan_ProcessingCommand_ReturnsFloorLimitExceededResponseAsync()
    {
        //Arrange
        _fixture.Register(() => new AmountAuthorizedNumeric(1234));
        _fixture.Register(() => new TerminalFloorLimit(123));

        SplitPaymentLogItem splitPaymentLogItem = null;
        _splitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementCommand command = TerminalRiskManagerFactory.CreateCommand(_fixture);
        TerminalVerificationResult tvr = new();
        tvr.SetTransactionExceedsFloorLimit();

        //Act
        TerminalRiskManagementResponse actual = await _systemUnderTest.Process(command);

        //Assert
        _splitPaymentsCoordinator.Verify(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem), Times.Once);
        Assert.Equal(tvr, actual.GetTerminalVerificationResult());
    }

    [Fact]
    public async Task CommandWithAuthorizedAmmount_And_WithPriorSplitPaymentLogsForGivenPan_ProcessingCommand_ReturnsFloorLimitExceededResponseAsync()
    {
        //Arrange
        _fixture.Register(() => new AmountAuthorizedNumeric(1234));
        _fixture.Register(() => new TerminalFloorLimit(123));

        Ber.ValueTypes.DataStorage.RecordKey recordKey = _fixture.Create<Ber.ValueTypes.DataStorage.RecordKey>();
        Ber.ValueTypes.DataStorage.Record dummyRecord = new Ber.ValueTypes.DataStorage.Record(recordKey, new Play.Ber.DataObjects.PrimitiveValue[0]);

        Alpha3CurrencyCode currencyCode = _fixture.Create<Alpha3CurrencyCode>();
        Money expectedAmount = new Money(123, currencyCode);

        SplitPaymentLogItem expectedLogItem = new SplitPaymentLogItem(dummyRecord, expectedAmount);

        _splitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out expectedLogItem))
            .Returns(true);

        TerminalRiskManagementCommand command = TerminalRiskManagerFactory.CreateCommand(_fixture);
        TerminalVerificationResult tvr = new();
        tvr.SetTransactionExceedsFloorLimit();

        //Act
        TerminalRiskManagementResponse actual = await _systemUnderTest.Process(command);

        //Assert
        _splitPaymentsCoordinator.Verify(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out expectedLogItem), Times.Once);
        Assert.Equal(tvr, actual.GetTerminalVerificationResult());
    }

    [Fact]
    public async Task CommandWithAuthorizedAmount_AuthorizedAmountSmallerThenRandomSelectionTreshold_TransactionIsRandomlySelectedForOnlineProcessing()
    {
        //Arrange & Setup.
        _fixture.Register(() => new AmountAuthorizedNumeric(123));
        _fixture.Register(() => new TerminalFloorLimit(1234));

        SplitPaymentLogItem splitPaymentLogItem = null;
        _splitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementCommand command = TerminalRiskManagerFactory.CreateCommand(_fixture);

        _probabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(command.GetRandomSelectionTargetPercentage()))))
            .Returns(Task.FromResult(true));

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        TerminalRiskManagementResponse actual = await _systemUnderTest.Process(command);

        //Assert
        Assert.Equal(tvr, actual.GetTerminalVerificationResult());
    }

    [Fact]
    public async Task CommandWithAuthorizedAmmount_AuthorizedAmountSmallerThenBiasedRandomSelectionTresholdAndTerminalFloorLimit_TransactionIsRandomlySelectedForOnlineProcessing()
    {
        //Arrange & setup.
        _fixture.Register(() => new AmountAuthorizedNumeric(123));
        _fixture.Register(() => new TerminalFloorLimit(1234));

        Alpha3CurrencyCode currencyCode = _fixture.Create<Alpha3CurrencyCode>();
        Money expectedBiasedRandomSelectionTreshHold = new Money(100, currencyCode);

        _fixture.Register(() => expectedBiasedRandomSelectionTreshHold);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _splitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementCommand command = TerminalRiskManagerFactory.CreateCommand(_fixture);

        _probabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(command.GetRandomSelectionTargetPercentage()))))
            .Returns(Task.FromResult(true));

        TerminalVerificationResult tvr = new();
        tvr.SetTransactionSelectedRandomlyForOnlineProcessing();

        //Act
        TerminalRiskManagementResponse actual = await _systemUnderTest.Process(command);

        //Assert
        Assert.Equal(tvr, actual.GetTerminalVerificationResult());
    }

    [Fact]
    public async Task CommandWithAuthorizedAmmount_VelocityCheckIsNotSupported_TransactionNotValid()
    {
        //Arrange & Setup
        _fixture.Register(() => new AmountAuthorizedNumeric(123));
        _fixture.Register(() => new TerminalFloorLimit(1234));

        Alpha3CurrencyCode currencyCode = _fixture.Create<Alpha3CurrencyCode>();
        Money expectedBiasedRandomSelectionTreshHold = new Money(134, currencyCode);

        _fixture.Register(() => expectedBiasedRandomSelectionTreshHold);

        SplitPaymentLogItem splitPaymentLogItem = null;
        _splitPaymentsCoordinator.Setup(m => m.TryGetSplitPaymentLogItem(It.IsAny<ApplicationPan>(), out splitPaymentLogItem))
            .Returns(false);

        TerminalRiskManagementCommand command = TerminalRiskManagerFactory.CreateCommand(_fixture);

        _probabilitySelectionQueue.Setup(m => m.IsRandomSelection(
            It.Is<Probability>(x => x.Equals(command.GetRandomSelectionTargetPercentage()))))
            .Returns(Task.FromResult(false));

        TerminalVerificationResult tvr = TerminalVerificationResult.Create();

        //Act
        TerminalRiskManagementResponse actual = await _systemUnderTest.Process(command);

        //Assert
        Assert.Equal(tvr, actual.GetTerminalVerificationResult());
        Assert.Equal(TransactionStatusInformationFlags.NotAvailable, actual.GetTransactionStatus());
    }
}
