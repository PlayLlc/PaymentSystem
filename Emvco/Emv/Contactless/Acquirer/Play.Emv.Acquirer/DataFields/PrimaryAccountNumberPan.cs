using System.Numerics;

using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record PrimaryAccountNumberPan : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 2</remarks>
    public static readonly DataFieldId DataFieldId = new(2);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _MaxByteCount = 10;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public PrimaryAccountNumberPan(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    /// <summary>
    /// Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    /// <exception cref="InterchangeException"></exception>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    /// <exception cref="InterchangeDataFieldNullException"></exception>
    public override PrimaryAccountNumberPan Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PrimaryAccountNumberPan(result.Value);
    }

    #endregion
}