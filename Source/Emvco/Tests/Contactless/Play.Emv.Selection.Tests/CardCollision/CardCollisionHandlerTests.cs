using System;

using AutoFixture;

using Moq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Start;
using Play.Messaging;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Selection.Tests.CardCollision;

public class CardCollisionHandlerTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly Mock<IEndpointClient> _DisplayProcess;
    private readonly CardCollisionHandler _SystemUnderTest;

    #endregion

    #region Constructor

    public CardCollisionHandlerTests()
    {
        _Fixture = new ContactlessFixture().Create();
        RegisterFixtures(_Fixture);

        _DisplayProcess = new Mock<IEndpointClient>(MockBehavior.Strict);
        _SystemUnderTest = new CardCollisionHandler(_DisplayProcess.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void HandleCardCollision_CollisionIsDetectedButNoUserInterfaceRequestDataPresent_ExceptionIsThrown()
    {
        //Arrange
        Outcome outcome = new();

        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new(correlationId, false, error, transactionSessionId);

        //Act & Assert
        Assert.Throws<InvalidOperationException>(() => _SystemUnderTest.HandleCardCollisions(response, outcome));
    }

    [Fact]
    public void
        HandleCardCollision_CollisionIsDetectedWithUserInterfaceRequestDataPresentButWithDifferentMessageThanPleasePresentOneCardOnly_NoCollisionIsHandled()
    {
        //Arrange
        Outcome outcome = new();

        SetUserInterfaceRequestData(outcome, DisplayMessageIdentifiers.PresentCard);

        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new(correlationId, false, error, transactionSessionId);

        //Act
        _SystemUnderTest.HandleCardCollisions(response, outcome);

        //Assert
        _DisplayProcess.Verify(m => m.Send(It.IsAny<DisplayMessageRequest>()), Times.Never);
    }

    [Fact]
    public void HandleCardCollision_CollisionIsDetectedWithCorrectMessageIdentifier_CollisionIsHandled()
    {
        //Arrange
        Outcome outcome = new();

        SetUserInterfaceRequestData(outcome, DisplayMessageIdentifiers.PleasePresentOneCardOnly);

        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new(correlationId, true, error, transactionSessionId);

        _DisplayProcess.Setup(m => m.Send(It.IsAny<DisplayMessageRequest>()));

        //Act
        _SystemUnderTest.HandleCardCollisions(response, outcome);
        outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? output);

        //Assert
        _DisplayProcess.Verify(m => m.Send(It.IsAny<DisplayMessageRequest>()), Times.Once);

        Assert.NotNull(output);
        Assert.Equal(DisplayMessageIdentifiers.PleasePresentOneCardOnly, output.GetMessageIdentifier());
        Assert.Equal(DisplayStatuses.ProcessingError, output.GetStatus());
    }

    [Fact]
    public void HandleCardCollision_NoCollisionDetected_CardCollisionHasBeenResolved()
    {
        //Arrange
        Outcome outcome = new();

        SetUserInterfaceRequestData(outcome, DisplayMessageIdentifiers.PleasePresentOneCardOnly);

        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new(correlationId, false, error, transactionSessionId);

        _DisplayProcess.Setup(m => m.Send(It.IsAny<DisplayMessageRequest>()));

        //Act
        _SystemUnderTest.HandleCardCollisions(response, outcome);
        outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? output);

        //Assert
        _DisplayProcess.Verify(m => m.Send(It.IsAny<DisplayMessageRequest>()), Times.Once);

        Assert.NotNull(output);
        Assert.Equal(DisplayStatuses.ReadyToRead, output.GetStatus());
        Assert.Equal(DisplayMessageIdentifiers.PleasePresentOneCardOnly, output.GetMessageIdentifier());
    }

    private static void SetUserInterfaceRequestData(Outcome outcome, DisplayMessageIdentifiers messageIdentifier)
    {
        UserInterfaceRequestData.Builder? builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(messageIdentifier);
        outcome.Update(builder);
    }

    private static void RegisterFixtures(IFixture fixture)
    {
        fixture.Freeze<TransactionSessionId>();
        fixture.Freeze<CorrelationId>();
    }

    #endregion
}