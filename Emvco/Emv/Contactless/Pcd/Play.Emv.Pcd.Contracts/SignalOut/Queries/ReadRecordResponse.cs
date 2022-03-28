using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadRecordResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadRecordResponse));

    #endregion

    #region Instance Values

    private readonly ShortFileId _ShortFileId;

    #endregion

    #region Constructor

    public ReadRecordResponse(CorrelationId correlationId, TransactionSessionId transactionSessionId, ReadRecordRApduSignal rApdu) :
        base(correlationId, MessageTypeId, transactionSessionId, rApdu)
    { }

    #endregion

    #region Instance Members

    public ShortFileId GetShortFileId() => _ShortFileId;

    /// <exception cref="BerParsingException"></exception>
    public PrimitiveValue[] GetPrimitiveDataObjects() => ReadRecordResponseTemplate.GetPrimitiveValuesFromRecords(GetData());

    public int GetValueByteCount() => GetData().Length;

    #endregion
}