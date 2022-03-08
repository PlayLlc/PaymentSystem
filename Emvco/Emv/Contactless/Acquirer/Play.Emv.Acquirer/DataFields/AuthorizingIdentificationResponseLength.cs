using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record AuthorizingIdentificationResponseLength : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 27</remarks>
    public static readonly DataFieldId DataFieldId = new(27);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public AuthorizingIdentificationResponseLength(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    /// <exception cref="InterchangeException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InterchangeDataFieldNullException"></exception>
    public override AuthorizingIdentificationResponseLength Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AuthorizingIdentificationResponseLength(result.Value);
    }

    #endregion
}