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

    private readonly Mock<IHandleDisplayRequests> _DisplayProcess;
    private readonly CardCollisionHandler _SystemUnderTest;

    #endregion

    #region Constructor

    public CardCollisionHandlerTests()
    {
        _Fixture = new ContactlessFixture().Create();
        RegisterFixtures(_Fixture);

        _DisplayProcess = new Mock<IHandleDisplayRequests>(MockBehavior.Strict);
        _SystemUnderTest = new CardCollisionHandler(_DisplayProcess.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void HandleCardCollision_CollisionIsDetectedButNoUserInterfaceRequestDataPresent_ExceptionIsThrown()
    {
        //Arrange
        Outcome outcome = Outcome.Default;

        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new ActivatePcdResponse(correlationId, false, error, transactionSessionId);

        //Act & Assert
        Assert.Throws<InvalidOperationException>(() => _SystemUnderTest.HandleCardCollisions(response, outcome));
    }

    [Fact]
    public void HandleCardCollision_CollisionIsDetectedWithUserInterfaceRequestDataPresentButWithDifferentMessageThanPleasePresentOneCardOnly_NoCollisionIsHandled()
    {
        //Arrange
        Outcome outcome = Outcome.Default;

        SetUserInterfaceRequestData(outcome, MessageIdentifiers.PresentCard);

        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new ActivatePcdResponse(correlationId, false, error, transactionSessionId);

        //Act
        _SystemUnderTest.HandleCardCollisions(response, outcome);

        //Assert
        _DisplayProcess.Verify(m => m.Request(It.IsAny<DisplayMessageRequest>()), Times.Never);
    }

    [Fact]
    public void HandleCardCollision_CollisionIsDetectedWithCorrectMessageIdentifier_CollisionIsHandled()
    {
        //Arrange
        Outcome outcome = Outcome.Default;

        SetUserInterfaceRequestData(outcome, MessageIdentifiers.PleasePresentOneCardOnly);

        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new ActivatePcdResponse(correlationId, true, error, transactionSessionId);

        _DisplayProcess.Setup(m => m.Request(It.IsAny<DisplayMessageRequest>()));

        //Act
        _SystemUnderTest.HandleCardCollisions(response, outcome);
        outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? output);

        //Assert
        _DisplayProcess.Verify(m => m.Request(It.IsAny<DisplayMessageRequest>()), Times.Once);

        Assert.NotNull(output);
        Assert.Equal(MessageIdentifiers.PleasePresentOneCardOnly, output.GetMessageIdentifier());
        Assert.Equal(Statuses.ProcessingError, output.GetStatus());
    }

    [Fact]
    public void HandleCardCollision_NoCollisionDetected_CardCollisionHasBeenResolved()
    {
        //Arrange
        Outcome outcome = Outcome.Default;

        SetUserInterfaceRequestData(outcome, MessageIdentifiers.PleasePresentOneCardOnly);

        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new ActivatePcdResponse(correlationId, false, error, transactionSessionId);

        _DisplayProcess.Setup(m => m.Request(It.IsAny<DisplayMessageRequest>()));

        //Act
        _SystemUnderTest.HandleCardCollisions(response, outcome);
        outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? output);

        //Assert
        _DisplayProcess.Verify(m => m.Request(It.IsAny<DisplayMessageRequest>()), Times.Once);

        Assert.NotNull(output);
        Assert.Equal(MessageIdentifiers.PleasePresentOneCardOnly, output.GetMessageIdentifier());
        Assert.Equal(Statuses.ReadyToRead, output.GetStatus());
    }

    private static void SetUserInterfaceRequestData(Outcome outcome, params MessageIdentifiers[] messageIdentifiers)
    {
        UserInterfaceRequestData.Builder? builder = UserInterfaceRequestData.GetBuilder();

        foreach (MessageIdentifiers messageIdentifier in messageIdentifiers)
            builder.Set(messageIdentifier);

        builder.Set(Statuses.ReadyToRead);

        outcome.Update(builder);
    }

    private static UserInterfaceRequestData GetExpectedSetUserInterfaceRequestData()
    {
        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        builder.Set(MessageIdentifiers.PleasePresentOneCardOnly);
        builder.Set(Statuses.ProcessingError);

        return builder.Complete();
    }

    private static void RegisterFixtures(IFixture fixture)
    {
        fixture.Freeze<TransactionSessionId>();
        fixture.Freeze<CorrelationId>();
    }

    #endregion
}
