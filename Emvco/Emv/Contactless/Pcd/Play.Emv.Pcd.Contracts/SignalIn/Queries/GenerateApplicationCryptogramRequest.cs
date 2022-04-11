using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
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

    public static GenerateApplicationCryptogramRequest Create(
        TransactionSessionId sessionId, ReferenceControlParameter referenceControlParameter,
        CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectList1RelatedData,
        DataObjectListResult? dataStorageDataObjectListResult = null)
    {
        if (dataStorageDataObjectListResult is null)
        {
            return new GenerateApplicationCryptogramRequest(sessionId,
                GenerateApplicationCryptogramCApduSignal.Create(referenceControlParameter, cardRiskManagementDataObjectList1RelatedData));
        }

        return new GenerateApplicationCryptogramRequest(sessionId,
            GenerateApplicationCryptogramCApduSignal.Create(referenceControlParameter, cardRiskManagementDataObjectListResult,
                dataStorageDataObjectListResult));
    }

    public bool IsCdaRequested() => ((GenerateApplicationCryptogramCApduSignal) GetCApduSignal()).IsCdaRequested();

    /// <exception cref="TerminalDataException"></exception>
    public CryptogramTypes GetCryptogramType() => ((GenerateApplicationCryptogramCApduSignal) GetCApduSignal()).GetCryptogramTypes();

    #endregion
}