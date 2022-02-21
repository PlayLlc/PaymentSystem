﻿using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record RetrievalReferenceNumber : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 37</remarks>
    public static readonly DataFieldId DataFieldId = new(37);

    public static readonly PlayEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 12;

    #endregion

    #region Constructor

    public RetrievalReferenceNumber(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override RetrievalReferenceNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new RetrievalReferenceNumber(result.Value);
    }

    #endregion
}