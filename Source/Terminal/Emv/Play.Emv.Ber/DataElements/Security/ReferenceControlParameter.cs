using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Working variable to store the reference control parameter of the GENERATE AC command.
/// </summary>
public record ReferenceControlParameter : DataElement<byte>, IEqualityComparer<ReferenceControlParameter>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8114;
    private const byte _ByteLength = 1;
    private const Bits _CdaRequestedFlag = Bits.Five;

    #endregion

    #region Constructor

    /// <exception cref="CardDataException"></exception>
    public ReferenceControlParameter(byte value) : base(value)
    {
        if (!CryptogramTypes.IsValid(value))
            throw new CardDataException($"The argument {nameof(value)} was not recognized as a valid {nameof(CryptogramTypes)}");
    }

    /// <exception cref="CardDataException"></exception>
    public ReferenceControlParameter(CryptogramType cryptogramType, bool isCdaSignatureRequested) : base(Create(cryptogramType, isCdaSignatureRequested))
    {
        if (!CryptogramTypes.IsValid((byte) cryptogramType))
            throw new CardDataException($"The argument {nameof(cryptogramType)} was not recognized as a valid {nameof(CryptogramTypes)}");
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataException"></exception>
    public static ReferenceControlParameter Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataException"></exception>
    public override ReferenceControlParameter Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataException"></exception>
    public static ReferenceControlParameter Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new ReferenceControlParameter(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(ReferenceControlParameter? x, ReferenceControlParameter? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ReferenceControlParameter obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(ReferenceControlParameter value) => value._Value;

    #endregion

    #region Instance Members

    private static byte Create(CryptogramType cryptogramTypes, bool isCdaSignatureRequested)
    {
        if (isCdaSignatureRequested)
            return (byte) ((byte) cryptogramTypes | (byte)_CdaRequestedFlag);

        return (byte) cryptogramTypes;
    }

    public bool IsCdaSignatureRequested() => _Value.IsBitSet(_CdaRequestedFlag);

    /// <exception cref="DataElementParsingException"></exception>
    public CryptogramTypes GetCryptogramType()
    {
        if (!CryptogramTypes.TryGet(_Value, out CryptogramTypes? result))
            throw new DataElementParsingException($"The {nameof(CryptogramInformationData)} expected a {nameof(CryptogramTypes)} but none could be found");

        return result!;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}