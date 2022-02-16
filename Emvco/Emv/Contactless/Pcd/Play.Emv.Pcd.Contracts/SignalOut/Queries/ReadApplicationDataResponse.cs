using System.Linq;

using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadApplicationDataResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(ReadApplicationDataResponse));

    #endregion

    #region Instance Values

    private readonly ReadElementaryFileRecordRangeResponse[] _Records;

    #endregion

    #region Constructor

    // HACK: We're grabbing the 'FirstOrDefault' from the cardResponses to pass a CAPDU object to the base - This is a bad design. Maybe we should use a different object here, like a batch response object.. We need to handle any responses in the CardClient that don't return a batch or RAPDU records as well
    public ReadApplicationDataResponse(
        CorrelationId correlation,
        TransactionSessionId transactionSessionId,
        ReadElementaryFileRecordRangeResponse[] cardResponses) : base(correlation, MessageTypeId, transactionSessionId,
        cardResponses.FirstOrDefault()!.GetRApduSignal())
    {
        _Records = cardResponses;
    }

    #endregion
}