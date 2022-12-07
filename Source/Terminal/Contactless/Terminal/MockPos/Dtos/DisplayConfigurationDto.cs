using System.Text.Json.Serialization;

using Play.Codecs;
using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Configuration;
using Play.Emv.Reader.Configuration;
using Play.Globalization.Country;
using Play.Globalization.Time;

namespace MockPos.Dtos;

public class DisplayConfigurationDto
{
    #region Instance Values

    public string? MessageHoldTime { get; set; }
    public string? NumericCountryCode { get; set; }

    [JsonPropertyName(nameof(DisplayMessageSets))]
    public List<DisplayMessageSetDto> DisplayMessageSets { get; set; } = new();

    #endregion

    #region Serialization

    public DisplayConfigurations Decode()
    {
        HoldTimeValue holdTimeValue = HoldTimeValue.Decode(PlayCodec.HexadecimalCodec.Encode(MessageHoldTime).AsSpan());
        NumericCountryCode numericCountryCode = new(PlayCodec.HexadecimalCodec.DecodeToUInt16(NumericCountryCode!));
        List<DisplayMessages> displayMessages = new();

        foreach (DisplayMessageSetDto set in DisplayMessageSets)
            displayMessages.Add(set.Decode());

        return new DisplayConfigurations(displayMessages.ToArray(), holdTimeValue, numericCountryCode);
    }

    #endregion
}