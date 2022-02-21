﻿using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record ExtendedPaymentCode : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 67</remarks>
    public static readonly DataFieldId DataFieldId = new(67);

    public static readonly PlayEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public ExtendedPaymentCode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ExtendedPaymentCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ExtendedPaymentCode(result.Value);
    }

    #endregion
}