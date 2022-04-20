using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Display.Contracts;

/// <summary>
///     A request to display something on the LCD screen according to the <see cref="UserInterfaceRequestData" />
/// </summary>
public record DisplayMessageRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(DisplayMessageRequest));
    public static readonly ChannelTypeId ChannelTypeId = DisplayChannel.Id;

    #endregion

    #region Instance Values

    private readonly UserInterfaceRequestData _UserInterfaceRequestData;

    #endregion

    #region Constructor

    public DisplayMessageRequest(UserInterfaceRequestData userInterfaceRequestData) : base(MessageTypeId, ChannelTypeId)
    {
        _UserInterfaceRequestData = userInterfaceRequestData;
    }

    #endregion

    #region Instance Members

    public LanguagePreference GetLanguagePreference() => _UserInterfaceRequestData.GetLanguagePreference();
    public MessageIdentifiers GetMessageIdentifier() => _UserInterfaceRequestData.GetMessageIdentifier();
    public UserInterfaceRequestData GetUserInterfaceRequestData() => _UserInterfaceRequestData;
    public Statuses GetStatus() => _UserInterfaceRequestData.GetStatus();

    #endregion
}