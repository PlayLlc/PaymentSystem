using System.Linq;

using AutoFixture;

using Moq;

using Play.Core.Extensions.IEnumerable;
using Play.Emv.Ber.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;
using Play.Testing.Emv.Ber.Primitive;
using Play.Testing.Emv.Contactless.AutoFixture;
using System;

using Xunit;

namespace Play.Emv.Kernel2.Tests.Services.BalanceReading;

public class OfflineBalanceReaderTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly KernelDatabase _Database;

    private readonly Mock<DataExchangeKernelService> _DataExchangeKernelService;
    private readonly Mock<IGetKernelState> _KernelStateResolver;
    private readonly Mock<IEndpointClient> _EndpointClient;
    private readonly Mock<IGetKernelStateId> _KernelStateIdRetriever;

    private readonly OfflineBalanceReader _SystemUnderTest;

    #endregion

    #region Constructor

    public OfflineBalanceReaderTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);

        _Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _Fixture.Behaviors.Remove(b));
        _Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _DataExchangeKernelService = new Mock<DataExchangeKernelService>(MockBehavior.Strict);
        _KernelStateResolver = new Mock<IGetKernelState>(MockBehavior.Strict);
        _EndpointClient = new Mock<IEndpointClient>(MockBehavior.Strict);
        _KernelStateIdRetriever = new Mock<IGetKernelStateId>(MockBehavior.Strict);

        _SystemUnderTest = new OfflineBalanceReader(_Database, _DataExchangeKernelService.Object, _KernelStateResolver.Object, _EndpointClient.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void OfflineBalanceReader_ProcessWithOutofSyncInvalidStateId_ThrowsRequestOutOfSyncException()
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
    public void OfflineBalanceReader_ProcessPreGenAcBalanceReaderMissingApplicationCapabilitiesInformationFromKernelDb_ReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForGetDataResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    [Fact]
    public void OfflineBalanceReader_ProcessPreGenAcBalanceReaderApplicationCapabilitiesInformationDoesNotSupportBalanceRead_ReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForGetDataResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new ApplicationCapabilitiesInformation(0b1000_0000);
        _Database.Update(applicationCapabilitiesInformation);

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    [Fact]
    public void OfflineBalanceReader_ProcessPreGenAcBalanceReaderButRespectiveTagNoPresentInDb_ReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForGetDataResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new ApplicationCapabilitiesInformation(0b10_0000_0000);
        _Database.Update(applicationCapabilitiesInformation);

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    [Fact]
    public void OfflineBalanceReader_ProcessPostGenAcBalanceReadApplicationCapabilitiesInformationTagMissingFromDb_ReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForGenerateAcResponse1.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    [Fact]
    public void OfflineBalanceReader_ProcessPostGenAcBalanceReadApplicationCapabilitiesInformationDoesNotSupportBalanceRead_ReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForGenerateAcResponse1.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new ApplicationCapabilitiesInformation(0b1000_0000);
        _Database.Update(applicationCapabilitiesInformation);

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    [Fact]
    public void OfflineBalanceReader_ProcessPostGenAcBalanceReadBalanceReadAfterGenAcMissingFromDb_ReturnsCurrentStateId()
    {
        //Arrange
        StateId currentStateId = WaitingForGenerateAcResponse1.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(currentStateId, actual);
    }

    [Fact]
    public void OfflineBalanceReader_ProcessPostGenAcBalanceRead_GetRequestDataIsSentAndWaitingForPostGenAcBalanceStateIdIsReturned()
    {
        //Arrange
        StateId currentStateId = WaitingForGenerateAcResponse1.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        // app capabilities that supports balance read.
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new ApplicationCapabilitiesInformation(0b10_0000_0000);
        _Database.Update(applicationCapabilitiesInformation);

        //setup endpointclient.

        BalanceReadAfterGenAcTestTlv testData = new();
        BalanceReadAfterGenAc balanceReadAfterGenAc = BalanceReadAfterGenAc.Decode(testData.EncodeValue().AsSpan());
        _Database.Update(balanceReadAfterGenAc);

        GetDataRequest capdu = GetDataRequest.Create(OfflineAccumulatorBalance.Tag, kernel2Session.GetTransactionSessionId());
        _EndpointClient.Setup(m => m.Send(It.Is<GetDataRequest>(x => x.GetTransactionSessionId() == capdu.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(WaitingForPostGenAcBalance.StateId, actual);
    }

    [Fact]
    public void OfflineBalanceReader_ProcessPreGenAcBalanceRead_GetRequestDataIsSentAndWaitingForPreGenAcBalanceStateIdIsReturned()
    {
        //Arrange
        StateId currentStateId = WaitingForGetDataResponse.StateId;
        _KernelStateIdRetriever.Setup(m => m.GetStateId()).Returns(currentStateId);

        Kernel2Session kernel2Session = _Fixture.Create<Kernel2Session>();
        Message message = _Fixture.Create<Message>();

        // app capabilities that supports balance read.
        ApplicationCapabilitiesInformation applicationCapabilitiesInformation = new ApplicationCapabilitiesInformation(0b10_0000_0000);
        _Database.Update(applicationCapabilitiesInformation);

        BalanceReadBeforeGenAcTestTlv testData = new();
        BalanceReadBeforeGenAc balanceReadBeforeGenAc = BalanceReadBeforeGenAc.Decode(testData.EncodeValue().AsSpan());
        _Database.Update(balanceReadBeforeGenAc);

        //setup endpointclient.
        GetDataRequest capdu = GetDataRequest.Create(OfflineAccumulatorBalance.Tag, kernel2Session.GetTransactionSessionId());
        _EndpointClient.Setup(m => m.Send(It.Is<GetDataRequest>(x => x.GetTransactionSessionId() == capdu.GetTransactionSessionId())));

        //Act
        StateId actual = _SystemUnderTest.Process(_KernelStateIdRetriever.Object, kernel2Session, message);

        //Assert
        Assert.Equal(WaitingForPreGenAcBalance.StateId, actual);
    }

    #endregion
}
