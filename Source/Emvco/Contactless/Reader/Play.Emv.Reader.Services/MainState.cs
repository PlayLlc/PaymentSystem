namespace Play.Emv.Reader.Services;

public abstract record MainState(ReaderConfiguration ReaderConfiguration)
{
    #region Instance Values

    public readonly ReaderConfiguration ReaderConfiguration = ReaderConfiguration;

    #endregion
}