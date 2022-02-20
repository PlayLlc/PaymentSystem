using Play.Emv.DataElements;
using Play.Emv.Sessions;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadApplicationDataCommand : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadApplicationDataCommand));

    #endregion

    #region Instance Values

    private readonly ApplicationFileLocator _ApplicationFileLocator;

    #endregion

    #region Constructor

    // HACK: Do not inject a NULL value into this base class. This request might actually end up being multiple multiple record requests. We need to find a better pattern to allow that
    public ReadApplicationDataCommand(ApplicationFileLocator applicationFileLocator, TransactionSessionId transactionSessionId) : base(null,
        MessageTypeId, transactionSessionId)
    {
        _ApplicationFileLocator = applicationFileLocator;
    }

    #endregion

    #region Instance Members

    public ApplicationFileLocator GetApplicationFileLocator() => _ApplicationFileLocator;
    public RecordRange[] GetRecordRanges() => _ApplicationFileLocator.GetRecordRanges();

    public static ReadApplicationDataCommand Create(
        ApplicationFileLocator applicationFileLocator,
        TransactionSessionId transactionSessionId) =>
        new(applicationFileLocator, transactionSessionId);

    #endregion
}