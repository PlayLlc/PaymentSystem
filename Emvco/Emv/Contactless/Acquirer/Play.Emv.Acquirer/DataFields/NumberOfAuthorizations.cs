﻿using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Acquirer.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;

namespace Play.Emv.Acquirer.DataFields;

public record NumberOfAuthorizations : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 81</remarks>
    public static readonly DataFieldId DataFieldId = new(81);

    public static readonly PlayEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public NumberOfAuthorizations(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override NumberOfAuthorizations Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NumberOfAuthorizations(result.Value);
    }

    #endregion
}