using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Kernel2.Databases;
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

    public PrimitiveValue[] GetPrimitiveValues(IResolveKnownObjectsAtRuntime runtimeCodec) =>
        runtimeCodec.DecodePrimitiveSiblingsAtRuntime(GetData().AsMemory()).ToArray();

    public ShortFileId GetShortFileId() => _ShortFileId;

    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    public PrimitiveValue[] GetRecords() => ReadRecordResponseTemplate.GetRecords(GetData());

    public int GetValueByteCount() => GetData().Length;

    #endregion
}