using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Display.Configuration;

public class DisplayMessage
{
    #region Instance Values

    protected readonly string _Message;
    internal readonly DisplayMessageIdentifier Identifier;

    #endregion

    #region Constructor

    public DisplayMessage(DisplayMessageIdentifiers identifier, string message)
    {
        _Message = message;
        Identifier = identifier;
    }

    #endregion

    #region Instance Members

    public DisplayMessageIdentifier GetDisplayMessageIdentifier() => Identifier;
    public string Display() => _Message;
    public virtual string Display(Money amount, CultureProfile culture) => amount.AsLocalFormat(culture);

    #endregion
}