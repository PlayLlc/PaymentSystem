using System;
using System.Linq;

using AutoFixture;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

using Moq;

using Play.Core.Extensions.IEnumerable;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
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
        StateId currentStateId = new StateId(new char[] { 's', 't', 'a', 't', 'e', '1', });
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

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(), referenceControlParameter, crmdrol);
        _EndpointClient.Setup(m => m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

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

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(), referenceControlParameter, crmdrol);
        _EndpointClient.Setup(m => m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

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

        CardRiskManagementDataObjectList1 cardRiskManagementDataObjectList = CardRiskManagementDataObjectList1.Decode(new byte[] { 13, 2, 12, 38 }.AsSpan());
        _Database.Update(cardRiskManagementDataObjectList);

        ReferenceControlParameterTestTlv referenceControlParameterTestTlv = new();
        ReferenceControlParameter referenceControlParameter = ReferenceControlParameter.Decode(referenceControlParameterTestTlv.EncodeValue().AsSpan());
        _Database.Update(referenceControlParameter);

        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData = new(cardRiskManagementDataObjectList.AsDataObjectListResult(_Database));

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(), referenceControlParameter, cardRiskManagementDataObjectList1RelatedData);
        _EndpointClient.Setup(m => m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    //GAC25
    [Fact]
    public void PrepareGenerateAcServiceNoIdsWithCDASupportedOverTC_ProcessIDSReadOnly_SendGenerateAcCommandAndReturnsCurrentStateId()
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

        GenerateApplicationCryptogramRequest expectedGenerateACRequest = GenerateApplicationCryptogramRequest.Create(kernel2Session.GetTransactionSessionId(), referenceControlParameter, crmdrol);
        _EndpointClient.Setup(m => m.Send(It.Is<GenerateApplicationCryptogramRequest>(x => x.GetTransactionSessionId() == expectedGenerateACRequest.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    #endregion
}
