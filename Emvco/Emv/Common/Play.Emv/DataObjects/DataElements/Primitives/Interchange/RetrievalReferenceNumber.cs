using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.DataElements.TrackData;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

internal record RetrievalReferenceNumber : PlayProprietaryDataElement<char[]>
{
    #region Static Metadata

    /// <value>Hex: 0xC025; Decimal: 49189;</value>
    public static readonly Tag Tag = 0xC025;

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    public const byte DataFieldId = 37;
    private const int _ByteLength = 12;

    #endregion

    #region Constructor

    public RetrievalReferenceNumber(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static PrimaryAccountNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static PrimaryAccountNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        char[] result = PlayCodec.AlphaNumericCodec.DecodeToChars(value);

        return new PrimaryAccountNumber(result);
    }

    #endregion
}