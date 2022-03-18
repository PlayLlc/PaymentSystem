using AutoFixture;

using Moq;

using Play.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Terminal.Common.Tests.TerminalActionAnalysisServiceTests;

public class GenerateApplicationCryptogramCommandTestSpy
{
    #region Instance Values

    private TransactionSessionId? _TransactionSessionId;
    private CryptogramInformationData? _CryptogramInformationData;
    private DataObjectListResult? _CardRiskDol;
    private DataObjectListResult? _DataStorageDol;

    #endregion

    #region Constructor

    public GenerateApplicationCryptogramCommandTestSpy()
    {
        _TransactionSessionId = null;
        _CryptogramInformationData = null;
        _CardRiskDol = null;
        _DataStorageDol = null;
    }

    #endregion

    #region Instance Members

    public void UpdateMessageSent(
        TransactionSessionId sessionId,
        CryptogramInformationData cryptogramInformationData,
        DataObjectListResult cardRiskManagementDataObjectListResult,
        DataObjectListResult? dataStorageDataObjectListResult = null)
    {
        Clear();

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

    /// <summary>
    ///     Setup
    /// </summary>
    /// <param name="fixture"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static GenerateApplicationCryptogramCommandTestSpy Setup(IFixture fixture)
    {
        GenerateApplicationCryptogramCommandTestSpy result = new();

        Mock<GenerateApplicationCryptogramRequest> mock = new();

        mock.Setup(a => GenerateApplicationCryptogramRequest.Create(It.IsAny<TransactionSessionId>(), It.IsAny<CryptogramInformationData>(),
                                                                    It.IsAny<DataObjectListResult>(), It.IsAny<DataObjectListResult?>()))
            .Callback((
                          TransactionSessionId transactionSessionId,
                          CryptogramInformationData cryptogramInformationData,
                          DataObjectListResult cardRiskDol,
                          DataObjectListResult? dataStorageDol) => result.UpdateMessageSent(transactionSessionId,
                                                                                            cryptogramInformationData, cardRiskDol,
                                                                                            dataStorageDol));

        fixture.Register(() => mock.Object);
        fixture.Freeze<GenerateApplicationCryptogramRequest>();

        return result;
    }

    #endregion
}