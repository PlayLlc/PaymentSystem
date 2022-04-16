using System.Numerics;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
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
    ///     1) Convert the binary encoded CVC3 (Track2) to the BCD encoding of the corresponding number expressed in base 10.
    ///     2) Copy the q least significant digits of the BCD encoded CVC3 (Track2) in the eligible positions of the
    ///     'Discretionary Data' in Track 2 Data. The eligible positions are indicated by the q non-zero bits in PCVC3(Track2).
    ///     3) Replace the nUN least significant eligible positions of the 'Discretionary Data' in Track 2 Data by the nUN
    ///     least significant digits of Unpredictable Number (Numeric). The eligible positions in the 'Discretionary Data' in
    ///     Track 2 Data are indicated by the nUN least significant non-zero bits in PUNATC(Track2).
    ///     4) If t ≠ 0, convert the Application Transaction Counter to the BCD encoding of the corresponding number
    ///     expressed in base 10.
    ///     5) Replace the t most significant eligible positions of the 'Discretionary Data' in Track 2 Data by the t least
    ///     significant digits of the BCD encoded Application Transaction Counter. The eligible positions in the 'Discretionary
    ///     Data' in Track 2 Data are indicated by the t most significant non-zero bits in PUNATC(Track2).
    /// </summary>
    /// <remarks>EMVco Book C-2 Section S13.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Track2Data UpdateDiscretionaryData(
        CardholderVerificationCode3Track2 cvc, PositionOfCardVerificationCode3Track2 pcvc, PunatcTrack2 punatc, UnpredictableNumber unpredictableNumber,
        NumericApplicationTransactionCounterTrack2 natc, ApplicationTransactionCounter atc)
    {
        Nibble[] discretionaryData = GetTrack2DiscretionaryData().AsNibbleArray();

        // BUG: We need separately the BCD number aaaand the digits. You can't use digitsToCopy.Length
        // Convert the binary encoded CVC3 (Track2) to the BCD encoding of the corresponding number expressed in base 10. 
        Nibble[] digitsToCopy = PlayCodec.NumericCodec.DecodeToNibbles(cvc.EncodeValue());

        UpdateDiscretionaryData(discretionaryData, digitsToCopy, pcvc);
        UpdateDiscretionaryData(discretionaryData, digitsToCopy.Length, unpredictableNumber, punatc);
        UpdateDiscretionaryData(discretionaryData, digitsToCopy.Length, natc, atc, punatc);
        UpdateDiscretionaryData(discretionaryData, punatc, natc);

        return new Track2Data(new Track2(discretionaryData));
    }

    /// <remarks>EMVco Book C-2 Section S13.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void UpdateDiscretionaryData(Nibble[] discretionaryData, Nibble[] digitsToCopy, PositionOfCardVerificationCode3Track2 pcvc)
    {
        // The eligible positions are indicated by the q non-zero bits in PCVC3(Track2). 
        Nibble[] indexArray = pcvc.GetBitFlagIndex();

        if (indexArray.Length > digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(indexArray)}");
        }

        // Copy the (indexArray.Length) least significant digits of the (digitsToCopy) into Discretionary Data
        for (int i = 0, j = indexArray.Length - 1; i < indexArray.Length; i++, j--)
            discretionaryData[indexArray[i]] = digitsToCopy[j];
    }

    /// <remarks>EMVco Book C-2 Section S13.18</remarks>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateDiscretionaryData(Nibble[] discretionaryData, int numberOfDigits, UnpredictableNumber unpredictableNumber, PunatcTrack2 punatc)
    {
        Nibble[] digitsToCopy = unpredictableNumber.GetDigits(unpredictableNumber);
        Nibble[] indexArray = punatc.GetBitFlagIndex()[..numberOfDigits];

        if (indexArray.Length > digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(indexArray)}");
        }

        for (int i = 0, j = digitsToCopy.Length - 1; (i < indexArray.Length) && (j > 0); i++, j--)
            discretionaryData[indexArray[i]] = digitsToCopy[j];
    }

    /// <remarks>EMVco Book C-2 Section S13.18</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private void UpdateDiscretionaryData(
        Nibble[] discretionaryData, int numberOfDigits, NumericApplicationTransactionCounterTrack2 natc, ApplicationTransactionCounter atc, PunatcTrack2 punatc)
    {
        if ((byte) natc == 0)
            return;

        Nibble[] digitsToCopy = PlayCodec.NumericCodec.DecodeToNibbles(atc.EncodeValue())[^numberOfDigits..];
        Nibble[] indexArray = punatc.GetBitFlagIndex()[numberOfDigits..];

        if (indexArray.Length > digitsToCopy.Length)
        {
            throw new TerminalDataException(
                $"The {nameof(PositionOfCardVerificationCode3Track2)} could not {nameof(UpdateDiscretionaryData)} because the length of the {nameof(digitsToCopy)} was less than the length of the {nameof(indexArray)}");
        }

        for (int i = 0, j = indexArray.Length - 1; (i < indexArray.Length) && (j > 0); i++, j--)
            discretionaryData[indexArray[i]] = digitsToCopy[j];
    }

    /// <summary>Copy nUN' into the least significant digit of the 'Discretionary Data' in Track 2 Data</summary>
    /// <remarks>EMVco Book C-2 Section S13.19</remarks>
    private void UpdateDiscretionaryData(Nibble[] discretionaryData, PunatcTrack2 punatc, NumericApplicationTransactionCounterTrack2 natc)
    {
        NumberOfNonZeroBits nun = new(punatc, natc);
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

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exception"></exception>
    public Track2Data CreateUpdate(TrackDiscretionaryData discretionaryData) => new(_Value.CreateUpdate(discretionaryData));

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
    public static Track2Data Decode(ReadOnlySpan<byte> value) => new(new Track2(value.AsNibbleArray()));

    public new byte[] EncodeValue() => _Value.Encode();
    public new byte[] EncodeValue(int length) => _Value.Encode()[..length];

    #endregion
}