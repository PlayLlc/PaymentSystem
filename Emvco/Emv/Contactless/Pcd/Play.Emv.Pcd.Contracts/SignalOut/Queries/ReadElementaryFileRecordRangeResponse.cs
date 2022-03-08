using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Sessions;
using Play.Emv.Templates;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadElementaryFileRecordRangeResponse : QueryPcdResponse
{
    #region Static Metadata

    private static readonly EmvCodec _Codec = EmvCodec.GetBerCodec();
    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadElementaryFileRecordRangeResponse));

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

    public TagLengthValue GetTagLengthValue()
    {
        Span<byte> buffer = stackalloc byte[_Records.Sum(a => a.GetValueByteCount())];

        for (int i = 0, j = 0; i < _Records.Length; i++)
        {
            _Records[i].GetRawRecord().CopyTo(buffer);
            j += _Records[i].GetValueByteCount();
        }

        return _Codec.DecodeTagLengthValue(buffer);
    }

    #endregion
}