using System.Numerics;

using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record MessageSecurityCode : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 96</remarks>
    public static readonly DataFieldId DataFieldId = new(96);

    public static readonly PlayEncodingId EncodingId = BinaryDataFieldCodec.Identifier;
    private const ushort _ByteCount = 64;

    #endregion

    #region Constructor

    public MessageSecurityCode(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override MessageSecurityCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new MessageSecurityCode(result.Value);
    }

    #endregion
}