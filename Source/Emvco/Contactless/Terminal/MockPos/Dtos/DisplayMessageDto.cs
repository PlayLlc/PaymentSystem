using Play.Codecs;
using Play.Core;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Display.Configuration;

namespace MockPos.Dtos;

public class DisplayMessageDto
{
    #region Instance Values

    public string? MessageIdentifier { get; set; }
    public string? Message { get; set; }

    #endregion

    #region Serialization

    public DisplayMessage Decode()
    {
        DisplayMessageIdentifiers.Empty.TryGet(PlayCodec.HexadecimalCodec.DecodeToByte(MessageIdentifier),
            out EnumObject<byte>? displayMessageIdentifierResult);

        DisplayMessageIdentifiers displayMessageIdentifier = (DisplayMessageIdentifiers) (displayMessageIdentifierResult ?? DisplayMessageIdentifiers.Empty);

        return new DisplayMessage(displayMessageIdentifier, Message);
    }

    #endregion
}