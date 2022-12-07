using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Command data field of the RECOVER AC command, coded according to DRDOL.
/// </summary>
public record DataRecoveryDataObjectListRelatedData : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8113;

    #endregion

    #region Constructor

    public DataRecoveryDataObjectListRelatedData(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataRecoveryDataObjectListRelatedData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataRecoveryDataObjectListRelatedData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataRecoveryDataObjectListRelatedData Decode(ReadOnlySpan<byte> value)
    {
        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new DataRecoveryDataObjectListRelatedData(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value, length);

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}