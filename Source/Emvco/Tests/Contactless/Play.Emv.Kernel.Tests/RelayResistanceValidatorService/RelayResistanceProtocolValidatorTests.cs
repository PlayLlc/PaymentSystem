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
        bool isThresholdReached = _SystemUnderTest.IsRetryThresholdHit(1);

        //Assert
        Assert.False(isThresholdReached);
    }

    [Fact]
    public void RelayResistanceProtocolValidatorValidatorWithNoRetries_IsRetryThresholdHit_ReturnsTrue()
    {
        //Arrange
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 0;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        //Act
        bool isThresHoldReached = _SystemUnderTest.IsRetryThresholdHit(1);

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

        TransactionType transactionType = new(14);

        TransactionSessionId differentSessionId = new(transactionType);
        Microseconds timeElapsed = new(1000);

        //Act & Assert

        Assert.Throws<TerminalDataException>(() =>
        {
            bool isInRange = _SystemUnderTest.IsInRange(differentSessionId, timeElapsed, _Database);
        });
    }

    [Fact]
    public void RelayResistanceProtocolValidator_IsInRange_ReturnsTrue()
    {
        //Arrange
        RegisterTerminaRelayResistanceProtocolTimes(_Database, 15, 12, 10, 8);
        RegisterMinimumRelayResistanceConfigurationTimes(_Database, 3, 2);
        RegisterMaximumRelayResistanceConfigurationTimes(_Database, 17, 10);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(1000);

        //Act
        bool isInRange = _SystemUnderTest.IsInRange(sessionId, timeElapsed, _Database);

        //Assert
        Assert.True(isInRange);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_ProcessingTimeExceedsMaximumProcessedRelayTime_ReturnsFalse()
    {
        //Arrange
        RegisterTerminaRelayResistanceProtocolTimes(_Database,  25, 12, 10, 8);
        RegisterMinimumRelayResistanceConfigurationTimes(_Database, 3, 2);
        RegisterMaximumRelayResistanceConfigurationTimes(_Database, 5, 4);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(1000);

        //Act
        bool isInRange = _SystemUnderTest.IsInRange(sessionId, timeElapsed, _Database);

        //Assert
        Assert.False(isInRange);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_ProcessingTimeSucceedsMinimumProcessedRelayTime_ReturnsFalse()
    {
        //Arrange
        RegisterTerminaRelayResistanceProtocolTimes(_Database, 15, 12, 10, 8);
        RegisterMinimumRelayResistanceConfigurationTimes(_Database, 8, 2);
        RegisterMaximumRelayResistanceConfigurationTimes(_Database, 17, 10);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(1000);

        //Act
        bool isInRange = _SystemUnderTest.IsInRange(sessionId, timeElapsed, _Database);

        //Assert
        Assert.False(isInRange);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_MinTimeForProcessingRelayResistanceIs0_ReturnsTrue()
    {
        //Arrange
        RegisterTerminaRelayResistanceProtocolTimes(_Database, 15, 12, 10, 8);
        RegisterMinimumRelayResistanceConfigurationTimes(_Database, 3, 2);
        RegisterMaximumRelayResistanceConfigurationTimes(_Database, 17, 10);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(1000);

        //Act
        bool isInRange = _SystemUnderTest.IsInRange(sessionId, timeElapsed, _Database);

        //Assert
        Assert.True(isInRange);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_MaxTimeForProcessingRelayResistanceIs0_ReturnsFalse()
    {
        //Arrange
        RegisterTerminaRelayResistanceProtocolTimes(_Database, 15, 12, 10, 8);
        RegisterMinimumRelayResistanceConfigurationTimes(_Database, 3, 2);
        RegisterMaximumRelayResistanceConfigurationTimes(_Database, 0, 0);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(1000);

        //Act
        bool isInRange = _SystemUnderTest.IsInRange(sessionId, timeElapsed, _Database);

        //Assert
        Assert.False(isInRange);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_CalculateMeasuredRrpTime_ReturnsExpectedResult()
    {
        //Arrange
        RegisterTerminaRelayResistanceProtocolTimes(_Database, 15, 12, 10, 8);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(1000);

        MeasuredRelayResistanceProcessingTime expected = new(10 - (15 - 10));

        //Act
        MeasuredRelayResistanceProcessingTime actual = _SystemUnderTest.CalculateMeasuredRrpTime(timeElapsed, _Database);

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_CalculateMeasuredRrpTimeWhenElapsedTimeIs0_AlwaysReturns0RelaySeconds()
    {
        //Arrange
        ushort terminalExpectedTransmissionTimeForCapduRelaySeconds = _Fixture.Create<ushort>();
        ushort terminalExpectedTransmissionTimeForRapduRelaySeconds = _Fixture.Create<ushort>();
        ushort deviceEstimatedTransmissionTimeForRapduRelaySeconds = _Fixture.Create<ushort>();
        ushort deviceResistanceEntropyRelaySeconds = _Fixture.Create<ushort>();

        RegisterTerminaRelayResistanceProtocolTimes(_Database, terminalExpectedTransmissionTimeForCapduRelaySeconds,
            terminalExpectedTransmissionTimeForRapduRelaySeconds, deviceEstimatedTransmissionTimeForRapduRelaySeconds, deviceResistanceEntropyRelaySeconds);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(0);

        MeasuredRelayResistanceProcessingTime expected = new(0);

        //Act
        MeasuredRelayResistanceProcessingTime actual = _SystemUnderTest.CalculateMeasuredRrpTime(timeElapsed, _Database);

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_ProcessedRrpTimeIsGreaterThenMinimumProcessed_ReturnsTrue()
    {
        //Arrange
        RegisterMinimumRelayResistanceConfigurationTimes(_Database, 12, 8);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(0);

        MeasuredRelayResistanceProcessingTime value = new(5);

        //Act
        bool isWithingMinimumRange = _SystemUnderTest.IsRelayResistanceWithinMinimumRange(value, _Database);

        //Assert
        Assert.True(isWithingMinimumRange);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_ProcessedRrpTimeIsSmallerThenMinimumProcessed_ReturnsFalse()
    {
        //Arrange
        RegisterMinimumRelayResistanceConfigurationTimes(_Database, 12, 8);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        Microseconds timeElapsed = new(0);

        MeasuredRelayResistanceProcessingTime value = new(3);

        //Act
        bool isWithingMinimumRange = _SystemUnderTest.IsRelayResistanceWithinMinimumRange(value, _Database);

        //Assert
        Assert.False(isWithingMinimumRange);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_ProcessedRrpTimeIsSmallerThenMaximumProcessed_ReturnsTrue()
    {
        //Arrange
        RegisterMaximumRelayResistanceConfigurationTimes(_Database, 12, 8);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        MeasuredRelayResistanceProcessingTime value = new(17);

        //Act
        bool isWithingMinimumRange = _SystemUnderTest.IsRelayResistanceWithinMaximumRange(value, _Database);

        //Assert
        Assert.True(isWithingMinimumRange);
    }

    [Fact]
    public void RelayResistanceProtocolValidator_ProcessedRrpTimeIsGreaterThenMaximumProcessed_ReturnsTrue()
    {
        //Arrange
        RegisterMaximumRelayResistanceConfigurationTimes(_Database, 8, 4);

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        int maxNrOfRetries = 2;

        _SystemUnderTest = new RelayResistanceProtocolValidator(sessionId, maxNrOfRetries);

        MeasuredRelayResistanceProcessingTime value = new(17);

        //Act
        bool isWithingMinimumRange = _SystemUnderTest.IsRelayResistanceWithinMaximumRange(value, _Database);

        //Assert
        Assert.False(isWithingMinimumRange);
    }

    private void RegisterTerminaRelayResistanceProtocolTimes(
        ITlvReaderAndWriter kernelDb, ushort terminalExpectedTransmissionTimeForCapduRelaySeconds, ushort terminalExpectedTransmissionTimeForRapduRelaySeconds,
        ushort deviceEstimatedTransmissionTimeForRapduRelaySeconds, ushort deviceResistanceEntropyRelaySeconds)
    {
        //C-APDU : terminal sending to Device
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedTransmissionTimeForRelayResistanceCapdu =
            new(terminalExpectedTransmissionTimeForCapduRelaySeconds);

        kernelDb.Update(terminalExpectedTransmissionTimeForRelayResistanceCapdu);

        //R-APDU : terminal reading from Device
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedTransmissionTimeForRelayResistanceRapdu =
            new(new RelaySeconds(terminalExpectedTransmissionTimeForRapduRelaySeconds));

        kernelDb.Update(terminalExpectedTransmissionTimeForRelayResistanceRapdu);

        //Device R-APDU : device sending to terminal(Kernel).
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceEstimatedTransmissionTimeForRelayResistanceRapdu =
            new(new RelaySeconds(deviceEstimatedTransmissionTimeForRapduRelaySeconds));

        kernelDb.Update(deviceEstimatedTransmissionTimeForRelayResistanceRapdu);

        DeviceRelayResistanceEntropy deviceRelayResistanceEntropy = new(new RelaySeconds(deviceResistanceEntropyRelaySeconds));

        kernelDb.Update(deviceRelayResistanceEntropy);
    }

    private void RegisterMinimumRelayResistanceConfigurationTimes(
        ITlvReaderAndWriter kernelDb, ushort minTimeForProcessingApduRelaySeconds, ushort minimumResistanceGracePeriodRelaySeconds)
    {
        MinTimeForProcessingRelayResistanceApdu minTimeForProcessing = new(minTimeForProcessingApduRelaySeconds);

        kernelDb.Update(minTimeForProcessing);

        MinimumRelayResistanceGracePeriod minimumRelayResistanceGracePeriod = new(minimumResistanceGracePeriodRelaySeconds);

        kernelDb.Update(minimumRelayResistanceGracePeriod);
    }

    private void RegisterMaximumRelayResistanceConfigurationTimes(
        ITlvReaderAndWriter kernelDb, ushort maxTimeForProcessingResistanceApduRelaySeconds, ushort maximumResistanceGracePeriodRelaySeconds)
    {
        MaxTimeForProcessingRelayResistanceApdu maxTimeForProcessingRelayResistanceApdu = new(new RelaySeconds(maxTimeForProcessingResistanceApduRelaySeconds));

        kernelDb.Update(maxTimeForProcessingRelayResistanceApdu);

        MaximumRelayResistanceGracePeriod maximumRelayResistanceGracePeriod = new(new RelaySeconds(maximumResistanceGracePeriodRelaySeconds));

        kernelDb.Update(maximumRelayResistanceGracePeriod);
    }

    #endregion
}