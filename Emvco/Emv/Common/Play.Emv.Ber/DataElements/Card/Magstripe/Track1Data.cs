using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Track 1 Data contains the data objects of the track 1 according to [ISO/IEC 7813] Structure B, excluding start
///     sentinel, end sentinel and LRC. The Track 1 Data may be present in the file read using the READ RECORD command
///     during a mag-stripe mode transaction. It is made up of the following sub-fields:
/// </summary>
public record Track1Data : DataElement<Track1>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    public static readonly Tag Tag = 0x56;
    private const byte _MaxByteLength = 76;

    #endregion

    #region Constructor

    public Track1Data(Track1 value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public Track1DiscretionaryData GetTrack1DiscretionaryData() => _Value.GetDiscretionaryData();

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public TrackPrimaryAccountNumber GetPrimaryAccountNumber() => _Value.GetPrimaryAccountNumber();

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <remarks>EMVco Book C-2 Section S13.20 - S13.22 && S14.29 - S14.30</remarks>
    public Track1Data UpdateDiscretionaryData(
        NumberOfNonZeroBits nun, CardholderVerificationCode3Track1 cvc, PositionOfCardVerificationCode3Track1 pcvc, PunatcTrack1 punatc,
        UnpredictableNumberNumeric unpredictableNumber, NumericApplicationTransactionCounterTrack1 natc, ApplicationTransactionCounter atc)
    {
        char[] discretionaryData = GetTrack1DiscretionaryData().AsCharArray();
        byte qNumberOfChars = (byte) pcvc.GetSetBitCount();
        byte nunNumberOfChars = nun;
        byte tNumberOfChars = (byte) natc.GetSetBitCount();

        ReadOnlySpan<Nibble> pcvcIndexArray = pcvc.GetBitFlagIndex();
        ReadOnlySpan<Nibble> punatcIndexArray = punatc.GetBitFlagIndex();

        UpdateDiscretionaryData(discretionaryData, qNumberOfChars, cvc, pcvcIndexArray);
        UpdateDiscretionaryData(discretionaryData, unpredictableNumber, punatcIndexArray[^nunNumberOfChars..]);
        UpdateDiscretionaryData(discretionaryData, natc, atc, punatcIndexArray[..tNumberOfChars]);
        UpdateDiscretionaryData(discretionaryData, nun);

        return new Track1Data(new Track1(PlayCodec.AlphaNumericSpecialCodec.Encode(discretionaryData)));
    }

    /// <summary>
    ///     Convert the binary encoded CVC3 (Track1) to the BCD encoding of the corresponding number expressed in base 10.
    ///     Convert the q least significant digits of the BCD encoded CVC3 (Track1) into ASCII format and copy the q ASCII
    ///     encoded CVC3 (Track1) characters into the eligible positions of the 'Discretionary Data' in Track 1 Data. The
    ///     eligible positions are indicated by the q non-zero bits in PCVC3(Track1).
    /// </summary>
    /// <param name="discretionaryData"></param>
    /// <param name="qNumberOfDigits">Number of non-zero bits in PositionOfCardVerificationCode3Track2</param>
    /// <param name="cvc"></param>
    /// <param name="pcvcIndexArray"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <remarks>EMVco Book C-2 Section S13.21 && S14.29</remarks>
    private void UpdateDiscretionaryData(
        Span<char> discretionaryData, int qNumberOfDigits, CardholderVerificationCode3Track1 cvc, ReadOnlySpan<Nibble> pcvcIndexArray)
    {
        ReadOnlySpan<char> digitsToCopy = cvc.AsCharArray()[^qNumberOfDigits..];

        if (pcvcIndexArray.Length < digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(pcvcIndexArray)}");
        }

        for (int i = 0; i < qNumberOfDigits; i++)
            discretionaryData[pcvcIndexArray[i]] = digitsToCopy[i];
    }

    /// <summary>
    ///     Convert the BCD encoded Unpredictable Number (Numeric) into ASCII format and replace the nUN least significant
    ///     eligible positions of the 'Discretionary Data' in Track 1 Data by the nUN least significant characters of the ASCII
    ///     encoded Unpredictable Number (Numeric). The eligible positions in the 'Discretionary Data' in Track 1 Data are
    ///     indicated by the nUN least significant non-zero bits in PUNATC(Track1).
    /// </summary>
    /// <param name="discretionaryData"></param>
    /// <param name="unpredictableNumber"></param>
    /// <param name="punatcIndexArray"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <remarks>EMVco Book C-2 Section S13.21 && S14.29</remarks>
    private void UpdateDiscretionaryData(Span<char> discretionaryData, UnpredictableNumberNumeric unpredictableNumber, ReadOnlySpan<Nibble> punatcIndexArray)
    {
        ReadOnlySpan<char> digitsToCopy = unpredictableNumber.AsCharArray()[^punatcIndexArray.Length..];

        if (punatcIndexArray.Length < digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(punatcIndexArray)}");
        }

        for (int i = 0; i < punatcIndexArray.Length; i++)
            discretionaryData[punatcIndexArray[i]] = digitsToCopy[i];
    }

    /// <summary>
    ///     If t ≠ 0, convert the Application Transaction Counter to the BCD encoding of the corresponding number
    ///     expressed in base 10. Convert the t least significant digits of the BCD encoded Application Transaction Counter
    ///     into ASCII format. Replace the t most significant eligible positions of the 'Discretionary Data' in Track 1 Data by
    ///     the t ASCII encoded Application Transaction Counter characters. The eligible positions in the 'Discretionary Data'
    ///     in Track 1 Data are indicated by the t most significant nonzero bits in PUNATC(Track1).
    /// </summary>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <remarks>EMVco Book C-2 Section S13.21 && S14.29</remarks>
    private void UpdateDiscretionaryData(
        Span<char> discretionaryData, NumericApplicationTransactionCounterTrack1 natc, ApplicationTransactionCounter atc, ReadOnlySpan<Nibble> punatcIndexArray)
    {
        if ((byte) natc == 0)
            return;

        ReadOnlySpan<char> digitsToCopy = atc.AsCharArray()[..punatcIndexArray.Length];

        if (punatcIndexArray.Length < digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(punatcIndexArray)}");
        }

        for (int i = 0; i < punatcIndexArray.Length; i++)
            discretionaryData[punatcIndexArray[i]] = digitsToCopy[i];
    }

    /// <exception cref="CodecParsingException"></exception>
    private void UpdateDiscretionaryData(Span<char> discretionaryData, NumberOfNonZeroBits nun)
    {
        discretionaryData[^1] = PlayCodec.AlphaNumericCodec.DecodeToChar((byte) nun);
    }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track1Data Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public override Track1Data Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public static Track1Data Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        if (!PlayCodec.AlphaNumericSpecialCodec.IsValid(value))
            throw new DataElementParsingException($"The {nameof(Track1Data)} could not be parsed because it was in an incorrect format");

        return new Track1Data(new Track1(value));
    }

    #endregion
}