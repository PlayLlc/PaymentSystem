using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Globalization.Currency;

namespace Play.Emv.Display.Contracts;

public class DisplayMessage
{
    #region Instance Values

    protected readonly string _Message;
    internal readonly byte Identifiers;

    #endregion

    #region Constructor

    public DisplayMessage(MessageIdentifiers identifiers, string message)
    {
        _Message = message;
        Identifiers = identifiers;
    }

    #endregion

    #region Instance Members

    public string Display() => _Message;
    public virtual string Display(Money amount) => string.Format(_Message, amount);

    #endregion
}