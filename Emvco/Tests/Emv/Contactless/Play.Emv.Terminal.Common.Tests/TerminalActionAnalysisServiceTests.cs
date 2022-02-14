using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.AutoMoq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

using Moq;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Icc.GenerateApplicationCryptogram;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Messaging;

using Xunit;

namespace Play.Emv.Terminal.Common.Tests;
public class GenerateApplicationCryptogramCommandTestSpy
{
    private TransactionSessionId? _TransactionSessionId;
    private CryptogramInformationData? _CryptogramInformationData;
    private DataObjectListResult? _CardRiskDol;
    private DataObjectListResult? _DataStorageDol;


    public GenerateApplicationCryptogramCommandTestSpy()
    {
        _TransactionSessionId = null;
        _CryptogramInformationData = null;
        _CardRiskDol = null;
        _DataStorageDol = null;
    }

    public void UpdateMessageSent(TransactionSessionId sessionId,
        CryptogramInformationData cryptogramInformationData,
        DataObjectListResult cardRiskManagementDataObjectListResult,
        DataObjectListResult? dataStorageDataObjectListResult = null)
    {
        _TransactionSessionId = sessionId;
        _CryptogramInformationData = cryptogramInformationData;
        _CardRiskDol = cardRiskManagementDataObjectListResult;
        _DataStorageDol = dataStorageDataObjectListResult;
    }

    public TransactionSessionId? GetTransactionSessionId() => _TransactionSessionId;
    public CryptogramInformationData? GetCryptogramInformationData() => _CryptogramInformationData;
    public DataObjectListResult? GetCardRiskDol() => _CardRiskDol;
    public DataObjectListResult? GetDataStorageDol() => _DataStorageDol;

    public void Clear()
    {
        _TransactionSessionId = null;
        _CryptogramInformationData = null;
        _CardRiskDol = null;
        _DataStorageDol = null;
    }

    public static GenerateApplicationCryptogramCommandTestSpy Setup(IFixture fixture)
    {
        GenerateApplicationCryptogramCommandTestSpy result = new(); 

        Mock<GenerateApplicationCryptogramCommand> mock = new();

        mock.Setup(a => GenerateApplicationCryptogramCommand.Create(It.IsAny<TransactionSessionId>(), It.IsAny<CryptogramInformationData>(),
                It.IsAny<DataObjectListResult>(),
                It.IsAny<DataObjectListResult?>()))
            .Callback((TransactionSessionId transactionSessionId,
                CryptogramInformationData cryptogramInformationData,
                DataObjectListResult cardRiskDol,
                DataObjectListResult? dataStorageDol) => result.UpdateMessageSent(transactionSessionId,
                cryptogramInformationData,
                cardRiskDol,
                dataStorageDol));

        fixture.Register(() => mock.Object);
        fixture.Freeze<GenerateApplicationCryptogramCommand>();
        return result;
    }
}
[Trait("Type", "Unit")]
public class TerminalActionAnalysisServiceTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly TerminalActionAnalysisService _TerminalActionAnalysisService;
    private readonly GenerateApplicationCryptogramCommandTestSpy _GenerateApplicationCryptogramCommandTestSpy;
      

    #endregion

    #region Constructor

    public TerminalActionAnalysisServiceTests()
    {
        _Fixture = new Fixture();
        _Fixture.Customize(new AutoMoqCustomization());
        _Fixture.Register(() => new Mock<TerminalActionAnalysisService>().Object);
        _GenerateApplicationCryptogramCommandTestSpy = GenerateApplicationCryptogramCommandTestSpy.Setup(_Fixture);
        _TerminalActionAnalysisService = _Fixture.Freeze<TerminalActionAnalysisService>(); 
    }

   

    #endregion

    #region Instance Members

    [Fact]
    public void Test()
    {
        var a = new TerminalActionAnalysisCommand(_Fixture.Create<TransactionSessionId>(),
            _Fixture.Create<CryptogramInformationData>(),
            _Fixture.Create<DataObjectListResult>(),
            _Fixture.Create<DataObjectListResult>());


        _TerminalActionAnalysisService.Process();


        _PcdEndpoint.Request(_Fixture.Create<ActivatePcdRequest>());
        _PcdEndpoint.
    }

    #endregion
}