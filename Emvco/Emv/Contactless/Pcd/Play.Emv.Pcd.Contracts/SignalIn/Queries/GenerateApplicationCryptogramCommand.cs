using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv;
using Play.Emv.Icc;
using Play.Emv.Icc.GenerateApplicationCryptogram;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GenerateApplicationCryptogramCommand : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GenerateApplicationCryptogramCommand));

    #endregion

    #region Constructor

    private GenerateApplicationCryptogramCommand(TransactionSessionId transactionSessionId, CApduSignal cApduSignal) : base(cApduSignal,
        MessageTypeId, transactionSessionId)
    { }

    #endregion

    #region Instance Members

    public static GenerateApplicationCryptogramCommand Create(
        TransactionSessionId sessionId,
        CryptogramInformationData cryptogramInformationData,
        DataObjectListResult cardRiskManagementDataObjectListResult,
        DataObjectListResult? dataStorageDataObjectListResult = null)
    {
        if (dataStorageDataObjectListResult is null)
        {
            return new GenerateApplicationCryptogramCommand(sessionId,
                GenerateApplicationCryptogramCApduSignal.Create(cryptogramInformationData.GetCryptogramType(),
                    cryptogramInformationData.IsCdaSignatureRequested(), cardRiskManagementDataObjectListResult));
        }

        return new GenerateApplicationCryptogramCommand(sessionId,
            GenerateApplicationCryptogramCApduSignal.Create(cryptogramInformationData.GetCryptogramType(),
                cryptogramInformationData.IsCdaSignatureRequested(), cardRiskManagementDataObjectListResult,
                dataStorageDataObjectListResult));
    }

    #endregion
}