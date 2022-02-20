using Play.Emv.Icc;
using Play.Emv.Icc.FileControlInformation;
using Play.Emv.Sessions;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SelectApplicationDefinitionFileInfoCommand : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SelectApplicationDefinitionFileInfoCommand));

    #endregion

    #region Instance Values

    private readonly DedicatedFileName _DedicatedFileName;

    #endregion

    #region Constructor

    public SelectApplicationDefinitionFileInfoCommand(
        TransactionSessionId transactionSessionId,
        CApduSignal cApduSignal,
        DedicatedFileName dedicatedFileName) : base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _DedicatedFileName = dedicatedFileName;
    }

    #endregion

    #region Instance Members

    public static SelectApplicationDefinitionFileInfoCommand Create(
        TransactionSessionId transactionSessionId,
        DedicatedFileName dedicatedFileName) =>
        new(transactionSessionId, GetFileControlInformationCApduSignal.Get(dedicatedFileName), dedicatedFileName);

    public DedicatedFileName GetDedicatedFileName() => _DedicatedFileName;

    #endregion
}