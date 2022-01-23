using System.Linq;

using Play.Emv.Sessions;
using Play.Emv.Templates.Records;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadElementaryFileRecordRangeResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(ReadElementaryFileRecordRangeResponse));

    #endregion

    #region Instance Values

    private readonly ReadElementaryFileRecordResponse[] _Records;

    #endregion

    #region Constructor

    // HACK: We're grabbing the 'FirstOrDefault' from the cardResponses to pass a CAPDU object to the base - This is a bad design. Maybe we should use a different object here, like a batch response object.. We need to handle any responses in the CardClient that don't return a batch or RAPDU records as well
    public ReadElementaryFileRecordRangeResponse(
        CorrelationId correlation,
        TransactionSessionId transactionSessionId,
        ReadElementaryFileRecordResponse[] responses) : base(correlation, MessageTypeId, transactionSessionId,
                                                             responses.FirstOrDefault()!.GetRApduSignal())
    {
        _Records = responses;
    }

    #endregion

    #region Instance Members

    public ReadRecordResponseTemplate[] GetTemplates()
    {
        ReadRecordResponseTemplate[] response = new ReadRecordResponseTemplate[_Records.Length];
        for (int i = 0; i < _Records.Length; i++)
            response[i] = new ReadRecordResponse(_Records[i].GetData());

        return response;
    }

    #endregion
}