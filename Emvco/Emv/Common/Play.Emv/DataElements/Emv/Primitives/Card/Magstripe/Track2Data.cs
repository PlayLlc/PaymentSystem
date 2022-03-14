using System;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Track 2 Data contains the data objects of the track 2 according to [ISO/IEC 7813], excluding start sentinel, end
///     sentinel and LRC. The Track 2 Data has a maximum length of 37 positions and is present in the file read using the
///     READ RECORD command during a mag-stripe mode transaction. It is made up of the following sub-fields:
/// </summary>
public record Track2Data : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F6B;
    private const byte _MaxByteLength = 19;

    #endregion

    #region Constructor

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public Track2Data(BigInteger value) : base(value)
    {
        Check.Primitive.ForMaximumValue((byte) value.GetByteCount(), _MaxByteLength, Tag);
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2Data Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2Data Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new Track2Data(result);
    }

    public new byte[] EncodeValue() => _Value.ToByteArray();
    public new byte[] EncodeValue(int length) => _Value.ToByteArray()[..length];

    #endregion
}