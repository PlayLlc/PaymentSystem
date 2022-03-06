using Play.Emv.DataElements.Emv;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GetDataBatchRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetDataBatchRequest));

    #endregion

    #region Instance Values

    private readonly TagsToRead _TagsToRead;

    #endregion

    #region Constructor

    protected GetDataBatchRequest(TransactionSessionId transactionSessionId, TagsToRead tagsToRead) : base(null, MessageTypeId,
        transactionSessionId)
    {
        _TagsToRead = tagsToRead;
    }

    #endregion

    #region Instance Members

    public TagsToRead GetTagsToRead() => _TagsToRead;

    public static GetDataBatchRequest Create(TransactionSessionId transactionSessionId, TagsToRead tagsToRead) =>
        new(transactionSessionId, tagsToRead);

    #endregion
}