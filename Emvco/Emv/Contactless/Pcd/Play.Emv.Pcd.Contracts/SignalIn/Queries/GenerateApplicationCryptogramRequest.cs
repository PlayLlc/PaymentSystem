using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv.Primitives.Security;
using Play.Emv.Icc;
using Play.Emv.Icc.GenerateApplicationCryptogram;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GenerateApplicationCryptogramRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GenerateApplicationCryptogramRequest));

    #endregion

    #region Constructor

    private GenerateApplicationCryptogramRequest(TransactionSessionId transactionSessionId, CApduSignal cApduSignal) : base(cApduSignal,
        MessageTypeId, transactionSessionId)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Create
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cryptogramInformationData"></param>
    /// <param name="cardRiskManagementDataObjectListResult"></param>
    /// <param name="dataStorageDataObjectListResult"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public static GenerateApplicationCryptogramRequest Create(
        TransactionSessionId sessionId,
        CryptogramInformationData cryptogramInformationData,
        DataObjectListResult cardRiskManagementDataObjectListResult,
        DataObjectListResult? dataStorageDataObjectListResult = null)
    {
        if (dataStorageDataObjectListResult is null)
        {
            return new GenerateApplicationCryptogramRequest(sessionId,
                GenerateApplicationCryptogramCApduSignal.Create(cryptogramInformationData.GetCryptogramType(),
                    cryptogramInformationData.IsCdaSignatureRequested(), cardRiskManagementDataObjectListResult));
        }

        return new GenerateApplicationCryptogramRequest(sessionId,
            GenerateApplicationCryptogramCApduSignal.Create(cryptogramInformationData.GetCryptogramType(),
                cryptogramInformationData.IsCdaSignatureRequested(), cardRiskManagementDataObjectListResult,
                dataStorageDataObjectListResult));
    }

    #endregion
}