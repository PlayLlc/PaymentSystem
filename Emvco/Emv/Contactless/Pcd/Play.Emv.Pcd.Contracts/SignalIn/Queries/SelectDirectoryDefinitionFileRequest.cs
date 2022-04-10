using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SelectDirectoryDefinitionFileRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SelectDirectoryDefinitionFileRequest));

    #endregion

    #region Instance Values

    private readonly DedicatedFileName _DirectoryDefinitionFileName;

    #endregion

    #region Constructor

    private SelectDirectoryDefinitionFileRequest(
        TransactionSessionId transactionSessionId, CApduSignal cApduSignal, DedicatedFileName directoryDefinitionFileName) :
        base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _DirectoryDefinitionFileName = directoryDefinitionFileName;
    }

    #endregion

    #region Instance Members

    public static SelectDirectoryDefinitionFileRequest Create(
        TransactionSessionId transactionSessionId, DedicatedFileName dedicatedFileName) =>
        new(transactionSessionId, GetFileControlInformationCApduSignal.Get(dedicatedFileName), dedicatedFileName);

    public DedicatedFileName GetDirectoryDefinitionFileName() => _DirectoryDefinitionFileName;

    #endregion
}