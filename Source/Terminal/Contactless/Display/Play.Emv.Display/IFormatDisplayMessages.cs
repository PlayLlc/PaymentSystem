using Play.Emv.Ber.DataElements;

namespace Play.Emv.Display;

public interface IFormatDisplayMessages
{
    #region Instance Members

    public string Display(UserInterfaceRequestData userInterfaceRequestData);

    #endregion
}