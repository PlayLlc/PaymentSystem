using Play.Emv.DataElements.Emv;
using Play.Globalization.Currency;

namespace Play.Emv.Display.Contracts;

public class DisplayMessage
{
    #region Instance Values

    protected readonly string _Message;
    internal readonly byte _Identifier;

    #endregion

    #region Constructor

    public DisplayMessage(MessageIdentifier identifier, string message)
    {
        _Message = message;
        _Identifier = identifier;
    }

    #endregion

    #region Instance Members

    public string Display() => _Message;
    public virtual string Display(Money amount) => string.Format(_Message, amount);

    #endregion
}