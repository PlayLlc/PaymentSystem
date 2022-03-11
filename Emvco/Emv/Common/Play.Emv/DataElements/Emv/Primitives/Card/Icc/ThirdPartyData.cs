using System;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

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

    #endregion

    #region Serialization

    public static ThirdPartyData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static ThirdPartyData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new DataElementParsingException(
                $"The {nameof(ThirdPartyData)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new ThirdPartyData(result.Value);
    }

    #endregion
}