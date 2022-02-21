using System.Numerics;

using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record OriginalDataElements : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 90</remarks>
    public static readonly DataFieldId DataFieldId = new(90);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 21;

    #endregion

    #region Constructor

    public OriginalDataElements(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override OriginalDataElements Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new OriginalDataElements(result.Value);
    }

    #endregion
}