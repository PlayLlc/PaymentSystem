namespace Play.Emv.DataExchange;

/// <summary>
///     Request and Response pairs implement this interface when a data exchange request originates from the Terminal
/// </summary>
public interface IExchangeDataWithTheTerminal
{
    #region Instance Members

    public DataExchangeTerminalId GetDataExchangeTerminalId();

    #endregion
}