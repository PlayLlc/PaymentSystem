using System;
using System.Linq;

using AutoFixture;

using Moq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Contracts;
using Play.Messaging;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Selection.Tests.ProtocolActivator;

public class ProtocolActivatorTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly Mock<IEndpointClient> _Endpoint;
    private readonly Start.ProtocolActivator _SystemUnderTest;

    #endregion

    #region Constructor

    public ProtocolActivatorTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Fixture.RegisterGlobalizationProperties();

        _Endpoint = new Mock<IEndpointClient>(MockBehavior.Strict);

        _SystemUnderTest = new Start.ProtocolActivator(_Endpoint.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void OutcomeWithNoRestartNeeded_InvokingProtocolActivator_EachCombinationEntryPointIsResetedProcessActivationIsNotARestart()
    {
        //Arrange
        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Outcome outcome = Outcome.Default;

        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        UserInterfaceRequestData expectedUserInterfaceRequestData = GetExpectedReadyToReadDisplayMessage();

        _Endpoint.Setup(m => m.Send(It.Is<DisplayMessageRequest>(x => x.GetUserInterfaceRequestData().Equals(expectedUserInterfaceRequestData))));
        _Endpoint.Setup(m => m.Send(It.Is<ActivatePcdRequest>(x => x.GetTransactionSessionId().Equals(transactionSessionId))));

        TransactionProfile preProcessingIndicator = SelectionFactory.CreateTransactionProfile(_Fixture, _Fixture.Create<bool>(), _Fixture.Create<bool>(),
            _Fixture.Create<bool>(), _Fixture.Create<bool>());

        TransactionProfile[] entryPointConfigurations = new[] {preProcessingIndicator};

        PreProcessingIndicators preprocessingIndicators = new(entryPointConfigurations);

        CandidateList candidateList = new();

        //Act
        _SystemUnderTest.ActivateProtocol(transactionSessionId, outcome, preprocessingIndicators, candidateList);

        //Assert
        _Endpoint.Verify(m => m.Send(It.Is<DisplayMessageRequest>(x => x.GetUserInterfaceRequestData().Equals(expectedUserInterfaceRequestData))), Times.Once);
        _Endpoint.Verify(m => m.Send(It.Is<ActivatePcdRequest>(x => x.GetTransactionSessionId().Equals(transactionSessionId))), Times.Once);

        preprocessingIndicators.Values.All(x =>
        {
            Assert.False(x.ContactlessApplicationNotAllowed);
            Assert.False(x.ReaderContactlessFloorLimitExceeded);
            Assert.False(x.ReaderCvmRequiredLimitExceeded);
            Assert.False(x.StatusCheckRequested);
            Assert.False(x.ZeroAmount);

            return true;
        });
    }

    [Fact]
    public void OutcomeWithNoRestartNeededButWithErrorPresent_InvokingProtocolActivator_CandidateListIsCleared()
    {
        //Arrange
        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Outcome outcome = Outcome.Default;
        SetErrorIndicationPresent(outcome);

        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        UserInterfaceRequestData expectedUserInterfaceRequestData = GetExpectedReadyToReadDisplayMessage();

        _Endpoint.Setup(m => m.Send(It.Is<DisplayMessageRequest>(x => x.GetUserInterfaceRequestData().Equals(expectedUserInterfaceRequestData))));
        _Endpoint.Setup(m => m.Send(It.Is<ActivatePcdRequest>(x => x.GetTransactionSessionId().Equals(transactionSessionId))));

        TransactionProfile preProcessingIndicator = SelectionFactory.CreateTransactionProfile(_Fixture, _Fixture.Create<bool>(), _Fixture.Create<bool>(),
            _Fixture.Create<bool>(), _Fixture.Create<bool>());

        TransactionProfile[] entryPointConfigurations = new[] {preProcessingIndicator};

        PreProcessingIndicators preprocessingIndicators = new(entryPointConfigurations);

        CandidateList candidateList = new();

        candidateList.Add(_Fixture.Create<Combination>());
        candidateList.Add(_Fixture.Create<Combination>());
        candidateList.Add(_Fixture.Create<Combination>());

        //Act
        _SystemUnderTest.ActivateProtocol(transactionSessionId, outcome, preprocessingIndicators, candidateList);

        //Assert
        Assert.Equal(0, candidateList.Count);
    }

    [Fact]
    public void OutcomeWithRestartNeededWithUiRequestOnRestartPresentButMissingUserInterfaceData_InvokingProtocolActivator_ThrowsException()
    {
        //Arrange
        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Outcome outcome = new(BuildOutcomeParameterWithRestartRequired());

        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        UserInterfaceRequestData expectedUserInterfaceRequestData = GetExpectedReadyToReadDisplayMessage();

        _Endpoint.Setup(m => m.Send(It.Is<DisplayMessageRequest>(x => x.GetUserInterfaceRequestData().Equals(expectedUserInterfaceRequestData))));
        _Endpoint.Setup(m => m.Send(It.Is<ActivatePcdRequest>(x => x.GetTransactionSessionId().Equals(transactionSessionId))));

        TransactionProfile preProcessingIndicator = SelectionFactory.CreateTransactionProfile(_Fixture, _Fixture.Create<bool>(), _Fixture.Create<bool>(),
            _Fixture.Create<bool>(), _Fixture.Create<bool>());

        TransactionProfile[] entryPointConfigurations = new[] {preProcessingIndicator};

        PreProcessingIndicators preprocessingIndicators = new(entryPointConfigurations);

        CandidateList candidateList = new();

        //Act & Assert

        Assert.Throws<InvalidOperationException>(() =>
        {
            _SystemUnderTest.ActivateProtocol(transactionSessionId, outcome, preprocessingIndicators, candidateList);
        });
    }

    [Fact]
    public void OutcomeWithRestartNeededWithUiRequestOnRestartPresent_InvokingProtocolActivator_DisplayProcessRequestsExistingRequestData()
    {
        //Arrange
        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Outcome outcome = new(BuildOutcomeParameterWithRestartRequired());
        SetUserInterfaceRequestData(outcome);

        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        UserInterfaceRequestData expectedUserInterfaceRequestData = GetExpectedReadyToReadDisplayMessage();

        _Endpoint.Setup(m => m.Send(It.Is<DisplayMessageRequest>(x => x.GetUserInterfaceRequestData().Equals(expectedUserInterfaceRequestData))));
        _Endpoint.Setup(m => m.Send(It.Is<ActivatePcdRequest>(x => x.GetTransactionSessionId().Equals(transactionSessionId))));

        TransactionProfile preProcessingIndicator = SelectionFactory.CreateTransactionProfile(_Fixture, _Fixture.Create<bool>(), _Fixture.Create<bool>(),
            _Fixture.Create<bool>(), _Fixture.Create<bool>());

        TransactionProfile[] entryPointConfigurations = new[] {preProcessingIndicator};

        PreProcessingIndicators preprocessingIndicators = new(entryPointConfigurations);

        CandidateList candidateList = new();

        //Act
        _SystemUnderTest.ActivateProtocol(transactionSessionId, outcome, preprocessingIndicators, candidateList);

        //Assert
        _Endpoint.Verify(m => m.Send(It.Is<DisplayMessageRequest>(x => x.GetUserInterfaceRequestData().Equals(expectedUserInterfaceRequestData))), Times.Once);
        _Endpoint.Verify(m => m.Send(It.Is<ActivatePcdRequest>(x => x.GetTransactionSessionId().Equals(transactionSessionId))), Times.Once);
    }

    [Fact]
    public void OutcomeWithRestartNeededWithUiRequestOnRestartPresent_InvokingProtocolActivator_DisplayProcessRequestsReadyToDisplayMessage()
    {
        //Arrange
        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Outcome outcome = new();

        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();

        builder.Set(StatusOutcomes.SelectNext);
        builder.SetIsUiRequestOnRestartPresent(false);

        outcome.Reset(builder.Complete());

        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        UserInterfaceRequestData expectedUserInterfaceRequestData = GetExpectedReadyToReadDisplayMessage();

        _Endpoint.Setup(m => m.Send(It.Is<DisplayMessageRequest>(x => x.GetUserInterfaceRequestData().Equals(expectedUserInterfaceRequestData))));
        _Endpoint.Setup(m => m.Send(It.Is<ActivatePcdRequest>(x => x.GetTransactionSessionId().Equals(transactionSessionId))));

        TransactionProfile preProcessingIndicator = SelectionFactory.CreateTransactionProfile(_Fixture, _Fixture.Create<bool>(), _Fixture.Create<bool>(),
            _Fixture.Create<bool>(), _Fixture.Create<bool>());

        TransactionProfile[] entryPointConfigurations = new[] {preProcessingIndicator};

        PreProcessingIndicators preprocessingIndicators = new(entryPointConfigurations);

        CandidateList candidateList = new();

        //Act
        _SystemUnderTest.ActivateProtocol(transactionSessionId, outcome, preprocessingIndicators, candidateList);

        //Assert
        _Endpoint.Verify(m => m.Send(It.Is<DisplayMessageRequest>(x => x.GetUserInterfaceRequestData().Equals(expectedUserInterfaceRequestData))), Times.Once);
        _Endpoint.Verify(m => m.Send(It.Is<ActivatePcdRequest>(x => x.GetTransactionSessionId().Equals(transactionSessionId))), Times.Once);
    }

    private static UserInterfaceRequestData GetExpectedReadyToReadDisplayMessage()
    {
        UserInterfaceRequestData.Builder? builder = UserInterfaceRequestData.GetBuilder();

        builder.Set(DisplayMessageIdentifiers.PresentCard);
        builder.Set(DisplayStatuses.ReadyToRead);

        return builder.Complete();
    }

    private static void SetUserInterfaceRequestData(Outcome outcome)
    {
        UserInterfaceRequestData.Builder? builder = UserInterfaceRequestData.GetBuilder();

        builder.Set(DisplayMessageIdentifiers.PresentCard);
        builder.Set(DisplayStatuses.ReadyToRead);

        outcome.Update(builder);
    }

    private static OutcomeParameterSet BuildOutcomeParameterWithRestartRequired()
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();

        builder.Set(StatusOutcomes.TryAgain);

        return builder.Complete();
    }

    private static void SetErrorIndicationPresent(Outcome outcome)
    {
        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();

        builder.Set(Level1Error.ProtocolError);

        outcome.Reset(builder.Complete());
    }

    #endregion
}