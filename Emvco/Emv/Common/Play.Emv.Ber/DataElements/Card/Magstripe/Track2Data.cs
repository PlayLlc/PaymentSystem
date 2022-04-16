using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Track 2 Data contains the data objects of the track 2 according to [ISO/IEC 7813], excluding start sentinel, end
///     sentinel and LRC. The Track 2 Data has a maximum length of 37 positions and is present in the file read using the
///     READ RECORD command during a mag-stripe mode transaction. It is made up of the following sub-fields:
/// </summary>
/// <remarks>
///     There are two formats used to encode Track 2 data. Those two different formats are represented by the 2
///     different constant start and end sentinels as well as the field separator
/// </remarks>
public record Track2Data : DataElement<Track2>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F6B;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public Track2Data(Track2 value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     GetPrimaryAccountNumber
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public TrackPrimaryAccountNumber GetPrimaryAccountNumber() => _Value.GetPrimaryAccountNumber();

    /// <summary>
    ///     GetTrack2DiscretionaryData
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public TrackDiscretionaryData GetTrack2DiscretionaryData() => _Value.GetDiscretionaryData();

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2Data Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override Track2Data Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2Data Decode(ReadOnlySpan<byte> value) => new Track2Data(new Track2(value.AsNibbleArray()));

    public new byte[] EncodeValue() => _Value.Encode();
    public new byte[] EncodeValue(int length) => _Value.Encode()[..length];

    #endregion
}