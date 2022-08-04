using AutoFixture;

using Moq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Selection.Tests.ProtocolActivator;

public class ProtocolActivatorTests
{
    #region Instance Values

    private readonly IFixture _Fixture;

    private readonly Mock<IHandleDisplayRequests> _DisplayProcess;
    private readonly Mock<IHandlePcdRequests> _ProximityCouplingDeviceEndpoint;

    private readonly Start.ProtocolActivator _SystemUnderTest;

    #endregion

    #region Constructor

    public ProtocolActivatorTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Fixture.RegisterGlobalizationProperties();

        _DisplayProcess = new Mock<IHandleDisplayRequests>(MockBehavior.Strict);
        _ProximityCouplingDeviceEndpoint = new Mock<IHandlePcdRequests>(MockBehavior.Strict);

        _SystemUnderTest = new Start.ProtocolActivator(_ProximityCouplingDeviceEndpoint.Object, _DisplayProcess.Object);
    }

    #endregion

    [Fact]
    public void OutcomeWithNoRestartNeeded_InvokingProtocolActivator_ProcessActivationIsNotARestart()
    {
        //Arrange
        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Outcome outcome = Outcome.Default;

        _Fixture.RegisterTerminalTransactionQualifiers();
        _Fixture.RegisterReaderContactlessTransactionLimit(1234);
        _Fixture.RegisterReaderCvmRequiredLimit(1234);
        _Fixture.RegisterTerminalFloorLimit(123);
        _Fixture.RegisterTerminalCategoriesSupportedList();

        DisplayMessageRequest expectedDisplayMessageRequest = GetExpectedReadyToReadDisplayMessage();
        _DisplayProcess.Setup(m => m.Request(It.Is<DisplayMessageRequest>(x => x.Equals(expectedDisplayMessageRequest))));

        TransactionProfile preProcessingIndicator = ProtocolActivatorFactory.CreatePreProcessingIndicator(_Fixture);

        TransactionProfile[] entryPointConfigurations = new[] { preProcessingIndicator };

        PreProcessingIndicators preprocessingIndicators = new(entryPointConfigurations);

        CandidateList candidateList = new();

        //Act
        _SystemUnderTest.ActivateProtocol(transactionSessionId, outcome, preprocessingIndicators, candidateList);

        //Assert
        _DisplayProcess.Verify(m => m.Request(It.Is<DisplayMessageRequest>(x => x.Equals(expectedDisplayMessageRequest))), Times.Once);
    }

    private static DisplayMessageRequest GetExpectedReadyToReadDisplayMessage()
    {
        UserInterfaceRequestData.Builder? builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(MessageIdentifiers.PresentCard);
        builder.Set(Statuses.ReadyToRead);

        return new DisplayMessageRequest(builder.Complete());
    }
}
