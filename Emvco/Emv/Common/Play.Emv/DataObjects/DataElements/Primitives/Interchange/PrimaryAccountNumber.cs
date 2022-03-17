using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.TrackData;

/// <summary>
///     The Account Number associated to Issuer Card
/// </summary>
public record PrimaryAccountNumber : PlayProprietaryDataElement<char[]>
{
    #region Static Metadata

    /// <value>Hex: C2; Decimal: 194; Interchange: 2</value>
    public static readonly Tag Tag = CreateProprietaryTag(DataFieldId);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const int _MaxByteLength = 10;
    private const byte _MaxCharLength = 19;
    public const byte DataFieldId = 2;

    #endregion

    #region Constructor

    public PrimaryAccountNumber(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public static byte GetMaxByteLength() => _MaxByteLength;
    public override ushort GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <summary>
    ///     Checks whether the Issuer EncodingId provided in the argument matches the leftmost 3 - 8 PAN digits (allowing for
    ///     the possible padding of the Issuer EncodingId with hexadecimal 'F's)
    /// </summary>
    /// <param name="issuerIdentifier"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public bool IsIssuerIdentifierMatching(IssuerIdentificationNumber issuerIdentifier)
    {
        uint thisPan = PlayCodec.NumericCodec.DecodeToUInt16(PlayCodec.NumericCodec.Encode(_Value));

        return thisPan == (uint) issuerIdentifier;
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PrimaryAccountNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static PrimaryAccountNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        // The reason we're decoding to a char array is because some Primary Account Numbers start with
        // a '0' value. If we encoded that to a numeric value we would truncate the leading zero values
        // and wouldn't be able to encode the value back or do comparisons with the issuer identification
        // number
        ReadOnlySpan<char> result = PlayCodec.NumericCodec.DecodeToChars(value);

        Check.Primitive.ForMaxCharLength(result.Length, _MaxCharLength, Tag);

        return new PrimaryAccountNumber(result);
    }

    #endregion
}