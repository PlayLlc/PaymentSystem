using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the type of cryptogram and the actions to be performed by the terminal
/// </summary>
public record CryptogramInformationData : DataElement<byte>, IEqualityComparer<CryptogramInformationData>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F27;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public CryptogramInformationData(byte value) : base(value)
    { }

    public CryptogramInformationData(CryptogramTypes cryptogramTypes, bool isCombinedDataAuthenticationSupported) : base(Create(cryptogramTypes,
        isCombinedDataAuthenticationSupported))
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static CryptogramInformationData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override CryptogramInformationData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static CryptogramInformationData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new CryptogramInformationData(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(CryptogramInformationData? x, CryptogramInformationData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CryptogramInformationData obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    private static byte Create(CryptogramTypes cryptogramTypes, bool isCombinedDataAuthenticationSupported)
    {
        if (isCombinedDataAuthenticationSupported)
            return (byte) (cryptogramTypes | (byte) Bits.Five);

        return (byte) cryptogramTypes;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// <summary>
    ///     GetCryptogramType
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public CryptogramTypes GetCryptogramType()
    {
        if (!CryptogramTypes.TryGet(_Value, out CryptogramTypes? result))
            throw new DataElementParsingException($"The {nameof(CryptogramInformationData)} expected a {nameof(CryptogramTypes)} but none could be found");

        return result!;
    }

    /// <remarks>EMV Book C-2 Section S9.23</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public bool IsValid(IReadTlvDatabase database)
    {
        //This will always be true. Desired behaviour ?
        if (_Value.AreBitsSet(CryptogramTypes.ApplicationAuthenticationCryptogram))
            return true;

        ReferenceControlParameter referenceControlParameter = database.Get<ReferenceControlParameter>(ReferenceControlParameter.Tag);

        if (_Value.AreBitsSet(CryptogramTypes.TransactionCryptogram))
            return referenceControlParameter.GetCryptogramType() == CryptogramTypes.TransactionCryptogram;

        if (_Value.AreBitsSet(CryptogramTypes.AuthorizationRequestCryptogram))
        {
            return (referenceControlParameter.GetCryptogramType() == CryptogramTypes.TransactionCryptogram)
                || (referenceControlParameter.GetCryptogramType() == CryptogramTypes.AuthorizationRequestCryptogram);
        }

        return false;
    }

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    /// <summary>
    ///     Value signifying whether a Combined Data Authentication Signature has been requested by the ICC
    /// </summary>
    /// <returns></returns>
    public bool IsCdaSignatureRequested() => _Value.IsBitSet(Bits.Five);

    #endregion
}