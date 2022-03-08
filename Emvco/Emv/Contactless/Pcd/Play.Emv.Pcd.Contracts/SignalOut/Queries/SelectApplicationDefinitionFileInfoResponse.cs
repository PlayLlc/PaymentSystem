using Play.Emv.Icc;
using Play.Emv.Sessions;
using Play.Emv.Templates.FileControlInformation.ApplicationDefinitionFile;
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
        RApduSignal responseApduSignal) : base(correlation, MessageTypeId, transactionSessionId, responseApduSignal)
    { }

    #endregion

    #region Instance Members

    public FileControlInformationAdf GetFileControlInformation() => FileControlInformationAdf.Decode(GetData());

    #endregion
}