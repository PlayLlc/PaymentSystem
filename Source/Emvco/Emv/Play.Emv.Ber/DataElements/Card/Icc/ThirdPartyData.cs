using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Country;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The Third Party Data contains various information, possibly including information from a third party. If present in
///     the Card, the Third Party Data must be returned in a file read using the READ RECORD command or in the File Control
///     Information Template. 'Device Type' is present when the most significant bit of byte 1 of 'Unique Identifier' is
///     set to 0b. In this case, the maximum length of 'Proprietary Data' is 26 bytes. Otherwise it is 28 bytes.
/// </summary>
public record ThirdPartyData : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F6E;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MinByteLength = 5;
    private const byte _MaxByteLength = 32;

    #endregion

    #region Constructor

    public ThirdPartyData(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public Alpha2CountryCode GetCountryCode() => new(PlayCodec.AlphabeticCodec.DecodeToChars(_Value.ToByteArray(true)[..2].AsSpan()));
    public ushort GetUniqueIdentifier() => PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(_Value.ToByteArray(true)[2..4].AsSpan());
    private bool IsDeviceTypePresent() => GetUniqueIdentifier().IsBitSet(16);

    public bool TryGetDeviceType(out ushort? result)
    {
        if (!IsDeviceTypePresent())
        {
            result = null;

            return false;
        }

        result = PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(_Value.ToByteArray(true)[4..6]);

        return true;
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ThirdPartyData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ThirdPartyData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ThirdPartyData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new ThirdPartyData(result);
    }

    #endregion
}