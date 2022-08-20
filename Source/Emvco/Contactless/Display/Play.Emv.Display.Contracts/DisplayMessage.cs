using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Display.Configuration;

public class DisplayMessage
{
    #region Instance Values

    protected readonly string _Message;
    internal readonly DisplayMessageIdentifier Identifiers;

    #endregion

    #region Constructor

    public DisplayMessage(DisplayMessageIdentifiers identifiers, string message)
    {
        _Message = message;
        Identifiers = identifiers;
    }

    #endregion

    #region Instance Members

    public string Display() => _Message;
    public virtual string Display(Money amount, CultureProfile culture) => amount.AsLocalFormat(culture);

    #endregion
}