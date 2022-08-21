using System.Text.Json.Serialization;

using Play.Codecs;
using Play.Emv.Display.Configuration;
using Play.Globalization.Country;
using Play.Globalization.Language;

namespace MockPos.Dtos;

public class DisplayMessageSetDto
{
    #region Instance Values

    public string? LanguageCode { get; set; }
    public string? CountryCode { get; set; }

    [JsonPropertyName(nameof(DisplayMessages))]
    public List<DisplayMessageDto> DisplayMessages { get; set; } = new();

    #endregion

    #region Serialization

    /// <exception cref="Play.Core.Exceptions.PlayInternalException"></exception>
    public DisplayMessages Decode()
    {
        List<DisplayMessage> displayMessages = new();

        foreach (DisplayMessageDto message in DisplayMessages)
            displayMessages.Add(message.Decode());

        return new DisplayMessages(new Alpha2LanguageCode(LanguageCode), new NumericCountryCode(PlayCodec.HexadecimalCodec.DecodeToUInt16(CountryCode)),
            displayMessages.ToDictionary(a => a.GetDisplayMessageIdentifier(), b => b));
    }

    #endregion
}