using System;
using System.Linq;

using AutoFixture;

using Moq;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Messaging;
using Play.Testing.Emv.Contactless.AutoFixture;
using Play.Testing.Emv.Contactless.AutoFixture.Configuration;
using Play.Testing.Emv.Contactless.Stubs;

using Xunit;

namespace Play.Emv.Kernel2.Tests.StateMachine.State01;

public class IdleTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly KernelDatabase _Database;
    private readonly Mock<IGenerateUnpredictableNumber> _UnpredictableNumberGenerator;

    private readonly DataExchangeKernelService _DataExchangeKernelService;
    private readonly Mock<IGetKernelState> _KernelStateResolver;
    private readonly Mock<IEndpointClient> _EndpointClient;
    private readonly Mock<IManageTornTransactions> _TornTransactionLog;

    private readonly Idle _SystemUnderTest;

    #endregion

    #region Constructor

    public IdleTests()
    {
        _Fixture = new ContactlessFixture().Create();
        KernelId masterCardKernel = new KernelId(2);
        _Fixture.Register(() => masterCardKernel);

        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture, new ContactlessFixtureBuilderOptions
        {
             InitializeDefaultConfigurationData = false,
              ActivateKernelDbOnInitialization = false
        });

        _KernelStateResolver = new Mock<IGetKernelState>(MockBehavior.Strict);
        _EndpointClient = new Mock<IEndpointClient>(MockBehavior.Strict);
        _TornTransactionLog = new Mock<IManageTornTransactions>(MockBehavior.Strict);
        _DataExchangeKernelService = new(_EndpointClient.Object, _Database);

        _UnpredictableNumberGenerator = new Mock<IGenerateUnpredictableNumber>(MockBehavior.Strict);

        _SystemUnderTest = new Idle(_Database, _DataExchangeKernelService, _EndpointClient.Object, _TornTransactionLog.Object, _KernelStateResolver.Object, _UnpredictableNumberGenerator.Object);
        _KernelStateResolver.Setup(m => m.GetKernelState(Idle.StateId)).Returns(_SystemUnderTest);
    }

    #endregion

    #region Instance Members

    #region ACT

    [Fact]
    public void KernelState_ProcessActHandleOutOfSyncState_OutOfSyncExceptionIsThrown()
    {
        //Arrange
        KernelSession session = _Fixture.Create<KernelSession>();
        KernelId kernelId = _Fixture.Create<KernelId>();

        KernelSessionId outofSyncSessionId = new KernelSessionId(kernelId, new TransactionSessionId(new TransactionType(34)));

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);
        ActivateKernelRequest activeKernelRequest = new ActivateKernelRequest(outofSyncSessionId, new Play.Ber.DataObjects.PrimitiveValue[0], rapdu);

        //Act & Assert
        Assert.Throws<RequestOutOfSyncException>(() => _SystemUnderTest.Handle(session, activeKernelRequest));
    }

    [Fact]
    public void KernelState_ProcessActWithAlreadyInitializedDb_ThrowsTerminalDataException()
    {
        //Arrange
        Kernel2Session session = _Fixture.Create<Kernel2Session>();
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);
        ActivateKernelRequest activeKernelRequest = new ActivateKernelRequest(kernelSessionId, new Play.Ber.DataObjects.PrimitiveValue[0], rapdu);

        _EndpointClient.Setup(m => m.Send(It.Is<OutKernelResponse>(response => response.GetTransaction().GetTransactionSessionId() == session.GetTransactionSessionId())));

        _Database.Activate(session.GetTransactionSessionId());

        //Act & Assert
        Assert.Throws<InvalidOperationException>(() => _SystemUnderTest.Handle(session, activeKernelRequest));
    }

    [Fact]
    public void KernelState_ProcessActFieldOffDetectionNotSupportedAndMissingPdolData_ReturnsS2()
    {
        //Arrange
        Kernel2Session session = _Fixture.Create<Kernel2Session>();
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);

        CardholderName tlv = new("testuser");
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b10);

        ActivateKernelRequest activeKernelRequest = new(kernelSessionId, new PrimitiveValue[] { tlv, applicationCapabilitiesInformation }, rapdu);

        _UnpredictableNumberGenerator.Setup(m => m.GenerateUnpredictableNumber()).Returns(new UnpredictableNumber(314));

        _EndpointClient.Setup(m => m.Send(It.IsAny<GetProcessingOptionsRequest>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryKernelResponse>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryTerminalRequest>()));

        _KernelStateResolver.Setup(m => m.GetKernelState(WaitingForPdolData.StateId)).Returns(new WaitingForPdolDataStub());

        //Act
        KernelState? expectedState = _SystemUnderTest.Handle(session, activeKernelRequest);

        //Assert
        bool success = _Database.TryGet(OutcomeParameterSet.Tag, out OutcomeParameterSet? outcomeParameterSet);

        Assert.True(success);
        Assert.Equal(new FieldOffRequestOutcome(0), outcomeParameterSet?.GetFieldOffRequestOutcome());
        Assert.IsAssignableFrom<WaitingForPdolData>(expectedState);
    }

    [Fact]
    public void KernelState_ProcessActFieldOffDetectionNotSupportedAndPdolDataPresent_ReturnsS3()
    {
        //Arrange
        Kernel2Session session = _Fixture.Create<Kernel2Session>();
        session.SetIsPdolDataMissing(false);

        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);

        CardholderName tlv = new("testuser");
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b10);

        ActivateKernelRequest activeKernelRequest = new(kernelSessionId, new PrimitiveValue[] { tlv, applicationCapabilitiesInformation }, rapdu);

        _UnpredictableNumberGenerator.Setup(m => m.GenerateUnpredictableNumber()).Returns(new UnpredictableNumber(314));

        _EndpointClient.Setup(m => m.Send(It.IsAny<GetProcessingOptionsRequest>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryKernelResponse>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryTerminalRequest>()));

        _KernelStateResolver.Setup(m => m.GetKernelState(WaitingForGpoResponse.StateId)).Returns(new WaitingForGpoResponseStub());

        //Act
        KernelState expectedState = _SystemUnderTest.Handle(session, activeKernelRequest);

        //Assert
        bool success = _Database.TryGet(OutcomeParameterSet.Tag, out OutcomeParameterSet? outcomeParameterSet);

        Assert.True(success);
        Assert.Equal(new FieldOffRequestOutcome(0), outcomeParameterSet?.GetFieldOffRequestOutcome());
        Assert.IsAssignableFrom<WaitingForGpoResponse>(expectedState);
    }

    [Fact]
    public void KernelState_ProcessActSignalWithFieldOffDetectionNotSupportedAndCardDoesNotSupportIdsAndPdolDataMissing_IdsReadIsNotSet()
    {
        //Arrange
        Kernel2Session session = _Fixture.Create<Kernel2Session>();
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);

        CardholderName tlv = new("testuser");
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b10);

        ActivateKernelRequest activeKernelRequest = new(kernelSessionId, new PrimitiveValue[] { tlv, applicationCapabilitiesInformation }, rapdu);

        _UnpredictableNumberGenerator.Setup(m => m.GenerateUnpredictableNumber()).Returns(new UnpredictableNumber(314));

        _EndpointClient.Setup(m => m.Send(It.IsAny<GetProcessingOptionsRequest>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryKernelResponse>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryTerminalRequest>()));

        _KernelStateResolver.Setup(m => m.GetKernelState(WaitingForPdolData.StateId)).Returns(new WaitingForPdolDataStub());

        //Act
        KernelState expectedState = _SystemUnderTest.Handle(session, activeKernelRequest);

        //Assert
        bool success = _Database.TryGet(OutcomeParameterSet.Tag, out OutcomeParameterSet? outcomeParameterSet);

        IntegratedDataStorageStatus ids = _Database.Get<IntegratedDataStorageStatus>(IntegratedDataStorageStatus.Tag);

        Assert.False(ids.IsReadSet());
        Assert.IsAssignableFrom<WaitingForPdolData>(expectedState);
    }

    [Fact]
    public void KernelState_ProcessActSignalWithFieldOffDetectionNotSupportedAndCardSupportsIdsAndPdolDataMissing_IdsIsReadSet()
    {
        //Arrange
        Kernel2Session session = _Fixture.Create<Kernel2Session>();
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);

        CardholderName tlv = new("testuser");
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b0100_0000_0000_0000_0000_0000);
        DataStorageId dataStorageId = new DataStorageId(123);

        ActivateKernelRequest activeKernelRequest = new(kernelSessionId, new PrimitiveValue[] { tlv, applicationCapabilitiesInformation, dataStorageId }, rapdu);

        _UnpredictableNumberGenerator.Setup(m => m.GenerateUnpredictableNumber()).Returns(new UnpredictableNumber(314));

        _EndpointClient.Setup(m => m.Send(It.IsAny<GetProcessingOptionsRequest>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryKernelResponse>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryTerminalRequest>()));

        _KernelStateResolver.Setup(m => m.GetKernelState(WaitingForPdolData.StateId)).Returns(new WaitingForPdolDataStub());

        //Act
        KernelState expectedState = _SystemUnderTest.Handle(session, activeKernelRequest);

        //Assert
        IntegratedDataStorageStatus ids = _Database.Get<IntegratedDataStorageStatus>(IntegratedDataStorageStatus.Tag);

        Assert.True(ids.IsReadSet());
        Assert.IsAssignableFrom<WaitingForPdolData>(expectedState);
    }

    //GetProcessingOptionsRequest - Handle PDOL Data is empty
    [Fact]
    public void KernelState_ProcessActSignalWithFieldOffDetectionNotSupportedAndCardSupportsIdsAndPdolDataMissing_SendRequestWithEmptyPDOLValues()
    {
        //Arrange
        Kernel2Session session = _Fixture.Create<Kernel2Session>();
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);

        CardholderName tlv = new("testuser");
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b0100_0000_0000_0000_0000_0000);
        DataStorageId dataStorageId = new DataStorageId(123);

        ActivateKernelRequest activeKernelRequest = new(kernelSessionId, new PrimitiveValue[] { tlv, applicationCapabilitiesInformation, dataStorageId }, rapdu);

        _UnpredictableNumberGenerator.Setup(m => m.GenerateUnpredictableNumber()).Returns(new UnpredictableNumber(314));

        GetProcessingOptionsRequest expectedProcessingOptRequestWithEmptyCommandTemplate = GetProcessingOptionsRequest.Create(session.GetTransactionSessionId());
        _EndpointClient.Setup(m => m.Send(It.Is<GetProcessingOptionsRequest>(request => request.GetCommandTemplate() == null)));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryKernelResponse>()));
        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryTerminalRequest>()));

        _KernelStateResolver.Setup(m => m.GetKernelState(WaitingForPdolData.StateId)).Returns(new WaitingForPdolDataStub());

        //Act
        KernelState expectedState = _SystemUnderTest.Handle(session, activeKernelRequest);

        //Assert
        _EndpointClient.Verify(m => m.Send(It.Is<GetProcessingOptionsRequest>(request => request.GetCommandTemplate() == null)), Times.Once);
    }

    [Fact]
    public void KernelState_ProcessActSignalWithFieldOffDetectionNotSupportedAndCardSupportsIdsAndPdolDataMissing_DekServiceSendsExpectedResonse()
    {
        //Arrange
        Kernel2Session session = _Fixture.Create<Kernel2Session>();
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);

        CardholderName cardHolderName = new("testuser");
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b0100_0000_0000_0000_0000_0000);
        DataStorageId dataStorageId = new DataStorageId(123);

        ActivateKernelRequest activeKernelRequest = new(kernelSessionId, new PrimitiveValue[] { cardHolderName, applicationCapabilitiesInformation, dataStorageId }, rapdu);

        _UnpredictableNumberGenerator.Setup(m => m.GenerateUnpredictableNumber()).Returns(new UnpredictableNumber(314));

        GetProcessingOptionsRequest expectedProcessingOptRequestWithEmptyCommandTemplate = GetProcessingOptionsRequest.Create(session.GetTransactionSessionId());
        _EndpointClient.Setup(m => m.Send(It.Is<GetProcessingOptionsRequest>(request => request.GetCommandTemplate() == null)));

        EmvCodec codec = EmvCodec.GetCodec();

        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryTerminalRequest>()));

        PrimitiveValue[] expectedPrimitivesToSend = new PrimitiveValue[] { dataStorageId, applicationCapabilitiesInformation };
        byte[] expectedEncodedValue = expectedPrimitivesToSend.SelectMany(a => a.EncodeTagLengthValue(codec)).ToArray();

        _EndpointClient.Setup(m => m.Send(It.Is<QueryKernelResponse>(response => response.GetDataToSend().EncodeValue().SequenceEqual(expectedEncodedValue))));

        _KernelStateResolver.Setup(m => m.GetKernelState(WaitingForPdolData.StateId)).Returns(new WaitingForPdolDataStub());

        //Act
        KernelState expectedState = _SystemUnderTest.Handle(session, activeKernelRequest);

        //Assert
        _EndpointClient.Verify(m => m.Send(It.Is<QueryKernelResponse>(response => response.GetTransactionSessionId() == session.GetTransactionSessionId())), Times.Once);
    }

    [Fact]
    public void KernelState_ProcessActSignalWithFieldOffDetectionNotSupportedAndCardSupportsIdsAndPdolDataMissing_DekServiceSendsExpectedRequest()
    {
        //Arrange
        Kernel2Session session = _Fixture.Create<Kernel2Session>();
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();

        SelectApplicationDefinitionFileInfoResponse rapdu = ContactlessFixture.CreateSelectApplicationDefinitionFileInfoResponse(_Fixture);

        CardholderName cardHolderName = new("testuser");
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b0100_0000_0000_0000_0000_0000);
        DataStorageId dataStorageId = new DataStorageId(123);

        ActivateKernelRequest activeKernelRequest = new(kernelSessionId, new PrimitiveValue[] { cardHolderName, applicationCapabilitiesInformation, dataStorageId }, rapdu);

        _UnpredictableNumberGenerator.Setup(m => m.GenerateUnpredictableNumber()).Returns(new UnpredictableNumber(314));

        GetProcessingOptionsRequest expectedProcessingOptRequestWithEmptyCommandTemplate = GetProcessingOptionsRequest.Create(session.GetTransactionSessionId());
        _EndpointClient.Setup(m => m.Send(It.Is<GetProcessingOptionsRequest>(request => request.GetCommandTemplate() == null)));

        EmvCodec codec = EmvCodec.GetCodec();

        _EndpointClient.Setup(m => m.Send(It.IsAny<QueryKernelResponse>()));

        Tag[] expectedDataNeeded = new[] { TagsToRead.Tag, TagsToWriteBeforeGeneratingApplicationCryptogram.Tag };

        _EndpointClient.Setup(m => m.Send(It.Is<QueryTerminalRequest>(request => request.GetDataNeeded().GetDataObjects().SequenceEqual(expectedDataNeeded))));

        _KernelStateResolver.Setup(m => m.GetKernelState(WaitingForPdolData.StateId)).Returns(new WaitingForPdolDataStub());

        //Act
        KernelState expectedState = _SystemUnderTest.Handle(session, activeKernelRequest);

        //Assert
        _EndpointClient.Verify(m => m.Send(It.Is<QueryTerminalRequest>(request => request.GetTransactionSessionId() == session.GetTransactionSessionId())), Times.Once);
    }

    #endregion

    #region CLEAN

    [Fact]
    public void IdleState_ProcessCleanSignal_ReturnsExpectedResult()
    {
        //Arrange
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();
        CleanKernelRequest cleanKernelRequest = new CleanKernelRequest(kernelSessionId);

        _Database.Activate(kernelSessionId.GetTransactionSessionId());

        _TornTransactionLog.Setup(m => m.CleanOldRecords(_DataExchangeKernelService, DekResponseType.DiscretionaryData));
        _EndpointClient.Setup(m => m.Send(It.Is<OutKernelResponse>(i => i.GetKernelSessionId() == kernelSessionId)));

        //Act
        KernelState state = _SystemUnderTest.Handle(cleanKernelRequest);

        //Assert
        Assert.Equal(state.GetStateId(), Idle.StateId);
        _TornTransactionLog.Verify(m => m.CleanOldRecords(_DataExchangeKernelService, DekResponseType.DiscretionaryData), Times.Once);
    }

    #endregion

    #region STOP

    [Fact]
    public void IdleState_ProcessStopHandleOutOfSyncSignal_ThrowsOutOfSyncException()
    {
        //Arrange
        KernelSession session = _Fixture.Create<KernelSession>();
        KernelId kernelId = _Fixture.Create<KernelId>();

        KernelSessionId outofSyncSessionId = new KernelSessionId(kernelId, new TransactionSessionId(new TransactionType(34)));

        StopKernelRequest stopKernelRequest = new StopKernelRequest(outofSyncSessionId);

        //Act & Assert
        Assert.Throws<RequestOutOfSyncException>(() => _SystemUnderTest.Handle(session, stopKernelRequest));
    }

    [Fact]
    public void IdleState_ProcessStopSignal_ReturnsExpectedResult()
    {
        //Arrange
        KernelSessionId kernelSessionId = _Fixture.Create<KernelSessionId>();
        KernelSession session = _Fixture.Create<KernelSession>();

        _Database.Activate(kernelSessionId.GetTransactionSessionId());

        StopKernelRequest stopKernelRequest = new StopKernelRequest(kernelSessionId);
        _EndpointClient.Setup(m => m.Send(It.Is<OutKernelResponse>(i => i.GetKernelSessionId() == kernelSessionId)));

        //Act
        KernelState state = _SystemUnderTest.Handle(session, stopKernelRequest);

        //Assert
        Assert.Equal(state.GetStateId(), Idle.StateId);
        OutcomeParameterSet outcomeParameterSet = _Database.Get<OutcomeParameterSet>(OutcomeParameterSet.Tag);
        Assert.Equal(StatusOutcomes.EndApplication, outcomeParameterSet.GetStatusOutcome());
    }

    #endregion

    #endregion
}
