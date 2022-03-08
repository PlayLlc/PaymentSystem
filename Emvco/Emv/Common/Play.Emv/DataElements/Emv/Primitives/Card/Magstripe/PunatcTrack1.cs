using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.Emv.Primitives.Card.Magstripe;

/// <summary>
///     PUNATC(Track1) indicates to the Kernel the positions in the discretionary data field of Track 1 Data where the
///     Unpredictable Number (NumericCodec) digits and Application Transaction Counter digits have to be copied.
/// </summary>
public record PunatcTrack1 : DataElement<ulong>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F63;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public PunatcTrack1(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static PunatcTrack1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static PunatcTrack1 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value).ToUInt64Result() ?? throw new DataElementNullException(EncodingId);

        return new PunatcTrack1(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion
}