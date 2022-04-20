using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Globalization.Time;

namespace Play.Emv.Ber.DataElements;

public record Track2EquivalentData : DataElement<Track2>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x57;

    #endregion

    #region Constructor

    public Track2EquivalentData(Track2 value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public TrackPrimaryAccountNumber GetPrimaryAccountNumber() => _Value.GetPrimaryAccountNumber();

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public ShortDate GetExpirationDate() => _Value.GetExpirationDate();

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public ServiceCode GetServiceCode() => _Value.GetServiceCode();

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public TrackDiscretionaryData GetDiscretionaryData() => _Value.GetDiscretionaryData();

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public Track2EquivalentData UpdateDiscretionaryData(TrackDiscretionaryData discretionaryData) => new(_Value.CreateUpdate(discretionaryData));

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track2EquivalentData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public override Track2EquivalentData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static Track2EquivalentData Decode(ReadOnlySpan<byte> value) => new(new Track2(value.AsNibbleArray()));

    public new byte[] EncodeValue() => _Value.Encode();
    public new byte[] EncodeValue(int length) => _Value.Encode()[..length];

    #endregion
}