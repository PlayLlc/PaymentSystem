using Play.Emv.Sessions;
using Play.Icc.Emv;
using Play.Icc.Emv.FileControlInformation;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SelectDirectoryDefinitionFileCommand : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(SelectDirectoryDefinitionFileCommand));

    #endregion

    #region Instance Values

    private readonly DedicatedFileName _DirectoryDefinitionFileName;

    #endregion

    #region Constructor

    private SelectDirectoryDefinitionFileCommand(
        TransactionSessionId transactionSessionId,
        CApduSignal cApduSignal,
        DedicatedFileName directoryDefinitionFileName) : base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _DirectoryDefinitionFileName = directoryDefinitionFileName;
    }

    #endregion

    #region Instance Members

    public static SelectDirectoryDefinitionFileCommand Create(
        TransactionSessionId transactionSessionId,
        DedicatedFileName dedicatedFileName) =>
        new(transactionSessionId, GetFileControlInformationCApduSignal.Get(dedicatedFileName), dedicatedFileName);

    public DedicatedFileName GetDirectoryDefinitionFileName() => _DirectoryDefinitionFileName;

    #endregion
}