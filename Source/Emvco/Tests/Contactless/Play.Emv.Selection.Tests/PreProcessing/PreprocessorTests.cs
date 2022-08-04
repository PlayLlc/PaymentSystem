using AutoFixture;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Outcomes;
using Play.Emv.Selection.Contracts;
using Play.Emv.Selection.Start;
using Play.Globalization;
using Play.Globalization.Time;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Selection.Tests.PreProcessing;

public class PreprocessorTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly Preprocessor _SystemUnderTest;

    #endregion

    #region Constructor

    public PreprocessorTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Fixture.RegisterGlobalizationProperties();

        _SystemUnderTest = new Preprocessor();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void Preprocessor_InvokingSetPreprocessingIndicatorsNoIndicatorWithContactlessApplicationNotAllowed_OutcomeParametersDoNotGetSet()
    {
        //Arrange
        
        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterAmmountAuthorizedNumeric(200);
        _Fixture.RegisterCultureProfile();

        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, true, true, true, true);

        PreProcessingIndicators preprocessingIndicators = new PreProcessingIndicators(new[] { transactionProfile });

        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        Transaction transaction = _Fixture.Create<Transaction>();
        OutcomeParameterSet expectedOutcomeParameterSet = transaction.GetOutcome().GetOutcomeParameterSet();
        //Act
        _SystemUnderTest.SetPreprocessingIndicators(transaction.GetOutcome(), preprocessingIndicators, transaction.GetAmountAuthorizedNumeric(), cultureProfile);

        //Assert
        Outcome outcome = transaction.GetOutcome();

        OutcomeParameterSet outcomeParameterSet = outcome.GetOutcomeParameterSet();
        bool exists = outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);

        Assert.False(exists);
        Assert.Null(userInterfaceRequestData);
        Assert.Equal(expectedOutcomeParameterSet, outcomeParameterSet);
    }

    [Fact]
    public void Preprocessor_InvokingSetPreprocessingIndicatorsWithAmountAuthorizedGreaterThanReaderContactlessTransactionLimit_OutcomeParametersAreSet()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b0010_1100_1010_1011);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterAmmountAuthorizedNumeric(2000);
        _Fixture.RegisterCultureProfile();

        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, false, false, true, false);

        PreProcessingIndicators preprocessingIndicators = new PreProcessingIndicators(new[] { transactionProfile });

        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        Transaction transaction = _Fixture.Create<Transaction>();
        OutcomeParameterSet expectedOutcomeParameterSet = SetOutcomeParameter(transaction.GetOutcome().GetOutcomeParameterSet());
        UserInterfaceRequestData expectedUserInterfaceRequestData = SetUserInterfaceRequestData();

        //Act
        _SystemUnderTest.SetPreprocessingIndicators(transaction.GetOutcome(), preprocessingIndicators, transaction.GetAmountAuthorizedNumeric(), cultureProfile);

        Outcome outcome = transaction.GetOutcome();

        OutcomeParameterSet outcomeParameterSet = outcome.GetOutcomeParameterSet();
        bool exists = outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);

        //Assert
        Assert.True(exists);
        Assert.NotNull(userInterfaceRequestData);
        Assert.Equal(expectedUserInterfaceRequestData, userInterfaceRequestData);
        Assert.Equal(expectedOutcomeParameterSet, outcomeParameterSet);
    }

    [Fact]
    public void Preprocessor_InvokingSetPreprocessingIndicatorsWithAtLeastOneTransactionWithZeroAmmountAllowedForOfflineAndZeroAmountForAuthoriserd_OutcomeParametersAreSet()
    {
        //Arrange
        _Fixture.RegisterTerminalTransactionQualifiers(0b0010_1100_1010_1011);
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();
        _Fixture.RegisterAmmountAuthorizedNumeric(0);
        _Fixture.RegisterCultureProfile();

        TransactionProfile transactionProfile = PreProcessingIndicatorFactory.CreateTransactionProfile(_Fixture, false, false, true, false);

        PreProcessingIndicators preprocessingIndicators = new PreProcessingIndicators(new[] { transactionProfile });

        CultureProfile cultureProfile = _Fixture.Create<CultureProfile>();

        Transaction transaction = _Fixture.Create<Transaction>();
        OutcomeParameterSet expectedOutcomeParameterSet = SetOutcomeParameter(transaction.GetOutcome().GetOutcomeParameterSet());
        UserInterfaceRequestData expectedUserInterfaceRequestData = SetUserInterfaceRequestData();

        //Act
        _SystemUnderTest.SetPreprocessingIndicators(transaction.GetOutcome(), preprocessingIndicators, transaction.GetAmountAuthorizedNumeric(), cultureProfile);

        Outcome outcome = transaction.GetOutcome();

        OutcomeParameterSet outcomeParameterSet = outcome.GetOutcomeParameterSet();
        bool exists = outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);

        //Assert
        Assert.True(exists);
        Assert.NotNull(userInterfaceRequestData);
        Assert.Equal(expectedUserInterfaceRequestData, userInterfaceRequestData);
        Assert.Equal(expectedOutcomeParameterSet, outcomeParameterSet);
    }

    private OutcomeParameterSet SetOutcomeParameter(OutcomeParameterSet outcomeParameter)
    {
        OutcomeParameterSet.Builder outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();

        outcomeParameterSetBuilder.Set(StatusOutcomes.TryAnotherInterface);
        outcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(true);
        outcomeParameterSetBuilder.SetIsUiRequestOnRestartPresent(false);
        outcomeParameterSetBuilder.SetIsDataRecordPresent(false);
        outcomeParameterSetBuilder.SetIsDiscretionaryDataPresent(false);
        outcomeParameterSetBuilder.Set(new Milliseconds(0));

        return outcomeParameter |= outcomeParameterSetBuilder.Complete();
    }

    private UserInterfaceRequestData SetUserInterfaceRequestData()
    {
        UserInterfaceRequestData.Builder userInterfaceSetter = UserInterfaceRequestData.GetBuilder();

        userInterfaceSetter.Set(MessageIdentifiers.PleaseInsertOrSwipeCard);
        userInterfaceSetter.Set(Statuses.ProcessingError);

        return userInterfaceSetter.Complete();
    }

    #endregion
}
