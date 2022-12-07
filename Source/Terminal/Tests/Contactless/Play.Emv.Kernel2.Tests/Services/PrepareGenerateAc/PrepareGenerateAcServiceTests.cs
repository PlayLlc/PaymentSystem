using System;
using System.Linq;

using AutoFixture;

using Moq;

using Play.Ber.InternalFactories;
using Play.Core.Extensions.IEnumerable;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services.PrepareGenerateAc;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;
using Play.Testing.Emv.Ber.Primitive;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

using Message = Play.Messaging.Message;

namespace Play.Emv.Kernel2.Tests.Services.PrepareGenerateAc;

public class PrepareGenerateAcServiceTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly KernelDatabase _Database;
    private readonly Mock<DataExchangeKernelService> _DataExchangeKernelService;
    private readonly Mock<IGetKernelState> _KernelStateResolver;
    private readonly Mock<IEndpointClient> _EndpointClient;
    private readonly Mock<IGetKernelStateId> _KernelStateIdRetriever;
    private readonly PrepareGenerateAcService _SystemUnderTest;

    #endregion

    #region Constructor

    public PrepareGenerateAcServiceTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);

        MaxNumberOfTornTransactionLogRecords maxNumberOfTornTransactionLogRecords = new(2);
        _Database.Update(maxNumberOfTornTransactionLogRecords);

        _Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _Fixture.Behaviors.Remove(b));
        _Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _DataExchangeKernelService = new Mock<DataExchangeKernelService>(MockBehavior.Strict);
        _KernelStateResolver = new Mock<IGetKernelState>(MockBehavior.Strict);
        _EndpointClient = new Mock<IEndpointClient>(MockBehavior.Strict);
        _KernelStateIdRetriever = new Mock<IGetKernelStateId>(MockBehavior.Strict);

        _SystemUnderTest = new PrepareGenerateAcService(_Database, _DataExchangeKernelService.Object, _KernelStateResolver.Object, _EndpointClient.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void PrepareGenerateAcService_ProcessOutOfSyncRequest_ThrowsRequestOutOfSyncException()
    {
        //Arrange
        StateId currentStateId = new(new char[] {'s', 't', 'a', 't', 'e', '1'});
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        //Act & Assert
        Assert.Throws<RequestOutOfSyncException>(() => _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message));
    }

    [Fact]
    public void PrepareGenerateAcServiceNoIds_ProcessDatabaseIsIdsReadFlagNotSetWithInvalidOdaStatusType_SendGenerateAcCommandAndReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.NotAvailable);

        Message message = _Fixture.Create<Message>();

        CardRiskManagementDataObjectList1RelatedDataTestTlv testProcessingData = new();
        CardRiskManagementDataObjectList1RelatedData crmdrol = CardRiskManagementDataObjectList1RelatedData.Decode(testProcessingData.EncodeValue().AsSpan());
        _Database.Update(crmdrol);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        GenerateApplicationCryptogramRequest expectedGenerateACRequest =
            GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(), referenceControlParameter, crmdrol);
        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    //gac.21-gac.22
    [Fact]
    public void PrepareGenerateAcServiceNoIdsWithCDAFailed_ProcessCdaFailed_SendGenerateAcCommandAndReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        _Database.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);

        _Database.Update(KernelConfiguration.Default);

        CardRiskManagementDataObjectList1RelatedDataTestTlv testProcessingData = new();
        CardRiskManagementDataObjectList1RelatedData crmdrol = CardRiskManagementDataObjectList1RelatedData.Decode(testProcessingData.EncodeValue().AsSpan());
        _Database.Update(crmdrol);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        GenerateApplicationCryptogramRequest expectedGenerateACRequest =
            GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(), referenceControlParameter, crmdrol);
        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    //gac21-gac24
    [Fact]
    public void PrepareGenerateAcServiceNoIdsWithACTypeDifferentThenAAC_ProcessIDSReadOnly_SendGenerateAcCommandAndReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);
        kernel2Session.Update(CryptogramTypes.AuthorizationRequestCryptogram);

        Message message = _Fixture.Create<Message>();

        _Database.Update(KernelConfiguration.Default);

        CardRiskManagementDataObjectList1 cardRiskManagementDataObjectList = CardRiskManagementDataObjectList1.Decode(new byte[] {13, 2, 12, 38}.AsSpan());
        _Database.Update(cardRiskManagementDataObjectList);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            new(cardRiskManagementDataObjectList.AsDataObjectListResult(_Database));

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(),
            referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);
        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    //GAC25
    [Fact]
    public void PrepareGenerateAcServiceNoIdsWithCdaSupportedByApplicationForAcTypes_ProcessIDSReadOnly_SendGenerateAcCommandAndReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        _Database.Update(KernelConfiguration.Default);

        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b1_0000_0000);
        _Database.Update(applicationCapabilitiesInformation);

        CardRiskManagementDataObjectList1 cardRiskManagementDataObjectList = CardRiskManagementDataObjectList1.Decode(new byte[] {13, 2, 12, 38}.AsSpan());
        _Database.Update(cardRiskManagementDataObjectList);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            new(cardRiskManagementDataObjectList.AsDataObjectListResult(_Database));

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(),
            referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);
        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    //GAC26
    [Fact]
    public void PrepareGenerateAcServiceNoIdsCDAFailed_Process_SetReferenceControlParameterWithoutCdaSignature()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        _Database.Update(KernelConfiguration.Default);

        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b1_000_0000);
        _Database.Update(applicationCapabilitiesInformation);

        CardRiskManagementDataObjectList1RelatedDataTestTlv testProcessingData = new();
        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            CardRiskManagementDataObjectList1RelatedData.Decode(testProcessingData.EncodeValue().AsSpan());
        _Database.Update(cardRiskManagementDataObjectList1RelatedData);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(),
            referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);
        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    //  GAC.2
    [Fact]
    public void
        PrepareGenerateAcServiceWithIdsFlagSetAndCDAFailedAndCardholderVerificationSupported_Process_SendGenerateAcCommandAndUpdateReferenceControlParameter()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        //Set Ids Flags
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId = new(12);
        _Database.Update(dataStorageRequestedOperatorId);
        DataStorageVersionNumberTerminal dataStorageVersionNumberTerminal = new(15);
        _Database.Update(dataStorageVersionNumberTerminal);
        IntegratedDataStorageStatus integratedDataStorageStatus = new(0b1000_0000);
        _Database.Update(integratedDataStorageStatus);

        //Set CDA Failed in TVR
        _Database.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);

        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new(0b1_000_0000);
        _Database.Update(applicationCapabilitiesInformation);

        //Cardholder Verification Supported
        KernelConfiguration kernelConfiguration = new(0b100_000);
        _Database.Update(kernelConfiguration);
        ApplicationInterchangeProfile applicationInterchangeProfile = new(0b10_0000_0000);
        _Database.Update(applicationInterchangeProfile);

        CardRiskManagementDataObjectList1RelatedDataTestTlv testProcessingData = new();
        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            CardRiskManagementDataObjectList1RelatedData.Decode(testProcessingData.EncodeValue().AsSpan());
        _Database.Update(cardRiskManagementDataObjectList1RelatedData);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(),
            referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);
        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
        Assert.Equal(kernel2Session.GetApplicationCryptogramType(), CryptogramTypes.ApplicationAuthenticationCryptogram);
    }

    //  GAC.3
    [Fact]
    public void
        PrepareGenerateAcServiceWithIdsFlagSetAndCDAFailedAndIsIntegratedDataStorageReadOnly_ProcessReadIds_SendGenerateAcCommandAndUpdateReferenceControlParameter()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        //Set Ids Flags
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId = new(12);
        _Database.Update(dataStorageRequestedOperatorId);
        DataStorageVersionNumberTerminal dataStorageVersionNumberTerminal = new(15);
        _Database.Update(dataStorageVersionNumberTerminal);
        IntegratedDataStorageStatus integratedDataStorageStatus = new(0b1000_0000);
        _Database.Update(integratedDataStorageStatus);

        CardRiskManagementDataObjectList1 cardRiskManagementDataObjectList = CardRiskManagementDataObjectList1.Decode(new byte[] {13, 2, 12, 38}.AsSpan());
        _Database.Update(cardRiskManagementDataObjectList);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            new(cardRiskManagementDataObjectList.AsDataObjectListResult(_Database));

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(),
            referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);
        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
        Assert.Equal(kernel2Session.GetApplicationCryptogramType(), CryptogramTypes.ApplicationAuthenticationCryptogram);
    }

    //  GAC.5-GAC.6
    [Fact]
    public void
        PrepareGenerateAcServiceWithIdsFlagSetAndCDAFailedAndDataStorageApplicationCryptogramTypeIsMissing_ProcessAndHandleError_CurrentStateIdIsReturned()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        //Set Ids Flags
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId = new(12);
        _Database.Update(dataStorageRequestedOperatorId);
        DataStorageVersionNumberTerminal dataStorageVersionNumberTerminal = new(15);
        _Database.Update(dataStorageVersionNumberTerminal);
        IntegratedDataStorageStatus integratedDataStorageStatus = new(0b1000_0000);
        _Database.Update(integratedDataStorageStatus);

        CardRiskManagementDataObjectList1 cardRiskManagementDataObjectList = CardRiskManagementDataObjectList1.Decode(new byte[] {13, 2, 12, 38}.AsSpan());
        _Database.Update(cardRiskManagementDataObjectList);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            new(cardRiskManagementDataObjectList.AsDataObjectListResult(_Database));

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(),
            referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);
        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //GAC.5-GAC.6
        DataStorageApplicationCryptogramType dataStorageApplicationCryptogramType = new(12);
        _Database.Update(dataStorageApplicationCryptogramType);

        DataStorageOperatorDataSetInfo dataStorageOperatorDataSetInfo = new(12);
        _Database.Update(dataStorageOperatorDataSetInfo);

        DataStorageDataObjectList dataStorageDataObjectList = new(Array.Empty<TagLength>());
        _Database.Update(dataStorageDataObjectList);

        //Act
        StateId stateId = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        ErrorIndication errorIndication = _Database.Get<ErrorIndication>(ErrorIndication.Tag);
        Level2Error actual = errorIndication.GetL2();
        Assert.Equal(Level2Error.IdsDataError, actual);
    }

    // GAC.7 - GAC-9
    [Fact]
    public void PrepareGenerateAcServiceAcTypeRankedHigherThanDsAcType_ProcessIdsWrite_CurrentStateIdIsReturned()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        //Set Ids Flags
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId = new(12);
        _Database.Update(dataStorageRequestedOperatorId);
        DataStorageVersionNumberTerminal dataStorageVersionNumberTerminal = new(15);
        _Database.Update(dataStorageVersionNumberTerminal);
        IntegratedDataStorageStatus integratedDataStorageStatus = new(0b1000_0000);
        _Database.Update(integratedDataStorageStatus);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        //CardRiskManagementDataObjectList1RelatedData
        CardRiskManagementDataObjectList1 cardRiskManagementDataObjectList = CardRiskManagementDataObjectList1.Decode(new byte[] {13, 2, 12, 38}.AsSpan());
        _Database.Update(cardRiskManagementDataObjectList);

        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            new(cardRiskManagementDataObjectList.AsDataObjectListResult(_Database));
        _Database.Update(cardRiskManagementDataObjectList1RelatedData);

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(),
            referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);

        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        DataStorageOperatorDataSetInfo dataStorageOperatorDataSetInfo = new(12);
        _Database.Update(dataStorageOperatorDataSetInfo);

        DataStorageDataObjectList dataStorageDataObjectList = new(Array.Empty<TagLength>());
        _Database.Update(dataStorageDataObjectList);

        DataStorageOperatorDataSetInfoForReader dataStorageOperatorDataSetInfoForReader = new(0b10);
        _Database.Update(dataStorageOperatorDataSetInfoForReader);

        //CryptogramTypes.ApplicationAuthenticationCryptogram
        DataStorageApplicationCryptogramType dataStorageApplicationCryptogramType = new(0);
        _Database.Update(dataStorageApplicationCryptogramType);

        //Act
        StateId stateId = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, stateId);

        DataStorageApplicationCryptogramType savedStoredDsAcType =
            _Database.Get<DataStorageApplicationCryptogramType>(DataStorageApplicationCryptogramType.Tag);
        Assert.Equal(dataStorageApplicationCryptogramType, savedStoredDsAcType);
    }

    [Fact]
    public void PrepareGenerateAcServiceDsOdsTermUsableForAcType_ProcessIdsWrite_CurrentStateIdIsReturned()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        //Set Ids Flags
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId = new(12);
        _Database.Update(dataStorageRequestedOperatorId);
        DataStorageVersionNumberTerminal dataStorageVersionNumberTerminal = new(15);
        _Database.Update(dataStorageVersionNumberTerminal);
        IntegratedDataStorageStatus integratedDataStorageStatus = new(0b1000_0000);
        _Database.Update(integratedDataStorageStatus);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        //CardRiskManagementDataObjectList1RelatedData
        CardRiskManagementDataObjectList1 cardRiskManagementDataObjectList = CardRiskManagementDataObjectList1.Decode(new byte[] {13, 2, 12, 38}.AsSpan());
        _Database.Update(cardRiskManagementDataObjectList);

        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            new(cardRiskManagementDataObjectList.AsDataObjectListResult(_Database));
        _Database.Update(cardRiskManagementDataObjectList1RelatedData);

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(),
            referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);

        _EndpointClient.Setup(m =>
            m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        DataStorageOperatorDataSetInfo dataStorageOperatorDataSetInfo = new(12);
        _Database.Update(dataStorageOperatorDataSetInfo);

        DataStorageDataObjectList dataStorageDataObjectList = new(Array.Empty<TagLength>());
        _Database.Update(dataStorageDataObjectList);

        DataStorageOperatorDataSetInfoForReader dataStorageOperatorDataSetInfoForReader = new(0b100010);
        _Database.Update(dataStorageOperatorDataSetInfoForReader);

        //CryptogramTypes.ApplicationAuthenticationCryptogram
        DataStorageApplicationCryptogramType dataStorageApplicationCryptogramType = new(11);
        _Database.Update(dataStorageApplicationCryptogramType);

        //Act
        StateId stateId = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, stateId);

        DataStorageApplicationCryptogramType savedStoredDsAcType =
            _Database.Get<DataStorageApplicationCryptogramType>(DataStorageApplicationCryptogramType.Tag);
        Assert.Equal(dataStorageApplicationCryptogramType, savedStoredDsAcType);
    }

    [Fact]
    public void PrepareGenerateAcServiceNoDsOdsTermSet_ProcessIdsNoMatchingAc_StopKernelRequestIsSentAndCurrentStateIdIsReturned()
    {
        //Arrange
        StateId currentStateId = WaitingForEmvReadRecordResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        kernel2Session.Update(OdaStatusTypes.Cda);

        Message message = _Fixture.Create<Message>();

        //Set Ids Flags
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId = new(12);
        _Database.Update(dataStorageRequestedOperatorId);
        DataStorageVersionNumberTerminal dataStorageVersionNumberTerminal = new(15);
        _Database.Update(dataStorageVersionNumberTerminal);
        IntegratedDataStorageStatus integratedDataStorageStatus = new(0b1000_0000);
        _Database.Update(integratedDataStorageStatus);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        //CardRiskManagementDataObjectList1RelatedData
        CardRiskManagementDataObjectList1 cardRiskManagementDataObjectList = CardRiskManagementDataObjectList1.Decode(new byte[] {13, 2, 12, 38}.AsSpan());
        _Database.Update(cardRiskManagementDataObjectList);

        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData =
            new(cardRiskManagementDataObjectList.AsDataObjectListResult(_Database));
        _Database.Update(cardRiskManagementDataObjectList1RelatedData);

        StopKernelRequest expectedStopKernelRequest = new(kernel2Session.GetKernelSessionId());

        _EndpointClient.Setup(m => m.Send(It.Is<RequestMessage>(x => x is GenerateApplicationCryptogramRequest || x is StopKernelRequest)));

        DataStorageOperatorDataSetInfo dataStorageOperatorDataSetInfo = new(12);
        _Database.Update(dataStorageOperatorDataSetInfo);

        DataStorageDataObjectList dataStorageDataObjectList = new(Array.Empty<TagLength>());
        _Database.Update(dataStorageDataObjectList);

        //StopIfNoDataStorageOperatorSetTerminalSet
        DataStorageOperatorDataSetInfoForReader dataStorageOperatorDataSetInfoForReader = new(0b100);
        _Database.Update(dataStorageOperatorDataSetInfoForReader);

        //CryptogramTypes.ApplicationAuthenticationCryptogram
        DataStorageApplicationCryptogramType dataStorageApplicationCryptogramType = new(0);
        _Database.Update(dataStorageApplicationCryptogramType);

        //Act
        StateId stateId = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        _EndpointClient.Verify(m => m.Send(It.Is<RequestMessage>(x => x is GenerateApplicationCryptogramRequest || x is StopKernelRequest)), Times.AtLeast(2));
        Assert.Equal(currentStateId, stateId);

        DataStorageApplicationCryptogramType savedStoredDsAcType =
            _Database.Get<DataStorageApplicationCryptogramType>(DataStorageApplicationCryptogramType.Tag);
        Assert.Equal(dataStorageApplicationCryptogramType, savedStoredDsAcType);
    }

    #endregion
}