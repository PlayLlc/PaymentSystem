using System;

using AutoFixture;

using Moq;

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

        ActivatePcdResponse response = new ActivatePcdResponse(correlationId, true, error, transactionSessionId);

        //Act & Assert
        Assert.Throws<InvalidOperationException>(() => _SystemUnderTest.HandleCardCollisions(response, outcome));
    }

    [Fact]
    public void HandleCardCollision_CollisionIsDetectedWithUserInterfaceRequestDataPresent_CollisionIsHandled()
    {
        //Arrange
        Outcome outcome = Outcome.Default;



        TransactionSessionId transactionSessionId = _Fixture.Create<TransactionSessionId>();
        Level1Error error = Level1Error.Ok;
        CorrelationId correlationId = _Fixture.Create<CorrelationId>();

        ActivatePcdResponse response = new ActivatePcdResponse(correlationId, true, error, transactionSessionId);

        //Act & Assert
        Assert.Throws<InvalidOperationException>(() => _SystemUnderTest.HandleCardCollisions(response, outcome));
    }

    #endregion
}
