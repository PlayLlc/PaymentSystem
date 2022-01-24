using Play.Emv.DataElements;
using Play.Emv.Messaging;
using Play.Messaging;

using MessageIdentifier = Play.Emv.DataElements.MessageIdentifier;

namespace Play.Emv.Display.Contracts;

/// <summary>
///     A request to display something on the LCD screen according to the <see cref="UserInterfaceRequestData" />
/// </summary>
public record DisplayMessageRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(DisplayMessageRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Display;

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
    public MessageIdentifier GetMessageIdentifier() => _UserInterfaceRequestData.GetMessageIdentifier();
    public UserInterfaceRequestData GetUserInterfaceRequestData() => _UserInterfaceRequestData;
    public Status GetStatus() => _UserInterfaceRequestData.GetStatus();

    #endregion
}