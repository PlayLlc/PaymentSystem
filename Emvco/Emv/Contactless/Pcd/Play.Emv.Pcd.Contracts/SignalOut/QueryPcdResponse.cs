using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record QueryPcdResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = ChannelType.ProximityCouplingDevice;
    private static readonly EmvCodec _Codec = EmvCodec.GetBerCodec();

    #endregion

    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;
    private readonly RApduSignal _RApduSignal;

    #endregion

    #region Constructor

    public QueryPcdResponse(
        CorrelationId correlationId,
        MessageTypeId messageTypeId,
        TransactionSessionId transactionSessionId,
        RApduSignal rApduSignal) : base(correlationId, messageTypeId, ChannelTypeId)
    {
        _TransactionSessionId = transactionSessionId;
        _RApduSignal = rApduSignal;
    }

    #endregion

    #region Instance Members

    public bool IsTransactionActive(TransactionSessionId currentTransactionSession) => _TransactionSessionId == currentTransactionSession;
    public bool IsSuccessful() => _RApduSignal.IsSuccessful();
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public TagLengthValue[] AsTagLengthValues() => _Codec.DecodeTagLengthValues(GetData().AsSpan());


    public RApduSignal GetRApduSignal() => _RApduSignal;

    public ErrorIndication GetErrorIndication()
    {
        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();
        builder.Set(_RApduSignal.GetLevel1Error());
        builder.Set(_RApduSignal.GetStatusWords());

        return builder.Complete();
    }

    public Level1Error GetLevel1Error() => _RApduSignal.GetLevel1Error();
    public StatusWords GetStatusWords() => _RApduSignal.GetStatusWords();
    public byte[] GetData() => _RApduSignal.GetData();

    #endregion
}