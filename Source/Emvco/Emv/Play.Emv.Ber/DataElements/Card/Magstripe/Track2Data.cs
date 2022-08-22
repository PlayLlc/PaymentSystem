using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

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
    private const byte _MaxByteLength = 19;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    public Track2Data(Track2 value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static Track2Data Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public override Track2Data Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static Track2Data Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        return new(new Track2(value.AsNibbleArray()));
    }

    public override byte[] EncodeValue() => _Value.Encode();
    public override byte[] EncodeValue(int length) => _Value.Encode()[..length];

    #endregion

    #region Instance Members

    /// <remarks>EMVco Book C-2 Section S13.17 - S13.19 && S14.26 - S14.27</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Track2Data UpdateDiscretionaryData(
        NumberOfNonZeroBits nun, CardholderVerificationCode3Track2 cvc, PositionOfCardVerificationCode3Track2 pcvc, PunatcTrack2 punatc,
        UnpredictableNumber unpredictableNumber, NumericApplicationTransactionCounterTrack2 natc, ApplicationTransactionCounter atc)
    {
        Nibble[] discretionaryData = GetTrack2DiscretionaryData().AsNibbleArray();
        byte qNumberOfDigits = (byte) pcvc.GetSetBitCount();
        byte nunNumberOfDigits = nun;
        byte tNumberOfDigits = (byte) natc;

        ReadOnlySpan<Nibble> pcvcIndexArray = pcvc.GetBitFlagIndex();
        ReadOnlySpan<Nibble> punatcIndexArray = punatc.GetBitFlagIndex();

        UpdateDiscretionaryData(discretionaryData, qNumberOfDigits, cvc, pcvcIndexArray);
        UpdateDiscretionaryData(discretionaryData, nunNumberOfDigits, unpredictableNumber, punatcIndexArray[^nunNumberOfDigits..]);
        UpdateDiscretionaryData(discretionaryData, tNumberOfDigits, natc, atc, punatcIndexArray[tNumberOfDigits..]);
        UpdateDiscretionaryData(discretionaryData, nun);

        return new Track2Data(new Track2(discretionaryData));
    }

    /// <summary>
    ///     Convert the binary encoded CVC3 (Track2) to the BCD encoding of the corresponding number expressed in base 10. Copy
    ///     the qNumberOfDigits least significant digits of the BCD encoded CVC3 (Track2) in the eligible positions of the
    ///     'Discretionary Data' in Track 2 Data. The eligible positions are indicated by the qNumberOfDigits non-zero bits in
    ///     PCVC3(Track2).
    /// </summary>
    /// <param name="discretionaryData"></param>
    /// <param name="qNumberOfDigits">Number of non-zero bits in PositionOfCardVerificationCode3Track2</param>
    /// <param name="cvc"></param>
    /// <param name="pcvcIndexArray"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <remarks>EMVco Book C-2 Section S13.18 && S14.26</remarks>
    private void UpdateDiscretionaryData(
        Span<Nibble> discretionaryData, byte qNumberOfDigits, CardholderVerificationCode3Track2 cvc, ReadOnlySpan<Nibble> pcvcIndexArray)
    {
        ReadOnlySpan<Nibble> digitsToCopy = PlayCodec.NumericCodec.DecodeToNibbles(cvc.EncodeValue())[^qNumberOfDigits..];

        if (pcvcIndexArray.Length > digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(pcvcIndexArray)}");
        }

        for (int i = 0; i < qNumberOfDigits; i++)
            discretionaryData[pcvcIndexArray[i]] = digitsToCopy[i];
    }

    /// <summary>
    ///     Replace the nunNumberOfDigits least significant eligible positions of the 'Discretionary Data' in Track 2 Data by
    ///     the nunNumberOfDigits least significant digits of Unpredictable Number (Numeric). The eligible positions in the
    ///     'Discretionary Data' in Track 2 Data are indicated by the nunNumberOfDigits least significant non-zero bits in
    ///     PUNATC(Track2).
    /// </summary>
    /// <param name="discretionaryData"></param>
    /// <param name="nunNumberOfDigits">The integer value of the <see cref="NumberOfNonZeroBits" /> calculated by the Kernel</param>
    /// <param name="unpredictableNumber"></param>
    /// <param name="punatcIndexArray"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>EMVco Book C-2 Section S13.18 && S14.26</remarks>
    private void UpdateDiscretionaryData(
        Span<Nibble> discretionaryData, int nunNumberOfDigits, UnpredictableNumber unpredictableNumber, ReadOnlySpan<Nibble> punatcIndexArray)
    {
        ReadOnlySpan<Nibble> digitsToCopy = unpredictableNumber.GetDigits()[^nunNumberOfDigits..];

        if (punatcIndexArray.Length > digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(punatcIndexArray)}");
        }

        for (int i = 0; i < nunNumberOfDigits; i++)
            discretionaryData[punatcIndexArray[i]] = digitsToCopy[i];
    }

    /// <summary>
    ///     If t ≠ 0, convert the Application Transaction Counter to the BCD encoding of the corresponding number expressed
    ///     in base 10. Replace the t most significant eligible positions of the 'Discretionary Data' in Track 2 Data by the t
    ///     least significant digits of the BCD encoded Application Transaction Counter. The eligible positions in the
    ///     'Discretionary Data' in Track 2 Data are indicated by the t most significant non-zero bits in PUNATC(Track2).
    /// </summary>
    /// <remarks>EMVco Book C-2 Section S13.18 && S14.26</remarks>
    /// <param name="discretionaryData"></param>
    /// <param name="tNumberOfDigits">
    ///     The integer value of the <see cref="NumericApplicationTransactionCounterTrack2" />
    /// </param>
    /// <param name="natc"></param>
    /// <param name="atc"></param>
    /// <param name="punatcIndexArray"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private void UpdateDiscretionaryData(
        Span<Nibble> discretionaryData, int tNumberOfDigits, NumericApplicationTransactionCounterTrack2 natc, ApplicationTransactionCounter atc,
        ReadOnlySpan<Nibble> punatcIndexArray)
    {
        if ((byte) natc == 0)
            return;

        Nibble[] digitsToCopy = PlayCodec.NumericCodec.DecodeToNibbles(atc.EncodeValue())[^tNumberOfDigits..];

        if (punatcIndexArray.Length > digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(punatcIndexArray)}");
        }

        for (int i = 0; i < tNumberOfDigits; i++)
            discretionaryData[punatcIndexArray[i]] = digitsToCopy[i];
    }

    /// <summary>Copy nUN' into the least significant digit of the 'Discretionary Data' in Track 2 Data</summary>
    /// <remarks>EMVco Book C-2 Section S13.19 && S14.27</remarks>
    private void UpdateDiscretionaryData(Span<Nibble> discretionaryData, NumberOfNonZeroBits nun)
    {
        discretionaryData[^1] = (byte) nun;
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public TrackPrimaryAccountNumber GetPrimaryAccountNumber() => _Value.GetPrimaryAccountNumber();

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public TrackDiscretionaryData GetTrack2DiscretionaryData() => _Value.GetDiscretionaryData();

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public override ushort GetValueByteCount() => (ushort) _Value.GetByteCount();

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public Track2Data CreateUpdate(TrackDiscretionaryData discretionaryData) => new(_Value.CreateUpdate(discretionaryData));

    #endregion
}