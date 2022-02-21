﻿using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record LocalTransactionTimeHhmmss : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 12</remarks>
    public static readonly DataFieldId DataFieldId = new(12);

    public static readonly PlayEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 3;

    #endregion

    #region Constructor

    public LocalTransactionTimeHhmmss(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override LocalTransactionTimeHhmmss Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new LocalTransactionTimeHhmmss(result.Value);
    }

    #endregion
}