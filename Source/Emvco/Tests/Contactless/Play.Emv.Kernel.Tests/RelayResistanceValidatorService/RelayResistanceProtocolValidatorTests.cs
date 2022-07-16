using AutoFixture;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Services;
using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.RelayResistanceValidatorService;

public class RelayResistanceProtocolValidatorTests : TestBase
{
    #region Instance values

    private readonly IFixture _Fixture;
    private readonly ITlvReaderAndWriter _Database;

    private IValidateRelayResistanceProtocol _SystemUnderTest;

    #endregion

    #region Constructor

    public RelayResistanceProtocolValidatorTests()
    {
        _Fixture = new ContactlessFixture().Create();

        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);

        RelayResistanceProtocolValidatorFixture.RegisterTransactionSessionId(_Fixture);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void RelayResistanceProtocolValidatorValidator_CreateNewValidator_RelayResistanceProtocolValidatorIsCreated()
    {
        //Arrange
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        //Act
        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        //Assert
        Assert.NotNull(_SystemUnderTest);
    }

    [Fact]
    public void RelayResistanceProtocolValidatorValidator_IsRetryThresholdHit_ReturnsFalse()
    {
        //Arrange
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);
        //Act
        bool isThresHoldReached = _SystemUnderTest.IsRetryThresholdHit();

        //Assert
        Assert.False(isThresHoldReached);
    }

    [Fact]
    public void RelayResistanceProtocolValidatorValidatorWithNoRetries_IsRetryThresholdHit_ReturnsTrue()
    {
        //Arrange
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 0;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);
        //Act
        bool isThresHoldReached = _SystemUnderTest.IsRetryThresholdHit();

        //Assert
        Assert.True(isThresHoldReached);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_IsInRangeForDifferentTransactionSessionId_ExceptionIsThrown()
    {
        //Arrange
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        TransactionType transactionType = new TransactionType(14);

        TransactionSessionId differentSessionId = new TransactionSessionId(transactionType);
        Milliseconds timeElapsed = new Milliseconds(1000);

        //Act & Assert

        Assert.Throws<TerminalDataException>(() =>
        {
            bool isInRange = _SystemUnderTest.IsInRange(differentSessionId, timeElapsed, _Database);
        });
    }

    [Fact]
    public void RelayResistanceProtocolValidator_IsInRange_ReturnsExpectedResult()
    {
        //Arrange
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        TransactionType transactionType = new TransactionType(14);

        TransactionSessionId differentSessionId = new TransactionSessionId(transactionType);
        Milliseconds timeElapsed = new Milliseconds(1000);

        RegisterRelayResistanceApduDataElements(_Database);

        //Act
        bool isInRange = _SystemUnderTest.IsInRange(sessionId, timeElapsed, _Database);

        //Assert
        Assert.True(isInRange);
    }

    private void RegisterRelayResistanceApduDataElements(ITlvReaderAndWriter kernelDb)
    {
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceEstimatedTransmissionTimeForRelayResistanceRapdu =
            new DeviceEstimatedTransmissionTimeForRelayResistanceRapdu(new RelaySeconds(4));

        kernelDb.Update(deviceEstimatedTransmissionTimeForRelayResistanceRapdu);

        DeviceRelayResistanceEntropy deviceRelayResistanceEntropy = new DeviceRelayResistanceEntropy(new RelaySeconds(8));

        kernelDb.Update(deviceRelayResistanceEntropy);

        MinTimeForProcessingRelayResistanceApdu minTimeForProcessing = new MinTimeForProcessingRelayResistanceApdu(2);

        kernelDb.Update(minTimeForProcessing);

        MaxTimeForProcessingRelayResistanceApdu maxTimeForProcessing = new MaxTimeForProcessingRelayResistanceApdu(5);

        kernelDb.Update(maxTimeForProcessing);
    }

    #endregion
}
