using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Icc;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the type of cryptogram and the actions to be performed by the terminal
/// </summary>
public record CryptogramInformationData : DataElement<byte>, IEqualityComparer<CryptogramInformationData>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F27;

    #endregion

    #region Constructor

    public CryptogramInformationData(byte value) : base(value)
    { }

    public CryptogramInformationData(CryptogramTypes cryptogramTypes, bool isCombinedDataAuthenticationSupported) : base(
        Create(cryptogramTypes, isCombinedDataAuthenticationSupported))
    { }

    #endregion

    #region Instance Members

    private static byte Create(CryptogramTypes cryptogramTypes, bool isCombinedDataAuthenticationSupported)
    {
        if (isCombinedDataAuthenticationSupported)
            return (byte) (cryptogramTypes | (byte) Bits.Five);

        return (byte) cryptogramTypes;
    }

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;

    /// <summary>
    ///     GetCryptogramType
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public CryptogramTypes GetCryptogramType()
    {
        if (!CryptogramTypes.TryGet(_Value, out CryptogramTypes? result))
        {
            throw new InvalidOperationException(
                $"The {nameof(CryptogramInformationData)} expected a {nameof(CryptogramTypes)} but none could be found");
        }

        return result!;
    }

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    /// <summary>
    ///     Value signifying whether a Combined Data Authentication Signature has been requested by the ICC
    /// </summary>
    /// <returns></returns>
    public bool IsCdaSignatureRequested() => _Value.IsBitSet(Bits.Five);

    #endregion

    #region Serialization

    public static CryptogramInformationData Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CryptogramInformationData Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 1;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(CryptogramInformationData)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(CryptogramInformationData)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new CryptogramInformationData(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(PlayEncodingId, _Value, 1);

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
}