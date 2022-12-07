namespace Play.Emv.Reader.Services;

public abstract record MainState(ReaderDatabase ReaderDatabase)
{
    #region Instance Values

    public readonly ReaderDatabase ReaderDatabase = ReaderDatabase;

    #endregion
}