using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Templates;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SelectDirectoryDefinitionFileResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SelectDirectoryDefinitionFileResponse));

    #endregion

    #region Instance Values

    private readonly FileControlInformationDdf? _FileControlInformation;

    #endregion

    #region Constructor

    public SelectDirectoryDefinitionFileResponse(
        CorrelationId correlation,
        TransactionSessionId transactionSessionId,
        FileControlInformationDdf fileControlInformation,
        GetFileControlInformationRApduSignal responseApduSignal) : base(correlation, MessageTypeId, transactionSessionId,
                                                                        responseApduSignal)
    {
        _FileControlInformation = fileControlInformation;
    }

    public SelectDirectoryDefinitionFileResponse(
        CorrelationId correlation,
        TransactionSessionId transactionSessionId,
        GetFileControlInformationRApduSignal responseApduSignal) : base(correlation, MessageTypeId, transactionSessionId,
                                                                        responseApduSignal)
    { }

    #endregion

    #region Instance Members

    public bool TryGetFileControlInformation(out FileControlInformationDdf? result)
    {
        if (_FileControlInformation is null)
        {
            result = null;

            return false;
        }

        result = _FileControlInformation!;

        return true;
    }

    #endregion
}