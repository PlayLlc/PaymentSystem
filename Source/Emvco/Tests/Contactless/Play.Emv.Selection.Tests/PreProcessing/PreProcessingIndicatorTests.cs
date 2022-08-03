using AutoFixture;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Selection.Contracts;
using Play.Globalization;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Selection.Tests.PreProcessing;

public class PreProcessingIndicatorTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly ITlvReaderAndWriter _Database;

    #endregion

    #region Constructor

    public PreProcessingIndicatorTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);

        _Fixture.RegisterGlobalizationProperties();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void PreProcessingIndicator_Instantiate_IsNotNull()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, true, true, true, true);

        //Act
        PreProcessingIndicator preprocessingIndicator = new PreProcessingIndicator(transactionProfile);

        //Assert
        Assert.NotNull(preprocessingIndicator);
    }

    [Fact]
    public void PreProcessingIndicator_ResetPreprocessingIndicators_ReturnsExpectedResult()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, true, true, true, true);
        PreProcessingIndicator sut = new PreProcessingIndicator(transactionProfile);

        //Act
        sut.ResetPreprocessingIndicators();

        //Assert
        //Flags not set
        Assert.False(sut.ContactlessApplicationNotAllowed);
        Assert.False(sut.ReaderContactlessFloorLimitExceeded);
        Assert.False(sut.ReaderCvmRequiredLimitExceeded);
        Assert.False(sut.StatusCheckRequested);
        Assert.False(sut.ZeroAmount);
    }

    [Fact]
    public void PreProcessingIndicator_ResetTerminalTransactionQualifiers_ReturnsExpectedResult()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b1110_1100_1010_0011);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, true, true, true, true);
        TerminalTransactionQualifiers expectedTtq = transactionProfile.GetTerminalTransactionQualifiers().AsValueCopy();

        PreProcessingIndicator sut = new PreProcessingIndicator(transactionProfile);

        sut.TerminalTransactionQualifiers.SetCvmRequired();
        sut.TerminalTransactionQualifiers.SetOnlineCryptogramRequired();

        //Act
        sut.ResetTerminalTransactionQualifiers();

        //Assert
        Assert.Equal(expectedTtq, sut.TerminalTransactionQualifiers);
    }

    [Fact]
    public void PreProcessingIndicator_SetMutableFieldsStatusCheckRequestIsSetAndAuthorizedAmountIsSingleUnitOfCurrency_OnlineCryptogramRequiredIsSet()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b0010_1100_1010_0011);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, true, false, false, false);

        PreProcessingIndicator sut = new PreProcessingIndicator(transactionProfile);

        AmountAuthorizedNumeric authorizedAmount = new AmountAuthorizedNumeric(100);
        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        //Act
        sut.Set(authorizedAmount, cultureProfile);

        //Assert
        Assert.True(sut.TerminalTransactionQualifiers.IsOnlineCryptogramRequired());
        Assert.True(sut.StatusCheckRequested);
    }

    [Fact]
    public void PreProcessingIndicator_SetMutableFieldsReaderContactlessFloorLimitExceededHasBeenSet_OnlineCryptogramRequiredIsSet()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b0010_1100_1010_0011);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, true, false, false, false);

        PreProcessingIndicator sut = new PreProcessingIndicator(transactionProfile);

        AmountAuthorizedNumeric authorizedAmount = new AmountAuthorizedNumeric(2200);
        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        //Act
        sut.Set(authorizedAmount, cultureProfile);

        //Assert
        Assert.True(sut.TerminalTransactionQualifiers.IsOnlineCryptogramRequired());
        Assert.True(sut.ReaderContactlessFloorLimitExceeded);
    }

    [Fact]
    public void PreProcessingIndicator_SetMutableFieldsZeroAmountHasBeenSetAndTtqHasReaderOnlineCapableConfigured_SetOnlineCryptogramRequiredIsSet()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b0010_1100_1010_0011);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        //ZeroAmountHasBeenSetEvent? zeroAmountHasBeenSet = SetZeroAmount(amountAuthorizedMoney, _TransactionProfile.IsZeroAmountAllowedForOffline());
        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, false, false, true, false);

        PreProcessingIndicator sut = new PreProcessingIndicator(transactionProfile);

        AmountAuthorizedNumeric authorizedAmount = new AmountAuthorizedNumeric(0);
        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        //Act
        sut.Set(authorizedAmount, cultureProfile);

        //Assert
        Assert.True(sut.TerminalTransactionQualifiers.IsOnlineCryptogramRequired());
        Assert.True(sut.ZeroAmount);
    }

    [Fact]
    public void PreProcessingIndicator_SetMutableFieldsZeroAmountHasBeenSet_ContactlessApplicationNotAllowedIsSet()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b0010_1100_1010_1011); //IsBitSet(4);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        //ZeroAmountHasBeenSetEvent? zeroAmountHasBeenSet = SetZeroAmount(amountAuthorizedMoney, _TransactionProfile.IsZeroAmountAllowedForOffline());
        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, false, false, true, false);

        PreProcessingIndicator sut = new PreProcessingIndicator(transactionProfile);

        AmountAuthorizedNumeric authorizedAmount = new AmountAuthorizedNumeric(0);
        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        //Act
        sut.Set(authorizedAmount, cultureProfile);

        //Assert
        Assert.True(sut.ContactlessApplicationNotAllowed);
    }

    [Fact]
    public void PreProcessingIndicator_SetMutableFieldsAmountAuthorizedNumericIsGreaterThanReaderContactlessTransactionLimit_ContactlessApplicationNotAllowedIsSet()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b0010_1100_1010_1011); //IsBitSet(4);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        //ZeroAmountHasBeenSetEvent? zeroAmountHasBeenSet = SetZeroAmount(amountAuthorizedMoney, _TransactionProfile.IsZeroAmountAllowedForOffline());
        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, false, false, true, false);

        PreProcessingIndicator sut = new PreProcessingIndicator(transactionProfile);

        AmountAuthorizedNumeric authorizedAmount = new AmountAuthorizedNumeric(1456);
        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        //Act
        sut.Set(authorizedAmount, cultureProfile);

        //Assert
        Assert.True(sut.ContactlessApplicationNotAllowed);
    }

    [Fact]
    public void PreProcessingIndicator_SetMutableFieldsReaderCvmRequiredLimitExceededHasBeenSet_CvmRequiredIsSet()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b0010_1100_1010_1011); //IsBitSet(4);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        //ZeroAmountHasBeenSetEvent? zeroAmountHasBeenSet = SetZeroAmount(amountAuthorizedMoney, _TransactionProfile.IsZeroAmountAllowedForOffline());
        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, false, false, true, false);

        PreProcessingIndicator sut = new PreProcessingIndicator(transactionProfile);

        AmountAuthorizedNumeric authorizedAmount = new AmountAuthorizedNumeric(1456);
        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        //Act
        sut.Set(authorizedAmount, cultureProfile);

        //Assert
        Assert.True(sut.TerminalTransactionQualifiers.IsCvmRequired());
        Assert.True(sut.ReaderCvmRequiredLimitExceeded);
    }

    #endregion
}
