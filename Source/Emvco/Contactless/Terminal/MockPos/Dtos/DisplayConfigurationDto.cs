using System.Text.Json.Serialization;

using Play.Codecs;
using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Configuration;
using Play.Emv.Reader.Configuration;
using Play.Globalization.Time;

namespace MockPos.Dtos;

public class DisplayConfigurationDto
{
    #region Instance Values

    public string? MessageHoldTime { get; set; }

    [JsonPropertyName(nameof(DisplayMessageSets))]
    public List<DisplayMessageSetDto> DisplayMessageSets { get; set; } = new();

    #endregion

    #region Serialization

    public DisplayConfigurations Decode()
    {
        HoldTimeValue holdTimeValue = HoldTimeValue.Decode(PlayCodec.HexadecimalCodec.Encode(MessageHoldTime).AsSpan());
        List<DisplayMessages> displayMessages = new();

        foreach (DisplayMessageSetDto set in DisplayMessageSets)
            displayMessages.Add(set.Decode());

        return new DisplayConfigurations(displayMessages.ToArray(), holdTimeValue);
    }

    #endregion
}