using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SelectApplicationDefinitionFileInfoResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SelectApplicationDefinitionFileInfoResponse));

    #endregion

    #region Constructor

    public SelectApplicationDefinitionFileInfoResponse(
        CorrelationId correlation,
        TransactionSessionId transactionSessionId,
        GetFileControlInformationRApduSignal responseApduSignal) : base(correlation, MessageTypeId, transactionSessionId,
                                                                        responseApduSignal)
    { }

    #endregion

    #region Instance Members

    public FileControlInformationAdf GetFileControlInformation() => FileControlInformationAdf.Decode(GetData());

    #endregion
}