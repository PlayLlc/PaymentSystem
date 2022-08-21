using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Messaging;
using Play.Globalization.Currency;
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

    public bool IsValueQualifierPresent() => _UserInterfaceRequestData.IsValueQualifierPresent();
    public Money? GetAmount() => _UserInterfaceRequestData.GetAmount();
    public LanguagePreference GetLanguagePreference() => _UserInterfaceRequestData.GetLanguagePreference();
    public DisplayMessageIdentifiers GetMessageIdentifier() => _UserInterfaceRequestData.GetMessageIdentifier();
    public UserInterfaceRequestData GetUserInterfaceRequestData() => _UserInterfaceRequestData;
    public DisplayStatuses GetStatus() => _UserInterfaceRequestData.GetStatus();
    public MessageHoldTime GetHoldTime() => _UserInterfaceRequestData.GetHoldTimeValue();

    #endregion
}